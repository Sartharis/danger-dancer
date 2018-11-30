using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

struct SData
{
    public Rect rect;
    public Vector2 pivot;
};

public class DynamicSprite : MonoBehaviour {
    public Texture spritesheet;
    public int rows;
    public int cols;
    // Use this for initialization
    void Start () {
        List<SData> newData = new List<SData>();
        float sliceWidth = spritesheet.width / cols;
        float sliceHeight = spritesheet.height / rows;
        int n = 0;
        for (int i = 0; i < rows; i++){
            for (int j = 0; j < cols; j++){
                SData smd = new SData();
                smd.pivot = new Vector2(0.5f, 0.5f);
                smd.rect = new Rect(i * sliceWidth, j * sliceHeight, sliceWidth, sliceHeight);
                newData.Add(smd);
                n++;
            }
        }
        System.Random rnd = new System.Random();
        int r = rnd.Next(0, n);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create((UnityEngine.Texture2D)spritesheet, newData[r].rect, newData[r].pivot);
    }
	
	// Update is called once per frame
}
