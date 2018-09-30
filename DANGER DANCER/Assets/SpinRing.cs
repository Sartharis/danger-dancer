using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinRing : MonoBehaviour
{

    public void OnSpin()
    {
        ScoreManager.Instance.AddScore(10, "Spin Ring", transform.position);
        Destroy(gameObject);
    }
    
}
