using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFunctions : MonoBehaviour
{
    public static Func<GameObject, bool> SimplePhysicalAttack(FrameDataPhysical frameDataPhysical)
    {
        return o =>
        {
            GameObject hurtboxGameObject = Instantiate(o.GetComponent<CharacterData>().Prefabs[frameDataPhysical.PrefabIndex], o.transform);
            CircleCollider2D circleCollider2D = hurtboxGameObject.GetComponent<CircleCollider2D>();
            HurtboxComponent hurtboxComponent = hurtboxGameObject.GetComponent<HurtboxComponent>();

            FrameDataPhysical tempFrameDataPhysical;

            if (o.GetComponent<PlayerPhysics>().Facing == Vector2.left)
                tempFrameDataPhysical = frameDataPhysical.Reversed();
            else
                tempFrameDataPhysical = frameDataPhysical;
            circleCollider2D.offset = tempFrameDataPhysical.Offset;
            circleCollider2D.radius = tempFrameDataPhysical.Radius;
            hurtboxComponent.Direction = tempFrameDataPhysical.Direction;
            hurtboxComponent.Multiplier = tempFrameDataPhysical.Multiplier;
            hurtboxComponent.Damage = tempFrameDataPhysical.Damage;
            hurtboxComponent.FramesOfLife = tempFrameDataPhysical.FramesOfLife;
            return true;
        };
    }

    public static Func<GameObject, bool> ComplexPhysicalAttack(FrameDataPhysical[] framesDataPhysical)
    {
        return o =>
        {
            for (int i = 0; i < framesDataPhysical.Length; ++i)
            {
                FrameDataPhysical currentFrameData;
                if (o.GetComponent<PlayerPhysics>().Facing == Vector2.left)
                    currentFrameData = framesDataPhysical[i].Reversed();
                else
                    currentFrameData = framesDataPhysical[i];

                GameObject hurtboxGameObject = Instantiate(o.GetComponent<CharacterData>().Prefabs[framesDataPhysical[i].PrefabIndex], o.transform);
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



    public static Func<GameObject, bool> SimpleProjectileAttack(FrameDataProjectile frameDataProjectile)
    {
        return o =>
        {
            FrameDataProjectile tempFrameDataProjectile;
            if (o.GetComponent<PlayerPhysics>().Facing == Vector2.left)
                tempFrameDataProjectile = frameDataProjectile.Reversed();
            else
                tempFrameDataProjectile = frameDataProjectile;

            GameObject projectileGameObject = Instantiate(
                o.GetComponent<CharacterData>().Prefabs[frameDataProjectile.PrefabIndex], 
                o.transform.position,
                o.transform.localRotation);
            Projectile projectile = projectileGameObject.GetComponent<Projectile>();
            projectile.FramesOfLife = tempFrameDataProjectile.FramesOfLife;
            projectile.Multiplier = tempFrameDataProjectile.Multiplier;
            projectile.Damage = tempFrameDataProjectile.Damage;
            projectile.Direction = tempFrameDataProjectile.Direction;
            projectile.Velocity = tempFrameDataProjectile.Velocity;
            projectile.FramesOfLife = tempFrameDataProjectile.FramesOfLife;
            projectile.GetComponent<CircleCollider2D>().radius = tempFrameDataProjectile.Radius;

            return true;
        };
    }
}
