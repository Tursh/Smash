using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingBehaviour : MonoBehaviour
{
    public Material NormalColor;
    public Material BlinkColor;
    public int Delay;

    private int timer;
    public bool isBlinking;
    private bool isBlinked;
    private Renderer rendererToBlink;

    private void Start()
    {
        rendererToBlink = GetComponent<Renderer>();
        rendererToBlink.material = NormalColor;
        timer = 0;
    }

    private void FixedUpdate()
    {
        if (isBlinking)
        {
            timer++;
            if (timer > Delay)
            {
                timer = 0;
                isBlinked = !isBlinked;
            }

            if (isBlinked)
                rendererToBlink.material = BlinkColor;
            else
                rendererToBlink.material = NormalColor;
        }
        else
            rendererToBlink.material = NormalColor;
    }
}
