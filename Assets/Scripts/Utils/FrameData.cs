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
    public GameObject Prefab;
    public int FramesOfLife;

    public FrameDataPhysical(GameObject prefab)
    {
        Damage = 0.1f;
        Multiplier = 1;
        Radius = 1;
        Direction = Vector2.zero;
        Offset = Vector2.zero;
        Prefab = prefab;
        FramesOfLife = 1;
    }

    public FrameDataPhysical(
        Vector2 direction,
        Vector2 offset,
        GameObject prefab,
        float damage = 0.1f, 
        float multiplier = 1, 
        float radius = 1,
        int framesOfLife = 1
        )
    {
        Damage = damage;
        Multiplier = multiplier;
        Radius = radius;
        Direction = direction;
        Offset = offset;
        Prefab = prefab;
        FramesOfLife = framesOfLife;
    }


    public FrameDataPhysical Reversed()
    {
        return new FrameDataPhysical(
            new Vector2(-Direction.x, Direction.y),
            new Vector2(-Offset.x, Offset.y),
            Prefab,
            Damage,
            Multiplier,
            Radius,
            FramesOfLife
            );
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
    public Vector2 SpawnOffset;
    public Vector2 Direction;
    public Vector2 Velocity;
    public GameObject Prefab;
    public readonly int FramesOfLife;
    
    public FrameDataProjectile(GameObject prefab)
    {
        Prefab = prefab;
        Damage = 1;
        Multiplier = 1;
        Radius = 1;
        SpawnOffset = Vector2.zero;
        Direction = Vector2.right;
        Velocity = Vector2.right;
        FramesOfLife = 60;
    }
    public FrameDataProjectile(
        Vector2 direction, 
        Vector2 velocity,
        Vector2 spawnOffset,
        GameObject prefab,
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
        SpawnOffset = spawnOffset;
        FramesOfLife = framesOfLife;
        Prefab = prefab;
    }
    

    public FrameDataProjectile Reversed()
    {
        return new FrameDataProjectile(
            new Vector2(-Direction.x, Direction.y), 
            new Vector2(-Velocity.x, Velocity.y),
            new Vector2(-SpawnOffset.x, SpawnOffset.y),
            Prefab,
            Damage,
            Multiplier,
            Radius,
            FramesOfLife);
    }

    public void Reverse()
    {
        Direction = new Vector2(-Direction.x, Direction.y);
        Velocity = new Vector2(-Velocity.x, Velocity.y);
        SpawnOffset = new Vector2(-SpawnOffset.x, SpawnOffset.y);
    }
}
