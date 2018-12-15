using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class dialogue : MonoBehaviour {
  // public GameObject myObject = gameObject;
	// Use this for initialization
  // private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>(GetComponentsInChildren<gameObject>());
  private List<Transform> sprites;
  // private Transfom target;
  private Vector3 targetPos = new Vector3(15.0f,-17.3f,-5.0f);
  // private float targetX;
  private int idx = 0;
  public float moveSpeed=1.5f;
  public float timedelay = 1.0f;
    private float initialtimedelay;
    private bool started = false;
   
	void Start () {
        initialtimedelay = timedelay;
    // targetPos = new Vector3(9.5f,-6.5f,0.0f);
    // Debug.Log(targetPos);
    sprites = gameObject.transform.Cast<Transform>().ToList();
		// Dialogue();
	}

    public void StartDialogue()
    {
        started = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
    if(started)
        {

            if (idx < sprites.Count && idx % 2 == 0 && sprites[idx].position.x > 15.0f)
            {
                sprites[idx].position = new Vector3(sprites[idx].position.x - moveSpeed, sprites[idx].position.y, sprites[idx].position.z);
            }
            else if (idx < sprites.Count && idx % 2 == 1 && sprites[idx].position.x < 15.0f)
            {
                sprites[idx].position = new Vector3(sprites[idx].position.x + moveSpeed, sprites[idx].position.y, sprites[idx].position.z);
            }

            if (timedelay > 0)
            {
                timedelay -= Time.deltaTime;
            }

            if (idx < sprites.Count && (timedelay <= 0 || (timedelay < initialtimedelay - 0.2f && Input.GetButtonDown("Fire1"))))
            {
                timedelay = initialtimedelay;
                sprites[idx].GetComponent<SpriteRenderer>().enabled = false;
                idx++;
                if(idx >= sprites.Count)
                {
                    LevelManager.Instance.FinishDialogue();
                    Destroy(gameObject);
                }
            }

        }

    }

  void Dialogue (){
    // for(int i = 0; i < spriteRenders.Count; ++i)
    //  {
    //    // store the colour first
    //     Debug.Log("hi");
    //  }
    int count = 0;
    foreach (Transform child in gameObject.transform)
    {
      if (count % 2 == 0)
      {
        child.position = new Vector3(15.0f,-17.3f,-5.0f);
      }
      count ++;
      //child is your child transform
    }
  }
}
