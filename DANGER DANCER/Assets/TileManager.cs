using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
public class TileManager : UnitySingleton<TileManager>
{

    private int beat;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] public List<SpawnTileEvent> spawnList;
    public TextAsset tileData;
    public Vector3 startTilePos;
    public Vector3 spawnTileOffset;
    private int spawnIndex;

    private void Start()
    {
        beat = 0;
        spawnList = null;
        spawnIndex = 0;
    }

    public void StartSpawning()
    {
        beat = 0;
        spawnIndex = 0;
        BeatManager.Instance.OnBeat += incrementBeat;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnList == null)
        {
            spawnList = XMLDeserializer.Instance.getSpawnTileEvent();
        }
    }

    void incrementBeat()
    {
        beat += 1;
        spawnObjects();
    }

    void spawnObjects()
    {
        if (spawnList != null)
        {
            while (spawnIndex < spawnList.Count && beat >= spawnList[spawnIndex].beat)
            {
                Vector3 pos = spawnList[spawnIndex].pos;
                for (int i = 0; i < spawnList[spawnIndex].tiles.Count; i++){
                    string tile = spawnList[spawnIndex].tiles[i];
                    GameObject obj = GetComponent<TileDict>().get(tile);
                    Debug.Log(pos);
                    Debug.Log(obj);
                    Instantiate(obj, pos, Quaternion.identity);
                    pos.x = pos.x + spawnList[spawnIndex].offset.x;
                    if ((i + 1) % spawnList[spawnIndex].width == 0){
                        pos.x = spawnList[spawnIndex].pos.x;
                        pos.y = pos.y + spawnList[spawnIndex].offset.y;
                    }
                }

                spawnIndex += 1;
            }
        }
    }
}
