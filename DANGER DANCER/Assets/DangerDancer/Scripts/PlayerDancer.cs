using UnityEngine;
using System.Collections;

enum EActionState
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

    [Header("Collision")]
    [SerializeField] private LayerMask wallMask;

    [Header("Components")]
    public Shaker bodyshaker;

    private EActionState actionState;
    private ERunState runState;
    private Vector2 moveDir;
    private float moveSpeed;
    private int fallTicks;
    private float playerControlFactor;
    private float accelerationModifier;

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
        return !isFallen() && actionState == EActionState.AS_IDLE;
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

    private void MoveUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Mathf.Sqrt(h * h + v * v) >= minInput)
        {
            float yaw = Mathf.Atan2(v, h);
            float dir_x = Mathf.Cos(yaw);
            float dir_y = Mathf.Sin(yaw);

            //If running, we don't immediately switch direction but move towards target direction
            if (isRunning())
            {
                Vector2 moveVector = moveDir;
                Vector2 addVector = new Vector2(dir_x, dir_y);

                moveSpeed -= (1 - Mathf.Max(0, Vector2.Dot(moveVector, addVector))) * turnDeceleration;
                moveDir = ((moveDir * moveSpeed) + (addVector * runManouverability * playerControlFactor)).normalized;
            }
            else
            {
                moveSpeed = walkSpeed;
                moveDir = new Vector2(dir_x, dir_y);
            }
        }
        else if (!isRunning())
        {
            moveSpeed = 0;
        }



        if (Mathf.Sqrt(h * h + v * v) >= minInput)
        {
            switch (runState)
            {
                case ERunState.RS_ATTACK:
                    if (moveSpeed < runAttackMaxSpeed)
                    {
                        moveSpeed += runAttackAcceleration * Time.deltaTime;
                        if (moveSpeed >= runAttackMaxSpeed) { moveSpeed = runAttackMaxSpeed; }
                    }
                    else
                    {
                        runState = ERunState.RS_DECAY;
                    }
                    break;

                case ERunState.RS_DECAY:
                    if (moveSpeed > runDecaySpeed)
                    {
                        moveSpeed -= runDecayDeceleration * Time.deltaTime * accelerationModifier;
                        if (moveSpeed <= runDecaySpeed) { moveSpeed = runDecaySpeed; }
                    }
                    else
                    {
                        runState = ERunState.RS_SUSTAIN;
                    }
                    break;

                case ERunState.RS_SUSTAIN:
                    if (moveSpeed < runDecaySpeed)
                    {
                        moveSpeed += runAttackAcceleration * Time.deltaTime * accelerationModifier;
                    }
                    else if (moveSpeed < runSustainMaxSpeed)
                    {
                        moveSpeed += runSustainAcceleration * Time.deltaTime * accelerationModifier;
                        if (moveSpeed >= runSustainMaxSpeed) { moveSpeed = runSustainMaxSpeed; }
                    }
                    break;

                default:
                    break;

            }
        }
        else
        {
            if (moveSpeed > walkSpeed)
            {
                moveSpeed -= runReleaseDeceleration * Time.deltaTime * accelerationModifier;
                if (moveSpeed <= walkSpeed)
                {
                    //Reset to the initial run state for future running
                    runState = ERunState.RS_ATTACK;
                    moveSpeed = walkSpeed;
                }
            }
        }

        if (moveSpeed > 0)
        {
            float x_f = moveDir.x * moveSpeed * Time.deltaTime;
            float y_f = moveDir.y * moveSpeed * Time.deltaTime;
            transform.position = new Vector2(body.position.x + x_f, body.position.y + y_f);
        }

        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.LerpAngle(transform.rotation.eulerAngles.z,
                                                                             GetTargetRotation(),
                                                                             0.2f)));

        accelerationModifier = Mathf.Lerp(accelerationModifier, 1, accelerationModifierLerp);
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
            actionState = EActionState.AS_POSE;
            actionTimer = 0.8f;
            anim.Play("Smashing");
            bodyshaker.shake += 0.1f;

            bool inZone= false;
            Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 0.2f);
            foreach(Collider2D col in colls)
            {
                if(col.tag == "PoseZone")
                {
                    inZone = true;
                    break;
                }
            }

            if(inZone)
            {

                ScoreManager.Instance.AddScore(50, "Pose Zone", transform.position);
            }
            else
            {
                ScoreManager.Instance.AddScore(10, "Pose", transform.position);
            }
        }
    }

    public void StartSpin()
    {
        if (CanDoAction())
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (Mathf.Sqrt(h * h + v * v) >= minInput)
            {
                float yaw = Mathf.Atan2(v, h);
                float dir_x = Mathf.Cos(yaw);
                float dir_y = Mathf.Sin(yaw);
                moveDir = new Vector2(dir_x,dir_y);
            }

                actionState = EActionState.AS_SPIN;
            actionTimer = 1.0f;
            anim.Play("Smashing");
            bodyshaker.shake += 0.2f;
            ScoreManager.Instance.AddScore(5, "Spin", transform.position);
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
                                                actionTimer * 360 * 2f,
                                                0.2f)));

                moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.1f);
            }
            else if (actionState == EActionState.AS_SPIN)
            {
                float x_f = moveDir.x * moveSpeed * Time.deltaTime;
                float y_f = moveDir.y * moveSpeed * Time.deltaTime;
                transform.position = new Vector2(body.position.x + x_f, body.position.y + y_f);

                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.LerpAngle(transform.rotation.eulerAngles.z,
                                                actionTimer * 360 * 4,
                                                0.2f)));

                moveSpeed = Mathf.Lerp(moveSpeed, 5, 0.1f);
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
