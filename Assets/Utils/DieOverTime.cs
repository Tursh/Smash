using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOverTime : MonoBehaviour
{
    public float TimeLeft;

    private void Start()
    {
        Destroy(gameObject, TimeLeft);
    }
}
