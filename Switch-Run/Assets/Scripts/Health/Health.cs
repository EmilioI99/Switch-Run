using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;

    [Header("Iframes")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOffFlashes;
    private SpriteRenderer spriteRend;

    [Header("Death")]
    public GameObject ghost;
    public BoxCollider2D playerCollider;
    public AnimatorOverrideController deathanim;


    public float currentHealth { get; private set;}
    private Animator anim;
    public bool dead;
    private PlayerMovement playerMovement;

    


    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage) 
    {
        if (playerMovement.blueActive)
            return;

        //Clamps the health value so it can't go outside the range (0, startingHealth)
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            //player hurt
            anim.SetTrigger("hurt");
            //iframes
            StartCoroutine(Invulneravility());
            SoundManager.instance.PlaySound(playerMovement.hurtsound);
        }
        else
        {
            if (!dead)
            {
                //player dead, plays sound and displays on console 
                dead = true;
                SoundManager.instance.PlaySound(playerMovement.deathsound);
                Debug.Log("Dead");

                //death animation and ghost active
                GetComponent<Animator>().runtimeAnimatorController = deathanim as RuntimeAnimatorController;
                anim.SetTrigger("die");
                ghost.SetActive(true);

                //Stop player movement
                GetComponent<PlayerMovement>().enabled = false;

                // Enable second collider component
                playerCollider.size = new Vector2(0.45f, 0.25f);
            }
        }
    }

    public void AddHealth(float _value) 
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invulneravility()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);
        for (int i = 0; i < numberOffFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration/(numberOffFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration/(numberOffFlashes * 2));

        }
        Physics2D.IgnoreLayerCollision(8, 9, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TakeDamage(1);
    }
}
