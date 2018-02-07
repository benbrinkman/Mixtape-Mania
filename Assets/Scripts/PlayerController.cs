using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public AudioSource sound1;
	public AudioSource sound2;
	public AudioSource sound3;
	public AudioSource sound4;

	public GameObject teamOneJam;
	public GameObject teamTwoJam;
	private bool jamStarted;

    bool[] channels;
    private const int DASHING = 0;
    private const int CHARGING = 1;
    private const int JAMMING = 2;

    public PhysicsMaterial2D bouncy;
    public PhysicsMaterial2D slippery;

    public Animator anim;
    private SpriteRenderer rend;
    public float moveForce;
    private float maxSpeedX;
    private float maxSpeedY;
    public float jumpForce;

    private bool jump;
    private float dashPauseTime;
    private float dashLength;
    private float dashCoolDownLength;
    private float dashTimer;
    private float dashVelocity;
    private bool dashStart;
    private bool dashing;

    private bool chargeAttack;
    public bool charging;
    public float chargePowerMax;
    private float chargeTimer;
    private float chargeTimerMax;
    private chargeAttack chargeAttackClass;
    private BoxCollider2D chargeCollider;
    private float atkTimer;
    private float atkCooldown;

    public bool blastAttack;
    public bool blastForce;
    public float blastCoolDown;
    private float blastCoolDownLength;
    public float blastLength;

    public float stunLength;
    public float stunTime;

	public bool jamming;
	public float jamStart;
	private float jamLength;

    private Transform knockback;
    public bool facingLeft;
    private float attack1ResetRate;
    private float dropThroughThreshold;

    public bool hasSong;
    public bool dropSong;
    public bool grounded;

    private Collider2D boxCol;
    public Rigidbody2D rb2d;
    private groundChecker groundCheck;

    // Player Teams
    public int team;

    // Player IDs
    public int playerNum;
    private static int playerCount = 1;

    // Use this for initialization
    void Awake()
    {
		jamLength = 2;
		jamStarted = false;
		groundCheck = GetComponentInChildren<groundChecker>();
        chargeAttackClass = GetComponentInChildren<chargeAttack>();
        chargeCollider = chargeAttackClass.GetComponent<BoxCollider2D>();

        channels = new bool[3];

        dashVelocity = 75;
        dashLength = 0.1f;
        dashCoolDownLength = 1f;
        dashPauseTime = 0.2f;
        dashTimer = Time.time - dashCoolDownLength;
        dashStart = false;
        dashing = false;

        blastLength = 0.5f;
        blastCoolDownLength = 0.5f;
        blastCoolDown = Time.time - blastCoolDownLength;
        blastAttack = false;

        chargeTimer = 0f;
        chargeTimerMax = 1.0f;
        chargePowerMax = 25;
        attack1ResetRate = 0.01f;
        chargeAttack = false;

        atkTimer = Time.time;
        atkCooldown = 1.2f;

        stunTime = Time.time;
        stunLength = 0;

        hasSong = false;
        dropSong = false;

        jump = false;
        facingLeft = true;
        knockback = this.gameObject.transform.GetChild(0);
        maxSpeedX = 10f;
        maxSpeedY = 10f;
        dropThroughThreshold = -0.7f;

        boxCol = GetComponent<BoxCollider2D>();
        anim = GetComponentInChildren<Animator>();
        rend = GetComponentInChildren<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();

        //playerNum = playerCount;
        //playerCount++;
        //team = playerNum < 3 ? 1 : 2;

        print("Player: " + playerNum + "  Team: " + team);


        // Disable collisions between players on the same team
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in players)
        {
            PlayerController pc = go.GetComponent<PlayerController>();
            Collider2D col = go.GetComponent<Collider2D>();

            if (pc.team == this.team)
            {
                Physics2D.IgnoreCollision(boxCol, col);
            }
        }
    }
    void Update()
    {
        Vector2 move = Vector2.zero;


        if(isStunned())
        {
            return;
        }

        // Basic Jumping
        if (Input.GetButtonDown(getPlayerID() + "Jump") && grounded)
        {
			sound1.Play ();
            jump = true;
            anim.SetBool("isJumping", true);
        }
        // Aerial Dash
        else if (Input.GetButtonDown(getPlayerID() + "Jump") && Time.time - dashTimer > dashCoolDownLength)
        {
			sound3.Play ();
            boxCol.sharedMaterial = bouncy;
            rb2d.velocity = Vector3.zero;
            dashTimer = Time.time;
            dashStart = true;
            anim.SetBool("isJumping", true);
            anim.SetTrigger("Dash");
        }

        // Jamming
        if (Input.GetButtonDown(getPlayerID() + "Fire3") && grounded)
        {
			rb2d.velocity = Vector3.zero;
			stun (jamLength);
			jamStarted = true;
            jamming = true;
            jamStart = Time.time;
        }

        // Charge attack stuff
        if (Input.GetButtonDown(getPlayerID() + "Fire1") && !hasSong && Time.time - atkTimer > atkCooldown)
        {
            chargeTimer = Time.time;
            chargeAttackClass.SetHitForce(0);
            charging = true;
        }

        // TODO: Fix charge attack, use the channels[] thing for proper stunning
        if (Input.GetButtonUp(getPlayerID() + "Fire1") && !hasSong)
        {
            chargeAttack = true;

			sound2.Play ();
            anim.SetTrigger("Attack");
        }
        if (Input.GetButtonDown(getPlayerID() + "Fire1") && hasSong)
        {
            dropSong = true;
        }

        // Blast (Shield)
        if (Input.GetButtonDown(getPlayerID() + "Fire2") && Time.time - blastCoolDown > blastCoolDownLength)
        {
			sound4.Play ();
            blastAttack = true;
            blastCoolDown = Time.time;
            anim.SetTrigger("Defend");
        }

    }

    void FixedUpdate()
    {
        float v = Input.GetAxis(getPlayerID() + "Vertical");
        float h = Input.GetAxis(getPlayerID() + "Horizontal");
        Vector2 aim = new Vector2(h, v);
        aim.Normalize();

        if (v < dropThroughThreshold && groundCheck.platformCollider != null) // && grounded
        {
            Physics2D.IgnoreCollision(boxCol, groundCheck.platformCollider);
            grounded = false;
        }

        // Turning
        if (h < 0)
        {
            facingLeft = true;
            rend.flipX = true;
        }
        else if (h > 0)
        {
            facingLeft = false;
            rend.flipX = false;
        }

        // Jumping
        if (jump)
        {
            rb2d.velocity += new Vector2(0f, jumpForce);
            jump = false;
        }

        // Walking
        if (!dashing && !isStunned())
        {
            float multiplier = grounded ? 1.0f : 0.5f;
            rb2d.velocity += Vector2.right * h * moveForce * multiplier;

            if (Mathf.Abs(h) > 0)
                anim.SetBool("isRunning", true);
        }

        // Horizontal speed cap
        if (Mathf.Abs(rb2d.velocity.x) > maxSpeedX && !dashing && !isStunned())
        {
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeedX, rb2d.velocity.y);
        }

        // Decay horizontal speed while not walking
        if (h == 0 && !isStunned())
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x * 0.01f, rb2d.velocity.y);
            anim.SetBool("isRunning", false);
        }


        // Dash Pause
        if (dashStart)
        {
            rb2d.velocity = Vector2.zero;

            float Zang = Vector2.SignedAngle(Vector2.right, aim);
            if (Mathf.Abs(Zang) >= 90)
                Zang += 180f;
            anim.transform.rotation = Quaternion.identity;
            anim.transform.Rotate(Vector3.forward, Zang);
        }

        // Dashing Pause End
        if (dashStart && Time.time - dashTimer > dashPauseTime)
        {
            dashTimer = Time.time;
            dashStart = false;
            dashing = true;
            
            rb2d.velocity = new Vector2(aim.x * dashVelocity, aim.y * dashVelocity);
        }

        // Ending Dash
        if (dashing && Time.time - dashTimer > dashLength)
        {
            dashing = false;
            boxCol.sharedMaterial = slippery;
            dashTimer = Time.time;
            anim.transform.rotation = Quaternion.identity;

            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -maxSpeedY, maxSpeedY));
        }


		if (jamming) {
			if (jamStarted) {
				jamStarted = false;
				if (team == 1) {
					GameObject part = Instantiate (teamOneJam, transform.position, Quaternion.identity); 
					part.transform.parent = gameObject.transform;
				} else {
					GameObject part = Instantiate (teamTwoJam, transform.position, Quaternion.identity);
					part.transform.parent = gameObject.transform;
				}
			}
			if (Time.time - jamStart > jamLength) {
				jamming = false;
			}


		}
        

        // Blocking
        if (blastAttack && Time.time - blastCoolDown > blastLength)
        {
            blastAttack = false;

        }


        // Basic attack reset
        chargeAttackClass.attacking = false;

        // Move to appropriate side
        Vector3 pos = chargeAttackClass.transform.localPosition;
        chargeAttackClass.transform.localPosition = new Vector3(0.8f * (facingLeft ? -1 : 1), pos.y, pos.z);

        // Basic attack
        if (chargeAttack)
        {
            float chargePower = CalcChargePowerMult() * chargePowerMax;
            chargeAttackClass.SetHitForce(chargePower);

            int direction = 1;
            if (!facingLeft)
                direction *= -1;

            // Re-enable component
            chargeAttackClass.attacking = true;

            chargeAttack = false;
            atkTimer = Time.time;
        }

       

    }

    void OnTriggerEnter2D(Collider2D col)
    {
    }
    void OnTriggerExit2D(Collider2D col)
    {
    }


    public string getPlayerID()
    {
        return "p" + playerNum + "_";
    }

    public void stun(float stun_length)
    {
        stunTime = Time.time;
        stunLength = stun_length;

        for (int i = 0; i < channels.Length; i++)
        {
            channels[i] = false;
        }
    }


	public bool isJamming(){
		return jamming;
	}

    public bool isStunned()
    {
        return Time.time - stunTime < stunLength;
    }

    public bool isBlocking()
    {
        return blastAttack;
    }

    float CalcChargePowerMult()
    {
        float chargeMultMin = 0.2f;
        float timeDif = Mathf.Clamp(Time.time - chargeTimer, chargeMultMin, chargeTimerMax);
        float chargeMult = 1.0f - (chargeTimerMax - timeDif);
        
        return chargeMult;
    }

}