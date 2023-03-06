using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timer = 0;
    public TextMeshProUGUI timerText;
    public Health health;
    public FinishLine finishLine;
    public float finishTime = 0;

    void Update()
    {
        if (health.dead || finishLine.finished)
            return;
        timer += Time.deltaTime;
        timerText.text = "" + timer.ToString("f1");
    }
}
