using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Playables;

public abstract class CharacterData : MonoBehaviour
{
    public abstract AttackSet AttackSet { get; set; }

    static protected Func<GameObject,bool> SimplePhysicalAttack(FrameDataPhysical frameDataPhysical)
    {

        return o =>
        {
            CircleCollider2D[] colliders = o.GetComponentsInChildren<CircleCollider2D>();
            HurtboxComponent[] hurtboxComponents = o.GetComponentsInChildren<HurtboxComponent>();

            FrameDataPhysical tempFrameDataPhysical;

            if (o.GetComponent<PlayerPhysics>().Facing == Vector2.left)
                tempFrameDataPhysical = frameDataPhysical.reversed();
            else
                tempFrameDataPhysical = frameDataPhysical;
            colliders[0].offset = tempFrameDataPhysical.Offset;
            colliders[0].radius = tempFrameDataPhysical.Radius;
            hurtboxComponents[0].Direction = tempFrameDataPhysical.Direction;
            hurtboxComponents[0].Multiplier = tempFrameDataPhysical.Multiplier;
            hurtboxComponents[0].Damage = tempFrameDataPhysical.Damage;
            hurtboxComponents[0].Enabled = true;
            return true;
        };
    }
    
    static protected Func<GameObject,bool> ComplexPhysicalAttack(FrameDataPhysical[] fdps)
    {
        return o =>
        {
            CircleCollider2D[] colliders = o.GetComponentsInChildren<CircleCollider2D>();
            HurtboxComponent[] hurtboxComponents = o.GetComponentsInChildren<HurtboxComponent>();

            if (colliders.Length != hurtboxComponents.Length)
                Debug.Log(
                    "The amount of colliders do not fit the amount of hurtboxComponents which do not make sense \n" +
                    "possible crash in the next frames");
            if (fdps.Length > colliders.Length)
            {
                Debug.Log("Not enough hurtboxes, ignoring the last " + (fdps.Length - colliders.Length).ToString() +
                          " frame datas");
                FrameDataPhysical[] newFdps = new FrameDataPhysical[colliders.Length];

                for (int i = 0; i < colliders.Length; ++i)
                {
                    newFdps[i] = fdps[i];
                }

                fdps = newFdps;
            }
            
            for (int i = 0; i < fdps.Length; ++i)
            {
                FrameDataPhysical currentFrameData;
                if (o.GetComponent<PlayerPhysics>().Facing == Vector2.left)
                    currentFrameData = fdps[i].reversed();
                else
                    currentFrameData = fdps[i];
                
                colliders[i].radius = currentFrameData.Radius;
                colliders[i].offset = currentFrameData.Offset;
                hurtboxComponents[i].Damage = currentFrameData.Damage;
                hurtboxComponents[i].Direction = currentFrameData.Direction;
                hurtboxComponents[i].Multiplier = currentFrameData.Multiplier;
                hurtboxComponents[i].Enabled = true;
            }

            return true;
        };
    }

    static protected Func<GameObject, bool> simpleProjectileAttack(FrameDataProjectile frameDataProjectile)
    {
        return o =>
        {
            FrameDataProjectile tempFrameDataProjectile;
            if (o.GetComponent<PlayerPhysics>().Facing == Vector2.left)
                tempFrameDataProjectile = frameDataProjectile.reversed();
            else
                tempFrameDataProjectile = frameDataProjectile;

            GameObject projectile = Instantiate(o.GetComponent<AttackManager>().ProjectilePrefabs[0],o.transform.position, o.transform.localRotation);
            projectile.GetComponent<Projectile>().TimeLeftToLive = tempFrameDataProjectile.TimeToLive;
            projectile.GetComponent<Projectile>().Multiplier = tempFrameDataProjectile.Multiplier;
            projectile.GetComponent<Projectile>().Damage = tempFrameDataProjectile.Damage;
            projectile.GetComponent<Projectile>().Direction = tempFrameDataProjectile.Direction;
            projectile.GetComponent<CircleCollider2D>().radius = tempFrameDataProjectile.Radius;
            projectile.GetComponent<Projectile>().Velocity = tempFrameDataProjectile.Velocity;
            
            return true;
        };

    }
    
}