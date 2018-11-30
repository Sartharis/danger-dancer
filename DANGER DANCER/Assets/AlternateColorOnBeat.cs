using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateColorOnBeat : MonoBehaviour {

    MeshRenderer meshrend;
    public bool useColor1 = false;
    public Color col1;
    public Color col2;

    // Use this for initialization
    void Start()
    {
        BeatManager.Instance.OnBeat += OnBeat;
        meshrend = GetComponent<MeshRenderer>();
        meshrend.material.color = useColor1 ? col1 : col2;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnBeat()
    {
        useColor1 = !useColor1;
        meshrend.material.color = useColor1 ? col1 : col2;
    }
}
