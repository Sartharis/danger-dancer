using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DynamicSprite : MonoBehaviour {
    public Texture spritesheet;
    public int rows;
    public int cols;
    // Use this for initialization
    void Start () {
        List<SpriteMetaData> newData = new List<SpriteMetaData>();
        float sliceWidth = spritesheet.width / cols;
        float sliceHeight = spritesheet.height / rows;
        int n = 0;
        for (int i = 0; i < rows; i++){
            for (int j = 0; j < cols; j++){
                SpriteMetaData smd = new SpriteMetaData();
                smd.pivot = new Vector2(0.5f, 0.5f);
                smd.alignment = 9;
                smd.name = spritesheet.name + "_" + n.ToString();
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
