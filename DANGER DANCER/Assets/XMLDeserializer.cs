using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using System;
using System.Text.RegularExpressions;
public class XMLDeserializer : UnitySingletonPersistent<XMLDeserializer> {
    XDocument xmlDoc;
    IEnumerable<XElement> items;
    List<SpawnEvent> spawnEvents = new List<SpawnEvent>();
    List<SpawnTileEvent> spawnTileEvents = new List<SpawnTileEvent>();
    int iteration = 0, pageNum = 0;
    string charText, dialogueText;
    bool finishedLoading = false;

	// Use this for initialization
	void Start ()
    {
        LoadXML();
    }

    // Update is called once per frame
    void Update()
    {
        // if (finishedLoading){
        //     Debug.Log("New");
        //     foreach(var e in spawnEvents){
        //         Debug.Log("Beat " + e.getBeat().ToString());
        //         Debug.Log("Index " + e.getIndex().ToString());
        //         Debug.Log("Spawned " + e.getSpawned());
        //     }
        // }

    }

    void LoadXML(){
        xmlDoc = XDocument.Parse(SpawnManager.Instance.spawnData.ToString());
        items = xmlDoc.Descendants("eventList").Elements();
        foreach(var item in items){
            string[] values = item.Value.Split(',');
            spawnEvents.Add(new SpawnEvent(values[0], values[1], values[2]));
        }
        xmlDoc = XDocument.Parse(TileManager.Instance.tileData.ToString());
        items = xmlDoc.Descendants("eventList").Elements();
        foreach(var item in items){
            string b = item.Element("beat").Value;
            string w = item.Element("width").Value;
            string h = item.Element("height").Value;
            string t = Regex.Replace(item.Element("data").Value, "\\s", String.Empty);
            spawnTileEvents.Add(new SpawnTileEvent(b, t, w, h));
        }
        finishedLoading = true;
    }

    public List<SpawnEvent> getSpawnEvent(){
        if(finishedLoading){
            return spawnEvents;
        }else{
            return null;
        }
    }

    public List<SpawnTileEvent> getSpawnTileEvent()
    {
        if (finishedLoading)
        {
            return spawnTileEvents;
        }
        else
        {
            return null;
        }
    }
}

[System.Serializable]
public struct SpawnEvent{
    public int beat;
    public int index;
    public string spawned;
    public SpawnEvent(string b, string i, string s){
        beat = int.Parse(b);
        index = int.Parse(i);
        spawned = s;
    }
}

[System.Serializable]
public struct SpawnTileEvent
{
    public Vector3 pos;
    public Vector3 offset;
    public List<string> tiles;
    public int beat;
    public int width;
    public int height;
    public SpawnTileEvent(string b, string t, string w, string h)
    {
        beat = int.Parse(b);
        pos = TileManager.Instance.startTilePos;
        offset = TileManager.Instance.spawnTileOffset;
        tiles = new List<string>();
        string[] temp = t.Split(',');
        foreach (string ti in temp){
            tiles.Add(ti);
        }
        width = int.Parse(w);
        height = int.Parse(h);
        if(width * height != tiles.Count){
            throw new Exception("width * height not equal to tile data array size");
        }
    }
}