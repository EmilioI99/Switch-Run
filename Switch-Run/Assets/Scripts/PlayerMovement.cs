using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;

    [Header("Physics")]
    [SerializeField] private float speed = 4;
    [SerializeField] private float jumpPower = 8;
    
    
    [Header("Collision")]
    public LayerMask groundLayer;
    public float groundLength = 0.8f;
    private bool grounded;


    private void Awake()
    {
        //Grab references for the Animator and Rigidbody2D
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        //Raycast for Jumping 
        grounded = Physics2D.Raycast(transform.position, Vector2.down, groundLength, groundLayer);

        //Flip player when moving
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(3, 3, 3);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-3, 3, 3);

        if (Input.GetKey(KeyCode.Space) && grounded)
            Jump();
            

        //Set Animator Parameters
        anim.SetBool("walk", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpPower);
        grounded = false;
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundLength);
    }

}
