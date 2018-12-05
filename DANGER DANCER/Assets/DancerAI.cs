using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell
{
    public Transform trans;
    public int x;
    public int y;
    public Cell(Transform t, int ty, int tx)
    {
        trans = t;
        x = tx;
        y = ty;
    }
    public bool Equal(Cell c)
    {
        return this.x == c.x && this.y == c.y &&
            Mathf.Approximately(this.x, c.x) && Mathf.Approximately(this.y, c.y);
    }
}

public class TargetCell : UnitySingleton<TargetCell>
{
    public List<Cell> targets;
    public Cell[,] grid;
    void Start()
    {
        targets = new List<Cell>();
        grid = new Cell[4, 5];
        for (int i = 10; i < 30; i++)
        {
            grid[(i - 10) / 5, (i - 10) % 5] = new Cell(SpawnManager.Instance.spawnPositions[i], (i - 10) / 5, (i - 10) % 5);
        }
    }

    public Cell GetTarget(Transform t)
    {
        Cell temp = null;
        if (targets == null || targets.Count == 0)
        {
            return temp;
        }
        float mdist = 1000000000;
        foreach (var v in targets)
        {
            if(v.trans != null && Mathf.Abs(v.trans.position.x - t.position.x) + Mathf.Abs(v.trans.position.y - t.position.y) < mdist)
            {
                mdist = Mathf.Abs(v.trans.position.x - t.position.x) + Mathf.Abs(v.trans.position.y - t.position.y);
                temp = v;
            }
        }
        return temp;
    }

    Cell GetCell(Transform t)
    {
        if (grid == null) return null;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {

                if (grid[i, j] == null) return null;
                if (Mathf.Approximately(t.position.x, (grid[i, j]).trans.position.x) && Mathf.Approximately(t.position.y, (grid[i, j]).trans.position.y))                if (Mathf.Approximately(t.position.x, (grid[i, j]).trans.position.x) && Mathf.Approximately(t.position.y, (grid[i, j]).trans.position.y))
                {
                    return grid[i,j];
                }
            }
        }
        return null;
    }
    public void AddTarget(Transform c)
    {
        Cell ce = GetCell(c);
        if (ce != null)
        {
            targets.Add(GetCell(c));
        }
    }

    public void RemTarget(Transform c)
    {
        Cell ce = GetCell(c);
        if (ce != null)
        {
            targets.Remove(GetCell(c));
        }
    }
}

public class NodeCost : UnitySingleton<NodeCost>
{
    int[,] costs;
    public Cell[,] grid;
    void Start()
    {
        costs = new int[4, 5];
        grid = new Cell[4, 5];
        for (int i = 10; i < 30; i++)
        {
            grid[(i - 10) / 5, (i - 10) % 5] = new Cell(SpawnManager.Instance.spawnPositions[i], (i - 10) / 5, (i - 10) % 5);
        }
    }
    public void updateCost(Transform t, int c)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {

                if (Mathf.Approximately(t.position.x, (grid[i, j]).trans.position.x) && Mathf.Approximately(t.position.y, (grid[i, j]).trans.position.y))
                {
                    costs[i,j] = c;
                }
            }
        }
    }

    public int getCost(Cell c)
    {
        return costs[c.y, c.x];
    }
}


public class DancerAI : MonoBehaviour {
    class Node
    {
        public Cell data;
        public Node prev;
        public int cost;
        public int moves;
        public Node(Cell d, Node p, int c,int m)
        {
            data = d;
            prev = p;
            cost = c;
            moves = m;
        }
        public void UpdateNode(Node p, int c, int m)
        {
            prev = p;
            cost = c;
            moves = m;
        }
    }
    public int moveDelay;
    int beats;
    Cell current;
    Cell target;
    bool instantiated = false;
    public Cell[,] grid;
    public bool tptospawnpoint;
    // Use this for initialization
    void Start () {
        grid = NodeCost.Instance.grid;
        if (TargetCell.Instance.GetTarget(this.transform) != null) target = TargetCell.Instance.GetTarget(this.transform);
        if (grid != null && target != null)
        {

            instantiated = true;
            if (tptospawnpoint && grid[2,2].trans != null)
            {
                Vector3 pos = grid[2, 2].trans.position;
                pos.z = this.gameObject.transform.position.z;
                this.gameObject.transform.position = pos;
            }
            beats = moveDelay;
            BeatManager.Instance.OnBeat += OnBeat;
        }
    }

    void UpdatePositions()
    {
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j< 5; j++)
            {

                if (Mathf.Approximately(this.gameObject.transform.position.x,(grid[i,j]).trans.position.x) && Mathf.Approximately(this.gameObject.transform.position.y, (grid[i, j]).trans.position.y))
                {
                    current = new Cell((grid[i, j]).trans, i,j);
                }
            }
        }

    }

    List<Cell> GetNeighbors(Node n)
    {
        List<Cell> temp = new List<Cell>();
        int x = n.data.x;
        int y = n.data.y;
        if((x-1) >= 0)
        {
            temp.Add(grid[y, x - 1]);
        }
        if((x+1) < 5)
        {
            temp.Add(grid[y, x + 1]);
        }
        if((y-1) >= 0)
        {
            temp.Add(grid[y - 1, x]);
        }
        if((y+1) < 4)
        {
            temp.Add(grid[y + 1, x]);
        }
        return temp;
    }

    int predictDist(Cell c)
    {
        return Mathf.Abs(c.x-target.x) + Mathf.Abs(c.y - target.y);
    }

    bool containsCell(List<Node> nodes, Cell c)
    {
        foreach(var n in nodes)
        {
            if(c.Equal(n.data))
            {
                return true;
            }
        }
        return false;
    }

    Node CalculatePath()
    {
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        open.Add(new Node(current, null, 0,0));
        Node t = null;
        int count = 0;
        while(open.Count != 0)
        {
            Node cur = open.First();
            List<Cell> temp = GetNeighbors(cur);
            Cell tc = null;
            closed.Add(cur);
            open.Remove(cur);
            while (temp.Count != 0)
            {
                tc = temp.First();
                temp.RemoveAt(0);
                if (!containsCell(closed, tc))
                {
                    open.Add(new Node(tc, cur, cur.moves + 1 + predictDist(tc) + NodeCost.Instance.getCost(tc), cur.moves + 1));
                    open = open.OrderBy(o => o.cost).ToList();
                }
            }
            if (target.Equal(cur.data) && tc != null)
            {
                t = new Node(tc, cur, cur.moves + 1 + predictDist(tc) + NodeCost.Instance.getCost(tc), cur.moves + 1);
                break;
            }
        }
        if(t != null)
        {
            while(!t.prev.data.Equal(current))
            {
                t = t.prev;
            }
            return t;
        }
        return t;
    }

    void OnBeat()
    {
        if (instantiated)
        {
            beats--;
            if (beats <= 0)
            {
                UpdatePositions();
                if (!current.Equal(target))
                {
                    //this.GetComponent<PlayerDancer>().actionState = EActionState.AS_SPIN;
                    Vector3 pos = CalculatePath().data.trans.position;
                    pos.z = this.gameObject.transform.position.z;
                    this.gameObject.transform.position = pos;
                }
                else
                {
                    //this.GetComponent<PlayerDancer>().actionState = EActionState.AS_POSE;
                    TargetCell.Instance.RemTarget(target.trans);
                    if(TargetCell.Instance.GetTarget(this.transform) != null)
                    {
                        target = TargetCell.Instance.GetTarget(this.transform);
                    }
                }
                beats = moveDelay;
            }
        }
    }

    void Update()
    {
        if (!instantiated)
        {
            Start();
        }
    }
}
