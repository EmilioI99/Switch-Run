using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardDamage : MonoBehaviour
{
    public float damage;
    public Health playerhealth;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerhealth.TakeDamage(damage);
        }
    }

}