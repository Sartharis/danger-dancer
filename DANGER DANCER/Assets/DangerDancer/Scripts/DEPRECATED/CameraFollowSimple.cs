using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSimple : MonoBehaviour
{
    [SerializeField] private Transform m_Target;
    [SerializeField, Range(0,1)] private float m_TargetLerp = 0.1f;
    [SerializeField, Range(0,1)] private float m_OffsetLerp = 0.1f;
    [SerializeField, Range(0,1)] private float m_MoveOffsetStrength = 0.4f;
    public float m_Shake;
    private Vector3 m_CenterPos;
    private Vector3 m_OffsetPos;

    private void Start()
    {
        m_CenterPos = transform.position;
        m_OffsetPos = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (m_Target != null)
        {
            Vector3 offsetVec = new Vector3();//(Vector3)(m_Target.getRunVector()) * m_MoveOffsetStrength;
            m_OffsetPos = Vector3.Lerp(m_OffsetPos, offsetVec, m_OffsetLerp);

            m_CenterPos = new Vector3(Mathf.Lerp(m_CenterPos.x, m_Target.transform.position.x + offsetVec.x, m_TargetLerp)
                                              , Mathf.Lerp(m_CenterPos.y, m_Target.transform.position.y + offsetVec.y, m_TargetLerp)
                                              , m_CenterPos.z);
        }
        Vector2 shakeRandom = Random.insideUnitCircle * m_Shake;
        transform.position = new Vector3(m_CenterPos.x + shakeRandom.x, m_CenterPos.y + shakeRandom.y, m_CenterPos.z) + m_OffsetPos;
    }
}
