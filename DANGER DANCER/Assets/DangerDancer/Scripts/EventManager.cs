using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : UnitySingleton<BeatManager>
{
    private Dictionary<string, List<float>> intervals;
    private float bpm;
    private float globalTimer = 0;
    void Start()
    {
        intervals = new Dictionary<string, List<float>>();
        bpm = BeatManager.Instance.getBPM();
        PopulateTags();

    }

    void Update()
    {
        globalTimer += Time.deltaTime;
    }

    private List<float> CreateInterval(float a, float b)
    {
        List<float> interval = new List<float>();
        interval.Add(a);
        interval.Add(b);
        return interval;
    }

    void PopulateTags()
    {
        intervals.Add("Police", CreateInterval(0f,10f));
        intervals.Add("Constant", CreateInterval(20f,100f));
    }

    public bool CheckTag(string tag)
    {
        try
        {
            List<float> interval = intervals[tag];
            return System.Math.Abs(interval[1]) < double.Epsilon || (globalTimer >= interval[0] && globalTimer <= interval[1]);
        }
        catch
        {
            return false;
        }

    }
}
