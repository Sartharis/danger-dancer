using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;

public class XMLDeserializer : MonoBehaviour {
    XDocument xmlDoc;
    IEnumerable<XElement> items;
    List<SpawnEvent> spawnEvents = new List<SpawnEvent>();
    int iteration = 0, pageNum = 0;
    string charText, dialogueText;
    bool finishedLoading = false;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        LoadXML();
    }

    // Update is called once per frame
    void Update()
    {
        if (finishedLoading){
            Debug.Log("New");
            foreach(var e in spawnEvents){
                Debug.Log("Beat " + e.getBeat().ToString());
                Debug.Log("Index " + e.getIndex().ToString());
                Debug.Log("Spawned " + e.getSpawned());
            }
        }

    }

    void LoadXML(){
        xmlDoc = XDocument.Load("Assets/Resources/XML/test.xml");
        items = xmlDoc.Descendants("eventList").Elements();
        foreach(var item in items){
            string[] values = item.Value.Split(',');
            //Debug.Log("happening");
            spawnEvents.Add(new SpawnEvent(values[0], values[1], values[2]));
        }
        finishedLoading = true;
    }
}

public class SpawnEvent{
    int beat;
    int index;
    string spawned;

    public SpawnEvent(string b, string i, string s){
        beat = int.Parse(b);
        index = int.Parse(i);
        spawned = s;
    }

    public int getBeat(){
        return beat;
    }

    public int getIndex()
    {
        return index;
    }

    public string getSpawned(){
        return spawned;
    }
}
