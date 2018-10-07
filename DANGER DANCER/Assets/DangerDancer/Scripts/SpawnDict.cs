using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnDict : MonoBehaviour
{
	[SerializeField] List<string> keys;
	[SerializeField] List<GameObject> values;

	public GameObject get(string k)
	{
		int i = keys.IndexOf (k);
		return values [i];
	}
}
