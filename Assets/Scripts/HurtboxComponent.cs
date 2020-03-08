using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxComponent : MonoBehaviour
{
    public Vector2 Direction;
    public float Damage;
    public float Multiplier;
    public bool Enabled;
    private AttackManager parentAttackManager;

    private void Start()
    {
        parentAttackManager = GetComponentInParent<AttackManager>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Enabled)
        {
            if (other.gameObject.layer == 9)
            {
                Hurt(other.gameObject);
            }
            else if (other.gameObject.layer == 11)
            {
                Hurt(other.gameObject.GetComponent<DummyComponent>().PlayerReference);
            }
        }
    }

    private void Hurt(GameObject player)
    {
        PlayerPhysics playerPhysics = player.gameObject.GetComponent<PlayerPhysics>();
        PlayerInfo playerInfo = player.gameObject.GetComponent<PlayerInfo>();
        playerPhysics.velocity += Direction * (Multiplier * (playerInfo.Damage + 1));
        playerInfo.Damage += Damage;
        Enabled = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerEnter2D(other);
    }
}