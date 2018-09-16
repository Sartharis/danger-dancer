using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TargetingPointer : MonoBehaviour
{
    [SerializeField] Transform m_PointerCenter;
    [SerializeField] float m_PointerRadius;
    [SerializeField] string m_PointerFollowTag;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		GameObject target= GameObject.FindGameObjectWithTag(m_PointerFollowTag);
        Vector2 dirVector=  target.transform.position - m_PointerCenter.position; 
        float yaw= Mathf.Rad2Deg * Mathf.Atan2(dirVector.y, dirVector.x);

        transform.position= m_PointerCenter.position + (Vector3)(dirVector.normalized * m_PointerRadius);
        transform.eulerAngles= new Vector3(0,0,yaw);
	}
}
