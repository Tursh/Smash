using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FrameDataPhysical
{
    public readonly float Damage;
    public readonly float Multiplier;
    public readonly float Radius;
    public Vector2 Direction;
    public Vector2 Offset;
    public int PrefabIndex;
    public int FramesOfLife;

    public FrameDataPhysical()
    {
        Damage = 0.1f;
        Multiplier = 1;
        Radius = 1;
        Direction = Vector2.zero;
        Offset = Vector2.zero;
        PrefabIndex = 0;
        FramesOfLife = 1;
    }

    public FrameDataPhysical(
        Vector2 direction,
        Vector2 offset,
        float damage = 0.1f, 
        float multiplier = 1, 
        float radius = 1,
        int framesOfLife = 1,
        int prefabIndex = 0)
    {
        Damage = damage;
        Multiplier = multiplier;
        Radius = radius;
        Direction = direction;
        Offset = offset;
        PrefabIndex = prefabIndex;
        FramesOfLife = framesOfLife;
    }


    public FrameDataPhysical Reversed()
    {
        return new FrameDataPhysical(
            new Vector2(-Direction.x, Direction.y),
            new Vector2(-Offset.x, Offset.y),
            Damage,
            Multiplier,
            Radius,
            FramesOfLife,
            PrefabIndex);
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
    public int PrefabIndex;
    public readonly int FramesOfLife;
    
    public FrameDataProjectile()
    {
        PrefabIndex = 0;
        Damage = 1;
        Multiplier = 1;
        Radius = 1;
        Direction = Vector2.right;
        Velocity = Vector2.right;
        FramesOfLife = 60;
    }
    public FrameDataProjectile(
        Vector2 direction, 
        Vector2 velocity,
        float damage = 1, 
        float multiplier = 1, 
        float radius = 1,
        int framesOfLife = 60,
        int prefabIndex = 0)
    {
        Damage = damage;
        Multiplier = multiplier;
        Radius = radius;
        Direction = direction;
        Velocity = velocity;
        FramesOfLife = framesOfLife;
        PrefabIndex = prefabIndex;
    }
    

    public FrameDataProjectile Reversed()
    {
        return new FrameDataProjectile(
            new Vector2(-Direction.x, Direction.y), 
            new Vector2(-Velocity.x, Velocity.y),
            Damage,
            Multiplier,
            Radius,
            FramesOfLife,
            PrefabIndex);
    }

    public void Reverse()
    {
        Direction = new Vector2(-Direction.x, Direction.y);
        Velocity = new Vector2(-Velocity.x, Velocity.y);
    }
}
