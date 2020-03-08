using System;
using System.Collections;
using System.Collections.Generic;
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

    public FrameDataPhysical()
    {
        Damage = 0;
        Multiplier = 0;
        Radius = 1;
        Direction = Vector2.zero;
        Offset = Vector2.zero;
    }

    public FrameDataPhysical(float Damage, float Multiplier, float Radius, Vector2 Direction, Vector2 Offset)
    {
        this.Damage = Damage;
        this.Multiplier = Multiplier;
        this.Radius = Radius;
        this.Direction = Direction;
        this.Offset = Offset;
    }

    public FrameDataPhysical reversed()
    {
        return new FrameDataPhysical(Damage,
            Multiplier, Radius, new Vector2(-Direction.x, Direction.y), new Vector2(-Offset.x, Offset.y));
    } 
}

public class FrameDataProjectile
{
    public readonly float Damage;
    public readonly float Multiplier;
    public readonly float Radius;
    public Vector2 Direction;
    public Vector2 Velocity;
    public float TimeToLive;
    
    public FrameDataProjectile()
    {
        Damage = 1;
        Multiplier = 1;
        Radius = 1;
        Direction = Vector2.right;
        Velocity = Vector2.right;
        TimeToLive = 1;
    }
    public FrameDataProjectile(float damage, float multiplier, float radius, Vector2 direction, Vector2 velocity, float timeToLive)
    {
        Damage = damage;
        Multiplier = multiplier;
        Radius = radius;
        Direction = direction;
        Velocity = velocity;
        TimeToLive = timeToLive;
    }
    

    public FrameDataProjectile reversed()
    {
        return new FrameDataProjectile(
            Damage, 
            Multiplier,
            Radius,
            new Vector2(-Direction.x, Direction.y), 
            new Vector2(-Velocity.x, Velocity.y), 
            TimeToLive);
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