using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    private Vector3 colliderOffset;
    float pickupTime; 

    [Header("Physics")]
    [SerializeField] private float speed = 4;
    [SerializeField] private float jumpPower = 8;


    [Header("Collision")]
    public LayerMask groundLayer;
    public float groundLength = 0.8f;
    public bool grounded;

    [Header("PowerUps")]
    public bool yellow;
    public bool green;
    public bool red;
    public bool blue;
    public bool orange;

    [Header("Multipliers")]
    public float speedMultiplier = 2;
    public float speedTime = 5; 


    private void Awake()
    {
        colliderOffset = new Vector3(0.25f, 0.0f, 0.0f);

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

        if (Input.GetKey(KeyCode.Space) && grounded)
            Jump();

        //Update Powerups when grabbing items
        if(yellow && Time.time > pickupTime +  speedTime)
        {
            yellow = false;
            speed /= speedMultiplier;
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


            if(collision.tag == "Yellow")
            {
                pickupTime = Time.time;
                yellow = true;
                speed *= speedMultiplier;
                Debug.Log(collision.tag);
            }
        }
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpPower);
        grounded = false;
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

}
