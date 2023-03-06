using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard_Sideways : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;
    private Animator anim;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
        playerMovement = GetComponent<PlayerMovement>();
    }


    private void Update()
    {
        if (movingLeft)
        {
            if (transform.position.x > leftEdge)
            {
                 transform.localScale = new Vector3(-2 ,2 , 2);


                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = false;
        }
        else
        {
            if (transform.position.x < rightEdge)
            {
                 transform.localScale = new Vector3(2 ,2 , 2);

                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = true;
        }
        anim.SetBool("moving", true);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
