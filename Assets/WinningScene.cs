using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningScene : MonoBehaviour
{
    public int timerInitialValue = 60 * 10;
    private int timer;
    
    private void Start()
    {
        timer = timerInitialValue;
    }

    private void FixedUpdate()
    {
        if (timer-- < 0)
            SceneManager.LoadScene(0);
    }
}
