using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Playables;

public enum AttackType { PHYSICAL, PROJECTILE }

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

public class FrameDataPhysical
{
    public readonly float Damage;
    public readonly float Multiplier;
    public readonly float Radius;
    public Vector2 Direction;
    public Vector2 Offset;
    public GameObject HurtboxPrefab;
    public int FramesOfLife;

    public FrameDataPhysical(GameObject hurtboxPrefab)
    {
        Damage = 0.1f;
        Multiplier = 1;
        Radius = 1;
        Direction = Vector2.zero;
        Offset = Vector2.zero;
        HurtboxPrefab = hurtboxPrefab;
        FramesOfLife = 1;
    }

    public FrameDataPhysical(
        GameObject hurtboxPrefab,
        Vector2 direction,
        Vector2 offset,
        float damage = 0.1f, 
        float multiplier = 1, 
        float radius = 1,
        int framesOfLife = 1)
    {
        Damage = damage;
        Multiplier = multiplier;
        Radius = radius;
        Direction = direction;
        Offset = offset;
        HurtboxPrefab = hurtboxPrefab;
        FramesOfLife = framesOfLife;
    }


    public FrameDataPhysical Reversed()
    {
        return new FrameDataPhysical(
            HurtboxPrefab,
            new Vector2(-Direction.x, Direction.y),
            new Vector2(-Offset.x, Offset.y),
            Damage,
            Multiplier,
            Radius );
    }

    public void Reverse()
    {
        Direction = new Vector2(-Direction.x,Direction.y);
        Offset = new Vector2(-Offset.x, Offset.y);
    }
}

public class FrameDataProjectile
{
    public readonly float Damage;
    public readonly float Multiplier;
    public readonly float Radius;
    public Vector2 Direction;
    public Vector2 Velocity;
    public GameObject ProjectilePrefab;
    public readonly int FramesOfLife;
    
    public FrameDataProjectile(GameObject projectilePrefab)
    {
        ProjectilePrefab = projectilePrefab;
        Damage = 1;
        Multiplier = 1;
        Radius = 1;
        Direction = Vector2.right;
        Velocity = Vector2.right;
        FramesOfLife = 60;
    }
    public FrameDataProjectile(
        GameObject projectilePrefab,
        Vector2 direction, 
        Vector2 velocity,
        float damage = 1, 
        float multiplier = 1, 
        float radius = 1,
        int framesOfLife = 60)
    {
        Damage = damage;
        Multiplier = multiplier;
        Radius = radius;
        Direction = direction;
        Velocity = velocity;
        FramesOfLife = framesOfLife;
        ProjectilePrefab = projectilePrefab;
    }
    

    public FrameDataProjectile Reversed()
    {
        return new FrameDataProjectile(
            ProjectilePrefab,
            new Vector2(-Direction.x, Direction.y), 
            new Vector2(-Velocity.x, Velocity.y),
            Damage,
            Multiplier,
            Radius,
            FramesOfLife);
    }

    public void Reverse()
    {
        Direction = new Vector2(-Direction.x, Direction.y);
        Velocity = new Vector2(-Velocity.x, Velocity.y);
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