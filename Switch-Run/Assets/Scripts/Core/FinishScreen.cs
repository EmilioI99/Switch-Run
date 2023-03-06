using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class FinishScreen : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    public void FinishActive(float _timer)
    {
        Debug.Log("Finish Screen");
        gameObject.SetActive(true);
        timeText.text ="You finished in: " + _timer.ToString("f1");
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
