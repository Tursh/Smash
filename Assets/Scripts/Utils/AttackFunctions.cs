using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackFunctions : MonoBehaviour
{
    public static Func<GameObject, bool> SimplePhysicalAttack(FrameDataPhysical frameDataPhysical)
    {
        return o =>
        {
            GameObject hurtboxGameObject = Instantiate(frameDataPhysical.Prefab, o.transform);
            CircleCollider2D circleCollider2D = hurtboxGameObject.GetComponent<CircleCollider2D>();
            HurtboxComponent hurtboxComponent = hurtboxGameObject.GetComponent<HurtboxComponent>();

            FrameDataPhysical tempFrameDataPhysical = frameDataPhysical;

            circleCollider2D.offset = tempFrameDataPhysical.Offset;
            circleCollider2D.radius = tempFrameDataPhysical.Radius;
            hurtboxComponent.Direction = tempFrameDataPhysical.Direction;
            hurtboxComponent.Multiplier = tempFrameDataPhysical.Multiplier;
            hurtboxComponent.Damage = tempFrameDataPhysical.Damage;
            hurtboxComponent.FramesOfLife = tempFrameDataPhysical.FramesOfLife;
            hurtboxComponent.setKnockback = tempFrameDataPhysical.SetKnockBack;
            return true;
        };
    }

    public static Func<GameObject, bool> ComplexPhysicalAttack(FrameDataPhysical[] framesDataPhysical)
    {
        return o =>
        {
            foreach (var t in framesDataPhysical)
            {
                FrameDataPhysical currentFrameData;
                if (o.GetComponent<PlayerPhysics>().Facing == Vector2.left)
                    currentFrameData = t.Reversed();
                else
                    currentFrameData = t;

                GameObject hurtboxGameObject =
                    Instantiate(t.Prefab,
                        o.transform);
                CircleCollider2D circleCollider2D = hurtboxGameObject.GetComponent<CircleCollider2D>();
                HurtboxComponent hurtboxComponent = hurtboxGameObject.GetComponent<HurtboxComponent>();


                circleCollider2D.radius = currentFrameData.Radius;
                circleCollider2D.offset = currentFrameData.Offset;
                hurtboxComponent.Damage = currentFrameData.Damage;
                hurtboxComponent.Direction = currentFrameData.Direction;
                hurtboxComponent.Multiplier = currentFrameData.Multiplier;
                hurtboxComponent.FramesOfLife = currentFrameData.FramesOfLife;
            }

            return true;
        };
    }



    public static FrameOfAttack SimpleProjectileAttack(FrameDataProjectile frameDataProjectile)
    {
        return new FrameOfAttack(o =>
            {
                FrameDataProjectile tempFrameDataProjectile = frameDataProjectile;
                GameObject projectileGameObject = Instantiate(
                    frameDataProjectile.Prefab,
                    o.transform.position + 
                    new Vector3(frameDataProjectile.SpawnOffset.x, 
                                frameDataProjectile.SpawnOffset.y),
                    o.transform.localRotation);
                Projectile projectile = projectileGameObject.GetComponent<Projectile>();
                projectile.Source = o;
                projectile.FramesOfLife = tempFrameDataProjectile.FramesOfLife;
                projectile.Multiplier = tempFrameDataProjectile.Multiplier;
                projectile.Damage = tempFrameDataProjectile.Damage;
                projectile.Direction = tempFrameDataProjectile.Direction;
                projectile.Velocity = tempFrameDataProjectile.Velocity;
                projectile.FramesOfLife = tempFrameDataProjectile.FramesOfLife;
                projectile.GetComponent<CircleCollider2D>().radius = tempFrameDataProjectile.Radius;

                return true;
            }
        );
    }
}
