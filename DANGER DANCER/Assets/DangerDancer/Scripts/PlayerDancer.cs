﻿using UnityEngine;
using System.Collections;

public enum EActionState
{
    AS_IDLE,
    AS_SPIN,
    AS_POSE
}

public class PlayerDancer : MonoBehaviour
{

    [Header("Run")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runAttackAcceleration;
    [SerializeField] private float runAttackMaxSpeed;
    [SerializeField] private float runDecayDeceleration;
    [SerializeField] private float runDecaySpeed;
    [SerializeField] private float runSustainAcceleration;
    [SerializeField] private float runSustainMaxSpeed;
    [SerializeField] private float runReleaseDeceleration;
    [SerializeField] private float runHaltingDeceleration;
    [SerializeField] private float runManouverability;
    [SerializeField] private float turnDeceleration;
    [SerializeField] private float accelerationModifierLerp;

    [Header("Control")]
    [SerializeField] private float minInput;
    [SerializeField] private float playerControlRegen;
    [SerializeField] private float poseDuration;
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinMaxSpeed;
    [SerializeField] private AnimationCurve spinCurve;

    [Header("Collision")]
    [SerializeField] private LayerMask wallMask;

    [Header("Components")]
    public Shaker bodyshaker;

    public EActionState actionState;
    private Vector2 actionStartPoint;
    private ERunState runState;
    public Vector2 moveDir;
    private float moveSpeed;
    private int fallTicks;
    private float playerControlFactor;
    private float accelerationModifier;
    private Vector2 queuedDir;

    private float actionTimer;

    private Rigidbody2D body;
    private Animator anim;
    private SpriteRenderer headsprite;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bodyshaker = transform.Find("Body").GetComponent<Shaker>();;
        headsprite = transform.Find("Body").transform.Find("Head").GetComponent<SpriteRenderer>();
        moveSpeed = walkSpeed;
        BeatManager.Instance.OnBeat += OnBeat;
    }


    // UPDATE---------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void Update()
    {
        if (playerControlFactor < 1.0f)
        {
            playerControlFactor += playerControlRegen * Time.deltaTime;
        }
        playerControlRegen = Mathf.Clamp(playerControlRegen, 0, 1);

        if (Input.GetButtonDown("Fire1"))
        {
            if (isFallen())
            {
                if (fallTicks > 1) bodyshaker.shake += 0.1f;
                fallTicks -= 1;
                if (fallTicks <= 0)
                {
                    body.velocity = new Vector2();
                }
            }
            else
            {
                StartPose();
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (isFallen())
            {
                if (fallTicks > 1) bodyshaker.shake += 0.1f;
                fallTicks -= 1;
                if (fallTicks <= 0)
                {
                    body.velocity = new Vector2();
                }
            }
            else
            {
                StartSpin();
            }
        }

        ActionsUpdate();
        if (canMove())
        {
            MoveUpdate();
        }
        AnimUpdate();
    }


    // MOVEMENT-------------------------------------------------------------------------------------------------------------------------------------------------

    public bool isFallen()
    {
        return fallTicks > 0;
    }

    public bool canMove()
    {
        return !isFallen();
    }

    public bool isMoving()
    {
        return moveSpeed > 0;
    }

    public bool isRunning()
    {
        return moveSpeed > walkSpeed;
    }

    public Vector2 getRunVector()
    {
        return moveDir * moveSpeed;
    }

    public void ModifySpeed(float speedModifier)
    {
        moveSpeed = Mathf.Max(0,moveSpeed + speedModifier);
    }

    public void ModifyAcceleration(float accelerationMod)
    {
        accelerationModifier *= accelerationMod;
    }

    public void Fall(Vector2 fallForce)
    {
        if (!isFallen())
        {
            //moveSpeed = 0;
            fallTicks = 3;
            //body.AddForce(fallForce);

            ScoreManager.Instance.AddScore(-15, "Oh no", transform.position);
        }
    }

    private float GetTargetRotation()
    {
        if (isMoving())
        {
            return Mathf.Rad2Deg * Mathf.Atan2(moveDir.y, moveDir.x);
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float dist = 0;
        (new Plane(new Vector3(0.0f, 0.0f, 1.0f), transform.position)).Raycast(ray, out dist);
        Vector3 point = ray.GetPoint(dist);
        Vector3 dirvec = point - transform.position;
        return Mathf.Rad2Deg * Mathf.Atan2(dirvec.y, dirvec.x);
    }

    private void OnBeat()
    {
        if(queuedDir.magnitude >= minInput)
        {
             StartSpin();
        }
        queuedDir = new Vector2();
    }

    private void MoveUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Mathf.Abs(h) >= Mathf.Abs(v))
        {
            queuedDir = new Vector2(h,0);
        }
        else
        {
            queuedDir = new Vector2(0,v);
        }
        

        if (queuedDir.magnitude >= minInput && BeatManager.Instance.IsOnBeat() && CanDoAction())
        {
            StartSpin();
            queuedDir = new Vector2();
        }
    }


    // ACTIONS-----------------------------------------------------------------------------------------------------------------------------------------------------

    public bool CanDoAction()
    {
        return actionState == EActionState.AS_IDLE && !isFallen();
    }

    public void StartPose()
    {
        if (CanDoAction())
        {
            actionStartPoint = transform.position;
            actionState = EActionState.AS_POSE;
            actionTimer = poseDuration;
            anim.Play("Smashing");
            bodyshaker.shake += 0.1f;

//             Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 0.2f);
//             foreach(Collider2D col in colls)
//             {
//                 PoseZone pZone = col.GetComponent<PoseZone>();
//                 if(pZone)
//                 {
//                     pZone.OnPose();
//                     break;
//                 }
//             }
        }
    }

    public void StartSpin()
    {
        if (CanDoAction())
        {
            float h = queuedDir.x;
            float v = queuedDir.y;

            if (Mathf.Sqrt(h * h + v * v) >= minInput)
            {
                float yaw = Mathf.Atan2(v, h);
                float dir_x = Mathf.Cos(yaw);
                float dir_y = Mathf.Sin(yaw);
                moveDir = new Vector2(dir_x,dir_y);
            }

            actionStartPoint = transform.position;
            actionState = EActionState.AS_SPIN;
            actionTimer = spinDuration;
            anim.Play("Smashing");
            bodyshaker.shake += 0.2f;
        }
    }

    public void ActionsUpdate()
    {
        if (actionState != EActionState.AS_IDLE)
        {
            if (actionTimer > 0.0f)
            {
                actionTimer -= Time.deltaTime;
            }

            if (actionTimer <= 0.0f)
            {
                actionState = EActionState.AS_IDLE;
            }

            if (actionState == EActionState.AS_POSE)
            {
                float x_f = moveDir.x * moveSpeed * Time.deltaTime;
                float y_f = moveDir.y * moveSpeed * Time.deltaTime;
                transform.position = new Vector2(body.position.x + x_f, body.position.y + y_f);

                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.LerpAngle(transform.rotation.eulerAngles.z,
                                                actionTimer * 360 * 6f,
                                                0.2f)));

                moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.1f);
            }
            else if (actionState == EActionState.AS_SPIN)
            {           
                float spinSpeed = spinMaxSpeed * spinCurve.Evaluate(1 - (actionTimer / spinDuration));
                float x_f = moveDir.x * (moveSpeed + spinSpeed) * Time.deltaTime;
                float y_f = moveDir.y * (moveSpeed + spinSpeed) * Time.deltaTime;
                transform.position = new Vector2(body.position.x + x_f, body.position.y + y_f);

                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.LerpAngle(transform.rotation.eulerAngles.z,
                                                actionTimer * 360 * 4,
                                                0.2f)));

                moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.1f);
            }
        }
    }


    // ANIMATIONS--------------------------------------------------------------------------------------------------------------------

    private void AnimUpdate()
    {
        headsprite.enabled = !isFallen();
        anim.SetBool("Fallen", isFallen());
        anim.SetBool("Running", isRunning());
        anim.SetBool("Moving", isMoving());
    }


    // COLLISIONS--------------------------------------------------------------------------------------------------------------------------------------------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Danger")
        {
            Fall(new Vector2());
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
       playerControlFactor = 0;
       moveDir = Vector2.Reflect(moveDir, coll.contacts[0].normal);
        if (actionState == EActionState.AS_SPIN)
        {
            ScoreManager.Instance.AddScore(5, "Spin bounce", transform.position);
        }
    }
}
