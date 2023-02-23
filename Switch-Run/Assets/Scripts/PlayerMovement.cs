using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    private Vector3 colliderOffset;
    float pickupTime;
    float baseSpeed;

    [Header("Physics")]
    [SerializeField] private float speed = 4;
    [SerializeField] private float jumpPower = 8;


    [Header("Collision")]
    public LayerMask groundLayer;
    public float groundLength = 0.8f;
    public bool grounded;


    [Header("PowerUps")]
    public bool yellow;
    public bool orange;
    public bool red;
    public bool green;
    public bool blue;
    

    [Header("Multipliers")]
    public float buffedSpeed = 8;
    public float speedTime = 5;


    [Header("Animation Override Controls")]
    public AnimatorOverrideController normalanim;
    public AnimatorOverrideController yellowanim;
    public AnimatorOverrideController orangeanim;
    public AnimatorOverrideController redanim;
    public AnimatorOverrideController greenanim;
    public AnimatorOverrideController blueanim;


    private void Awake()
    {
        colliderOffset = new Vector3(0.25f, 0.0f, 0.0f);
        baseSpeed = speed;

        //Grab references for the Animator and Rigidbody2D
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    private void Update()
    {
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

        if (Input.GetKey(KeyCode.Space) && (grounded || orange))
        {
            Jump();
        }
            

        
        //Cancel powerups when timer is done
        if (yellow && Time.time > pickupTime +  speedTime)
        {
            yellow = false;
            speed = baseSpeed;
            NormalSkin();
        }
        if (orange && Time.time > pickupTime + speedTime)
        {
            orange = false;
            NormalSkin();
        }
        if (red && Time.time > pickupTime + speedTime)
        {
            red = false;
            NormalSkin();
        }
        if (green && Time.time > pickupTime + speedTime)
        {
            green = false;
            NormalSkin();
        }
        if (blue && Time.time > pickupTime + speedTime)
        {
            blue = false;
            NormalSkin();
        }
        

        //Set Animator Parameters
        anim.SetBool("walk", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Set Powerups when grabbing items
        if(collision.gameObject.layer == 7)
        {
            Destroy(collision.gameObject);
            pickupTime = Time.time;
            Debug.Log("Picked up: " + collision.tag);

            switch (collision.tag)
            {
                case "Yellow":
                    YellowSkin();  
                    yellow = true;
                    speed = buffedSpeed;                   
                    break;
                case "Orange":
                    OrangeSkin();
                    orange = true;
                    break;
                case "Red":
                    RedSkin();
                    red = true;
                    break;
                case "Green":
                    GreenSkin();
                    green = true;
                    break;
                case "Blue":
                    BlueSkin();
                    blue = true;
                    break;
            }
        }
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpPower);
        grounded = false;
    }

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


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

}
