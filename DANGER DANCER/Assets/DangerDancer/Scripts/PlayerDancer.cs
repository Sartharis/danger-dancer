using UnityEngine;
using System.Collections;

public enum EActionState
{
    AS_IDLE,
    AS_SPIN,
    AS_POSE
}

public class PlayerDancer : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float minInput;
    [SerializeField] private float playerControlRegen;
    [SerializeField] private float poseDuration;
    [SerializeField] private float spinDuration;
    [SerializeField] private float spinDistance;
	[SerializeField] private float invulnerableDuration;
	[SerializeField] private float blinkPeriod;
    [SerializeField] private AnimationCurve spinCurve;

    [Header("Collision")]
    [SerializeField] private LayerMask wallMask;

    [Header("Collision")]
    [SerializeField] private float offBeatGracePeriod;

    [Header("Components")]
    public Shaker bodyshaker;

    public EActionState actionState;
    private Vector2 actionStartPoint;
    private Vector2 actionEndPoint;
    private ERunState runState;
    public Vector2 moveDir;
    private float moveSpeed;
    private int fallTicks;
    private Vector2 queuedDir;
    private bool moveAttemptedPress;
    private bool messedUpBeatPress;
    private float offBeatGraceTime = 0;

    private float actionTimer;
	private float invulnerableTimer;

    private Rigidbody2D body;
    private Animator anim;
	private SpriteRenderer bodysprite;
    private SpriteRenderer headsprite;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bodyshaker = transform.Find("Body").GetComponent<Shaker>();
		bodysprite = transform.Find ("Body").GetComponent<SpriteRenderer> ();
        headsprite = transform.Find("Body").transform.Find("Head").GetComponent<SpriteRenderer>();
        BeatManager.Instance.OnBeat += OnBeat;
        moveAttemptedPress = false;
        messedUpBeatPress = false;
    }


    // UPDATE---------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void Update()
    {
        offBeatGraceTime -= Time.deltaTime;

        if (Input.GetButtonDown("Fire1"))
        {
            if (isFallen())
            {
                if(BeatManager.Instance.IsOnBeat())
                {
                    if (fallTicks > 1) bodyshaker.shake += 0.1f;
                    fallTicks -= 1;
					if (!isFallen ())
					{
						invulnerableTimer = invulnerableDuration;
					}
                }
            }
            else
            {
                StartPose();
            }
        }

        ActionsUpdate();
        if (canMove())
        {
            MoveUpdate();
        }
		InvulnerableUpdate();

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
        return actionState == EActionState.AS_SPIN;
    }

    public void Fall(Vector2 fallForce)
    {
		if (!isFallen() && invulnerableTimer <= 0.0f)
        {
            fallTicks = 1;
            ScoreManager.Instance.AddScore(-15, "Oh no", transform.position);
             CameraShake.Instance.ShakeCamera(1f, 0.05f);
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

    private void MessUpMove()
    {

       
        if ( offBeatGraceTime <= 0)
        {
            offBeatGraceTime = offBeatGracePeriod;
            messedUpBeatPress = true;
            ScoreManager.Instance.AddScore(-5, "Off Beat", transform.position);
             CameraShake.Instance.ShakeCamera(0.5f, 0.05f);
        }

        bodyshaker.shake += 0.1f;
    }

    private void OnBeat()
    {
        if( moveAttemptedPress && queuedDir.magnitude >= minInput && actionState == EActionState.AS_IDLE)
        {
            MessUpMove();
        }
        messedUpBeatPress = false;
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
        

        if (queuedDir.magnitude >= minInput)
        {
            if(!moveAttemptedPress && !messedUpBeatPress)
            {
                if (BeatManager.Instance.IsOnBeat() && CanDoAction())
                {
                    StartSpin();
                    queuedDir = new Vector2();
                    offBeatGraceTime = 0;

                    if (isFallen())
                    {
                        if (fallTicks > 1) bodyshaker.shake += 0.1f;
                        fallTicks -= 1;
                    }

                }
                else
                {

                    MessUpMove();
                }
                moveAttemptedPress = true;
            }
        }
        else
        {
            moveAttemptedPress = false;
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
            actionEndPoint = transform.position + (Vector3)moveDir * spinDistance;
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
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.LerpAngle(transform.rotation.eulerAngles.z,
                                                actionTimer * 360 * 6f,
                                                0.2f)));
            }
            else if (actionState == EActionState.AS_SPIN)
            {           
                Vector3 spinPoint = actionStartPoint + (actionEndPoint - actionStartPoint) * spinCurve.Evaluate(1 - (actionTimer / spinDuration));
                body.MovePosition(spinPoint);
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.LerpAngle(transform.rotation.eulerAngles.z,
                                                actionTimer * 360 * 4,
                                                0.2f)));
            }
        }
    }


    // ANIMATIONS--------------------------------------------------------------------------------------------------------------------

    private void AnimUpdate()
    {
		bodysprite.enabled = blinkOn();
		headsprite.enabled = !isFallen() && blinkOn();
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
        if (actionState == EActionState.AS_SPIN)
        {
            actionEndPoint = actionStartPoint;
            actionStartPoint = transform.position;
        }
    }

	private void InvulnerableUpdate()
	{
		if (invulnerableTimer > 0.0f)
		{
			invulnerableTimer -= Time.deltaTime;
		}
	}

	private bool blinkOn()
	{
		if (invulnerableTimer <= 0.0f)
		{
			return true;
		}
		return (Mathf.Floor (invulnerableTimer / blinkPeriod)) % 2 == 0;
	}
}
