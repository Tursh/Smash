using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Multiplier;
    public float Damage;
    public Vector2 Direction;
    public Vector2 Velocity;
    public float FramesOfLife;
    public GameObject Source;

    private Rigidbody2D rigidBody2D;
    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        rigidBody2D.velocity = Velocity * 10;
    }

    protected virtual void FixedUpdate()
    {
        if (FramesOfLife-- <= 0)
            Delete();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject == Source)
            return;
        if (other.gameObject.layer == 14 && other.gameObject.GetComponent<Projectile>().Source == Source)
            return;
        if (other.gameObject.layer == 9)
        {
            Hurt(other.gameObject);
        }
        else if (other.gameObject.layer == 11)
        {
            Hurt(other.gameObject.GetComponent<DummyComponent>().PlayerReference);
        }
        Delete();
    }

    void Hurt(GameObject player)
    {
        player.GetComponent<CharacterData>().Hurt(Direction,Multiplier,Damage);
    }

    protected virtual void Delete()
    {
        Destroy(gameObject);
    }
}
