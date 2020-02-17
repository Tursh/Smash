using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxComponent : MonoBehaviour
{
    [SerializeField] public Vector2 Direction;
    [SerializeField] public float Damage;
    [SerializeField] public float Multiplier;
    [SerializeField] public bool Enabled;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9 && Enabled)
        {
            PlayerPhysics playerPhysics = other.gameObject.GetComponent<PlayerPhysics>();
            PlayerInfo playerInfo = other.gameObject.GetComponent<PlayerInfo>();
            playerPhysics.velocity += Direction * (Multiplier * (playerInfo.Damage + 1));
            playerInfo.Damage += Damage;
        }
            
     }
}
