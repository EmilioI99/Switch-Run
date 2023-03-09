using UnityEngine;
using UnityEngine.UI;
using DentedPixel;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    private Vector3 colliderOffset;
    float baseSpeed;
    float activationTime;
    private bool doubleJump;
    float horizontalInput;
    private Health health;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip pickupsound;
    [SerializeField] private AudioClip jumpsound;
    [SerializeField] public AudioClip deathsound;
    [SerializeField] public AudioClip hitsound;
    [SerializeField] public AudioClip hurtsound;


    [Header("Physics")]
    [SerializeField] private float speed = 4;
    [SerializeField] private float jumpPower = 9;


    [Header("Collision")]
    public LayerMask groundLayer;
    public float groundLength = 0.8f;
    public bool grounded;


    [Header("PowerUps")]
    public bool yellow;
    public bool yellowActive;
    public bool orange;
    public bool orangeActive;
    public bool red;
    public bool redActive;
    public bool green;
    public bool greenActive;
    public bool blue;
    public bool blueActive;


    [Header("Multipliers")]
    public float buffedSpeed = 8;
    public float powerUpTime = 5;
    public float healthValue = 3;


    [Header("Animation Override Controls")]
    public AnimatorOverrideController normalanim;
    public AnimatorOverrideController yellowanim;
    public AnimatorOverrideController orangeanim;
    public AnimatorOverrideController redanim;
    public AnimatorOverrideController greenanim;
    public AnimatorOverrideController blueanim;
    

    [Header("PowerUp Bar")]
    public GameObject bg;
    public GameObject bar;


    private void Awake()
    {
        //Offset used in raytracing for jump
        colliderOffset = new Vector3(0.25f, 0.0f, 0.0f);

        //Base speed of the player
        baseSpeed = speed;

        //Grab references for the Animator and Rigidbody2D
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        health = GetComponent<Health>();
    }

    private void Update()
    {
        //Player movement with input keys
        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        //Raycast for Jumping 
        
        grounded = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) ||
                    Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
        
       

        //Flip player when moving
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(3, 3, 3);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-3, 3, 3);
        
        if (grounded && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }
        
        //Jump activation with key input
        if (orangeActive)
        {
            //Player can double jump when orange power is active
            if (Input.GetButtonDown("Jump") && (grounded || doubleJump))
            {
                SoundManager.instance.PlaySound(jumpsound);
                Jump();
                doubleJump = !doubleJump;
            }
        }
        else if (grounded && Input.GetButtonDown("Jump")) 
        {
            SoundManager.instance.PlaySound(jumpsound);
            Jump();
        }

        //Makes player fall faster when letting go of the jump button
        if (Input.GetButtonUp("Jump") && body.velocity.y > 0f)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.7f);
        }

        //Activate  powers on left click
        if (Input.GetMouseButtonDown(0) && (yellow || orange || red || green || blue))
        {
            Debug.Log("Activated Powerup");
            activationTime = Time.time;

            if (yellow)
            {
                yellow = false;
                yellowActive = true;
                YellowSkin();
                speed = buffedSpeed;
                AnimateBar();
            }
            else if (orange) 
            {
                orange = false;
                orangeActive = true;
                doubleJump = true;
                OrangeSkin();
                AnimateBar();
            }
            else if (red)
            {
                red = false;
                redActive = true;
                RedSkin();
                AnimateBar();
            }
            else if (green)
            {
                green = false;
                greenActive = true;
                health.AddHealth(healthValue);
                GreenSkin();
                AnimateBar();
            }
            else if (blue)
            {
                blue = false;
                blueActive = true;
                BlueSkin();
                AnimateBar();
            }
        }

        
        //Cancel powerups when timer is done
        if ((yellowActive && Time.time > activationTime +  powerUpTime) || Input.GetMouseButtonDown(1))
        {
            yellow = false;
            yellowActive = false;
            speed = baseSpeed;
            NormalSkin();
            StopBarAnimation();
            bg.SetActive(false);
        }
        if ((orangeActive && Time.time > activationTime + powerUpTime) || Input.GetMouseButtonDown(1))
        {
            orange = false;
            orangeActive = false;
            NormalSkin();
            StopBarAnimation();
            bg.SetActive(false);
        }
        if ((redActive && Time.time > activationTime + powerUpTime) || Input.GetMouseButtonDown(1))
        {
            red = false;
            redActive = false;
            NormalSkin();
            StopBarAnimation();
            bg.SetActive(false);
        }
        if ((greenActive && Time.time > activationTime + powerUpTime) || Input.GetMouseButtonDown(1))
        {
            green = false;
            greenActive = false;
            health.AddHealth(healthValue);
            NormalSkin();
            StopBarAnimation();
            bg.SetActive(false);
        }
        if ((blueActive && Time.time > activationTime + powerUpTime) || Input.GetMouseButtonDown(1))
        {
            blue = false;
            blueActive = false;
            NormalSkin();
            StopBarAnimation();
            bg.SetActive(false);
        }
        

        //Set Animator Parameters
        anim.SetBool("walk", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Set Powerups when grabbing items
        if(collision.gameObject.layer == 7 && !(yellowActive || orangeActive || redActive || greenActive || blueActive))
        {
            SoundManager.instance.PlaySound(pickupsound);
            Destroy(collision.gameObject);
            Debug.Log("Picked up: " + collision.tag);
            bg.SetActive(true);
            bar.transform.localScale = new Vector3(1, 1, 1);

            switch (collision.tag)
            {
                case "Yellow":
                    yellow = true;
                    orange = red = green = blue = false;
                    Color yellowColor = new Color(1.0f, 1.0f, 0.0f, 1.0f); // Yellow
                    bar.GetComponent<Image>().color = yellowColor;
                    break;
                case "Orange":
                    orange = true;
                    yellow = red = green = blue = false;
                    Color orangeColor = new Color(1.0f, 0.5f, 0.0f, 1.0f); // Orange
                    bar.GetComponent<Image>().color = orangeColor;
                    break;
                case "Red":
                    red = true;
                    yellow = orange = green = blue = false;
                    Color redColor = new Color(1.0f, 0.0f, 0.0f, 1.0f); // Red
                    bar.GetComponent<Image>().color = redColor;
                    break;
                case "Green":
                    green = true;
                    yellow = orange = red = blue = false;
                    Color greenColor = new Color(0.0f, 1.0f, 0.0f, 1.0f); // Green
                    bar.GetComponent<Image>().color = greenColor;
                    break;
                case "Blue":
                    blue = true;
                    yellow = orange = red = green = false;
                    Color cyanColor = new Color(0.0f, 1.0f, 1.0f, 1.0f); // Cyan
                    bar.GetComponent<Image>().color = cyanColor;
                    break;
            } 
        }
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpPower);
    }
    public bool canAttack() 
    {
        return grounded && redActive;
    }

    //Functions to switch animations when player uses powerups
    public void NormalSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = normalanim as RuntimeAnimatorController;
    }

    public void YellowSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = yellowanim as RuntimeAnimatorController;
    }
    public void OrangeSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = orangeanim as RuntimeAnimatorController;
    }
    public void RedSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = redanim as RuntimeAnimatorController;
    }
    public void GreenSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = greenanim as RuntimeAnimatorController;
    }
    public void BlueSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = blueanim as RuntimeAnimatorController;
    }


    public void AnimateBar()
    {
        //Animates the cooldown bar for powerups
        //It animates the bar from where it is to the value of 0 in powerUpTime seconds
        LeanTween.scaleX(bar, 0, powerUpTime);
    }
    public void StopBarAnimation()
    {
        LeanTween.cancel(bar);
    }



    public void OnDrawGizmos()
    {
        //Draws red lines from player visualizing raycasting used in jumps
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

}
