using System;
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
    private Queue<FrameOfAttack> framesOfAttack;

    public void Wait(int numberOfFrames)
    {
        FrameOfAttack[] frameOfAttacks = new FrameOfAttack[numberOfFrames];
        for (int i = 0; i < numberOfFrames; i++)
            frameOfAttacks[i] = new FrameOfAttack();
        Enqueue(frameOfAttacks);
    }
    
    public Attack(FrameOfAttack[] framesOfAttack)
    {
        this.framesOfAttack = new Queue<FrameOfAttack>();
        Enqueue(framesOfAttack);
    }

    public Attack()
    {
        framesOfAttack = new Queue<FrameOfAttack>();
    }

    public FrameOfAttack Dequeue()
    {
        if (framesOfAttack.Any())
            return framesOfAttack.Dequeue();
        return new FrameOfAttack();
    }

    public void Enqueue(FrameOfAttack[] frames)
    {
        for (int i = 0; i < frames.Length; ++i)
            framesOfAttack.Enqueue(frames[i]);
    }
    
    public void Enqueue(TimerFramesOfAttack frames)
    {
        frames.timer = 0;
        for (int i = 0; i < frames.FrameOfAttacks.Length; ++i)
            framesOfAttack.Enqueue(frames.FrameOfAttacks[i]);
    }
    

    public FrameOfAttack[] GetFrames()
    {
        return framesOfAttack.ToArray();
    }
    
    public void Enqueue(FrameOfAttack frame)
    {
        Enqueue(new []{frame});
    }

    public void Clear() => framesOfAttack.Clear();
    public bool IsEmpty() => !framesOfAttack.Any();
}