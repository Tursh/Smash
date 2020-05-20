using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CstmAnimation
{
    public bool Done;
    private Func<GameObject, int, bool> step;
    private int steps;
    private int maxSteps;
    public CstmAnimation(Func<GameObject,int, bool> step, int maxSteps)
    {
        Done = false;
        this.maxSteps = maxSteps;
        this.step = step;
    }

    public void Step(GameObject o)
    {
        steps++;
        step(o, steps);
        Done = steps >= maxSteps;
    }

    public void Start()
    {
        steps = 0;
        Done = false;
    }

    public void Stop()
    {
        steps = 0;
        Done = true;
    }

    public void Pause()
    {
        Done = true;
    }

    public void Play()
    {
        Done = false;
    }

    public void Play(int step)
    {
        steps = step;
        Play();
    }
}

