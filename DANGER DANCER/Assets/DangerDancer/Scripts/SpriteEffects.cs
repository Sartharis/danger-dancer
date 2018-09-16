using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffects : MonoBehaviour
{

    public float deformX = 0f;
    public float deformY = 0f;
    private float deformLerp = 0.1f;

    public float deformShakeX = 0f;
    public float deformShakeY = 0f;
    private float deformShakeLerp = 0.05f;

    public float rippleDeformX = 0f;
    public float rippleDeformY = 0f;
    [SerializeField] private float rippleDeformReturnForce = 2000f;
    private float currentRippleForceX;
    private float currentRippleForceY;
    private Vector3 initialScale;

    [Range(0, 1)] [SerializeField] private float rippleDamping = 0.975f;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    private void Update()
    {
        deformX = Mathf.Lerp(deformX, 0, deformLerp);
        deformY = Mathf.Lerp(deformY, 0, deformLerp);

        deformShakeX = Mathf.Lerp(deformShakeX, 0, deformShakeLerp);
        deformShakeY = Mathf.Lerp(deformShakeY, 0, deformShakeLerp);

        currentRippleForceX += Mathf.Sign(rippleDeformX) * (rippleDeformX * rippleDeformX * rippleDeformReturnForce * Time.deltaTime);
        currentRippleForceY += Mathf.Sign(rippleDeformY) * (rippleDeformY * rippleDeformY * rippleDeformReturnForce * Time.deltaTime);

        currentRippleForceX *= rippleDamping;
        currentRippleForceY *= rippleDamping;

        rippleDeformX -= currentRippleForceX * Time.deltaTime;
        rippleDeformY -= currentRippleForceY * Time.deltaTime;

        transform.localScale = initialScale + new Vector3(deformX + rippleDeformX + Random.Range(-1,1)*deformShakeX, 
                                                          deformY + rippleDeformY + Random.Range(-1,1)*deformShakeY, 
                                                          0);

    }
}
