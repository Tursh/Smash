using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSet
{
    public AttackGroup LightGrounded { get; set; }
    public AttackGroup HeavyGrounded { get; set; }
    public AttackGroup LightAerial { get; set; }
    public AttackGroup HeavyAerial { get; set; }
}

public class Attack
{
    public Vector2 Direction { get; private set; }
    public float Damage { get; private set; }
    public float Multiplier { get; private set; }
    public Vector2 BoxSize { get; private set; }
    public Vector2 Offset { get; private set; }

    public Attack(Vector2 direction, float damage, float multiplier, Vector2 boxSize, Vector2 offset)
    {
        Direction = direction;
        Damage = damage;
        Multiplier = multiplier;
        BoxSize = boxSize;
        Offset = offset;
    }
}

public class AttackGroup
{
    public Attack[] Up { get; private set; }
    public Attack[] Down { get; private set; }
    public Attack[] Forward { get; private set; }
    public Attack[] Back { get; private set; }
    
    public AttackGroup(Attack[] up, Attack[] down, Attack[] forward, Attack[] back)
    {
        Up = up;
        Down = down;
        Forward = forward;
        Back = back;
    }
}
