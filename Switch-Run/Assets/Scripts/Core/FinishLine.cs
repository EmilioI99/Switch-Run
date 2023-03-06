using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public FinishScreen finishScreen;
    public Timer timer;
    public bool finished = false;
    public PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Player Finished");
            finished = true;
            finishScreen.FinishActive(timer.timer);
            //Stop Player Movement
            playerMovement.enabled = false;
            collision.gameObject.SetActive(false);
        }
    }
}
