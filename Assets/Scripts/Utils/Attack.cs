﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public enum AttackState
{
    Attacking,
    Idle
}

public class TimerFramesOfAttack
{
    public FrameOfAttack[] FrameOfAttacks;
    public int timer;
    public int time;

    public TimerFramesOfAttack(int time, FrameOfAttack[] frameOfAttacks, int delay = 0)
    {
        List<FrameOfAttack> FrameOfAttacks;
        this.time = time;
        FrameOfAttacks = frameOfAttacks.ToList();
        for (int i = 0; i < delay; i++)
            FrameOfAttacks = FrameOfAttacks.Prepend(new FrameOfAttack()).ToList();
        this.FrameOfAttacks = FrameOfAttacks.ToArray();
    }

    public bool CanAttack()
    {
        return timer > time;
    }

    public bool CanAttackInc()
    {
        return timer > time++;
    }
}

public class FrameOfAttack
{
    private Func<GameObject,bool> Function;

    public FrameOfAttack()
    {
        Function = o => false;
    }

    public FrameOfAttack(Func<GameObject, bool> function)
    {
        Function = function;
    }
    
    public bool Act(GameObject player)
    {
        return Function(player);
    }
}

public class Attack
{
    private Queue<FrameOfAttack> FramesOfAttack;

    public void Wait(int numberOfFrames)
    {
        FrameOfAttack[] frameOfAttacks = new FrameOfAttack[numberOfFrames];
        for (int i = 0; i < numberOfFrames; i++)
            frameOfAttacks[i] = new FrameOfAttack();
        Push(frameOfAttacks);
    }
    
    public Attack(FrameOfAttack[] framesOfAttack)
    {
        FramesOfAttack = new Queue<FrameOfAttack>();
        Push(framesOfAttack);
    }

    public Attack()
    {
        FramesOfAttack = new Queue<FrameOfAttack>();
    }

    public FrameOfAttack Pop()
    {
        if (FramesOfAttack.Any())
            return FramesOfAttack.Dequeue();
        return new FrameOfAttack();
    }

    public void Push(FrameOfAttack[] frames)
    {
        for (int i = 0; i < frames.Length; ++i)
            FramesOfAttack.Enqueue(frames[i]);
    }
    
    public void Push(TimerFramesOfAttack frames)
    {
        frames.timer = 0;
        for (int i = 0; i < frames.FrameOfAttacks.Length; ++i)
            FramesOfAttack.Enqueue(frames.FrameOfAttacks[i]);
    }
    

    public FrameOfAttack[] GetFrames()
    {
        return FramesOfAttack.ToArray();
    }
    
    public void Push(FrameOfAttack frame)
    {
        Push(new []{frame});
    }

    public void Clear() => FramesOfAttack.Clear();
    public bool IsEmpty() => !FramesOfAttack.Any();
}