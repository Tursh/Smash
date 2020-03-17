using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Playables;

public enum AttackState
{
    Attacking,
    Idle
}

public class FrameOfAttack
{
    private Func<GameObject, bool> Function;

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

    public void Clear() => FramesOfAttack.Clear();
    public bool IsEmpty() => !FramesOfAttack.Any();
}