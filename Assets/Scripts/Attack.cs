using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Playables;

public class AttackSet
{
    public AttackGroup LightGrounded { get; set; }
    public AttackGroup HeavyGrounded { get; set; }
    public AttackGroup LightAerial { get; set; }
    public AttackGroup HeavyAerial { get; set; }

    public AttackSet(AttackGroup lightGrounded, AttackGroup heavyGrounded, AttackGroup lightAerial,
        AttackGroup heavyAerial)
    {
        LightGrounded = lightGrounded;
        HeavyGrounded = heavyGrounded;
        LightAerial = lightAerial;
        HeavyAerial = heavyAerial;
    }
}


public class Attack
{
    private Func<GameObject, bool> Function;
    
    public Attack()
    {
        Function = o => false;
    }

    public Attack(Func<GameObject, bool> function)
    {
        Function = function;
    }
    
    public void Act(GameObject player)
    {
        Function(player);
    }
}

public class AttackGroup
{
    public Attack[] Up { get; private set; }
    public Attack[] Down { get; private set; }
    public Attack[] Forward { get; private set; }
    public Attack[] Back { get; private set; }

    public AttackGroup()
    {
    }

    public AttackGroup(Attack[] up, Attack[] down, Attack[] forward, Attack[] back)
    {
        Up = up;
        Down = down;
        Forward = forward;
        Back = back;
    }
}