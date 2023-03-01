using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timer = 0;
    public TextMeshProUGUI timerText;

    void Update()
    {
        timer += Time.deltaTime;
        timerText.text = "" + timer.ToString("f1");
    }
}
