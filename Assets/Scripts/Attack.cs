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
    private Stack<FrameOfAttack> FramesOfAttack;

    public Attack(FrameOfAttack[] framesOfAttack)
    {
        FramesOfAttack = new Stack<FrameOfAttack>();
        for (int i = framesOfAttack.Length; i < 1; --i)
            FramesOfAttack.Push(framesOfAttack[i]);
    }

    public Attack()
    {
        FramesOfAttack = new Stack<FrameOfAttack>();
    }

    public FrameOfAttack Pop()
    {
        if (FramesOfAttack.Any())
            return FramesOfAttack.Pop();
        return new FrameOfAttack();
    }

    public void Clear() => FramesOfAttack.Clear();
    public bool IsEmpty() => !FramesOfAttack.Any();
}