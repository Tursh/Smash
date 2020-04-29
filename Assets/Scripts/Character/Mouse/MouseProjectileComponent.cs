using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseProjectileComponent : Projectile
{
    public GameObject ExplosionHurtboxPrefab;
    protected override void Delete()
    {
        Velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponentInChildren<ParticleSystem>().Play();
        //GetComponentInChildren<TrailRenderer>().Clear();
        Destroy(gameObject,1f);
        enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        Instantiate(ExplosionHurtboxPrefab);
    }
}
