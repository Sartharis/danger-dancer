using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using System;
using System.Text.RegularExpressions;

public class GhostRecorder : UnitySingletonPersistent<GhostRecorder> {
    private int tm;
    private List<ReplayAction> acts;

    private void Start()
    {
        tm = 0;
        acts = new List<ReplayAction>();
    }
    public void addAction(int t, Vector2 d, String a)
    {
        acts.Add(new ReplayAction { Atime = t, Direction = d, Action = a });
    }

    public void endRecording(int t){
        tm = t;
        XmlSerializer xmls = new XmlSerializer(typeof(ReplayData));
        StreamWriter sw = new StreamWriter("Replay.xml");
        xmls.Serialize(sw.BaseStream, new ReplayData { Time = tm, Actions = acts });
        sw.Close();
    }
}

[XmlRoot]
public class ReplayData{
    [XmlElement]
    public int Time;

    [XmlArray]
    public List<ReplayAction> Actions;


}

[XmlRoot]
public class ReplayAction{
    [XmlElement]
    public int Atime;

    [XmlElement]
    public Vector2 Direction;

    [XmlElement]
    public String Action;
}

