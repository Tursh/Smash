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
    void FixedUpdate()
    {
        transform.position += new Vector3(Velocity.x,Velocity.y);
        
        if (FramesOfLife-- <= 0)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
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

    void Hurt(GameObject player)
    {
        PlayerPhysics playerPhysics = player.gameObject.GetComponent<PlayerPhysics>();
        PlayerInfo playerInfo = player.gameObject.GetComponent<PlayerInfo>();
        playerPhysics.velocity += Direction * (Multiplier * (playerInfo.Damage + 1));
        playerInfo.Damage += Damage;
        Destroy(gameObject);
    }
}
