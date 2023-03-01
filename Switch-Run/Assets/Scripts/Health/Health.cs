using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set;}
    private Animator anim;
    private bool dead;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void TakeDamage(float _damage) 
    {
        if (playerMovement.blueActive)
            _damage = 0;

        //Clamps the health value so it can't go outside the range (0, startingHealth)
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            //player hurt
            anim.SetTrigger("hurt");
            //iframes
        }
        else
        {
            if (!dead)
            {
                //player dead
                Debug.Log("Dead");
                anim.SetTrigger("die");
                GetComponent<PlayerMovement>().enabled = false;
                gameObject.SetActive(false);
                dead = true;
            }
        }
    }

    public void AddHealth(float _value) 
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TakeDamage(1);
    }
}
