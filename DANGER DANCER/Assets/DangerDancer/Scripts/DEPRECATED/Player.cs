using UnityEngine;
using System.Collections;

enum ERunState
{
    RS_ATTACK,
    RS_DECAY,
    RS_SUSTAIN
}

public class Player : MonoBehaviour
{
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
    [SerializeField] private float runFallDot;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float playerControlRegen;
    [SerializeField] private float accelerationModifierLerp;

    [SerializeField] private float smashRadius;
    [SerializeField] private float smashAngle;

    [SerializeField] private float minInput;
    [SerializeField] private LayerMask wallMask;

    private ERunState runState;
    private Vector2 moveDir;
    private float moveSpeed;
    private int fallTicks;
    private float playerControlFactor;
    private float accelerationModifier;

    private Rigidbody2D body;
    private Animator anim;
    private SpriteRenderer headsprite;
    public Shaker bodyshaker;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bodyshaker = transform.Find("Body").GetComponent<Shaker>();;
        headsprite = transform.Find("Body").transform.Find("Head").GetComponent<SpriteRenderer>();
        moveSpeed = walkSpeed;
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

    private void Attack()
    {
        anim.Play("Smashing");
        bodyshaker.shake += 0.1f;
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

    private void AnimUpdate()
    {
        headsprite.enabled = !isFallen();
        anim.SetBool("Fallen", isFallen());
        anim.SetBool("Running", isRunning());
        anim.SetBool("Moving", isMoving());
    }

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

    private void Update()
    {
        if (!isFallen())
        {
            MoveUpdate();
        }

        if (playerControlFactor < 1.0f)
        {
            playerControlFactor += playerControlRegen * Time.deltaTime;
        }
        playerControlRegen = Mathf.Clamp(playerControlRegen, 0, 1);

        if (Input.GetButtonDown("Fire1"))
        {
            if (isFallen())
            {
                if(fallTicks > 1) bodyshaker.shake += 0.1f;
                fallTicks -= 1;
                if(fallTicks <= 0)
                {
                    body.velocity = new Vector2();
                }
            }
            else
            {
                Attack();
            }
        }

        AnimUpdate();
    }

    public void Fall(Vector2 fallForce)
    {
        moveSpeed = 0;
        fallTicks = 3;
        body.AddForce(fallForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Danger")
        {
            Fall(new Vector2());
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Vector2 hitVector = coll.contacts[0].point - (Vector2)transform.position;

        float dot = Vector2.Dot(hitVector.normalized, transform.right);
        print(dot);
        if( moveSpeed > fallSpeed && dot > runFallDot )
        {
            Fall((moveDir * -1).normalized * 200);
        }
        else
        {
            playerControlFactor = 0;
            moveDir = Vector2.Reflect(moveDir, coll.contacts[0].normal);
        }
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

                moveSpeed -=  (1 - Mathf.Max(0,Vector2.Dot(moveVector, addVector))) * turnDeceleration;
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
            switch(runState)
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
}
