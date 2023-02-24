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

    [Header("Physics")]
    [SerializeField] private float speed = 4;
    [SerializeField] private float jumpPower = 8;


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

    }

    private void Update()
    {
        //Player movement with input keys
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        //Raycast for Jumping 
        grounded = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) ||
                   Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer); 

        //Flip player when moving
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(3, 3, 3);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-3, 3, 3);

        //Jump activation with key input
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            Jump();
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
        if (yellowActive && Time.time > activationTime +  powerUpTime)
        {
            yellowActive = false;
            speed = baseSpeed;
            NormalSkin();
            bg.SetActive(false);
        }
        if (orangeActive && Time.time > activationTime + powerUpTime)
        {
            orangeActive = false;
            NormalSkin();
            bg.SetActive(false);
        }
        if (redActive && Time.time > activationTime + powerUpTime)
        {
            redActive = false;
            NormalSkin();
            bg.SetActive(false);
        }
        if (greenActive && Time.time > activationTime + powerUpTime)
        {
            greenActive = false;
            NormalSkin();
            bg.SetActive(false);
        }
        if (blueActive && Time.time > activationTime + powerUpTime)
        {
            blueActive = false;
            NormalSkin();
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
        grounded = false;
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


    public void OnDrawGizmos()
    {
        //Draws red lines from player visualizing raycasting used in jumps
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

}
