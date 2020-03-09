using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Playables;

public abstract class CharacterData : MonoBehaviour
{
    public abstract AttackSet AttackSet { get; set; }
    
    protected static Func<GameObject,bool> SimplePhysicalAttack(FrameDataPhysical frameDataPhysical)
    {

        return o =>
        {
            GameObject hurtboxGameObject = Instantiate(frameDataPhysical.HurtboxPrefab, o.transform);
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
    
    protected static Func<GameObject,bool> ComplexPhysicalAttack(FrameDataPhysical[] framesDataPhysical)
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
                
                GameObject hurtboxGameObject = Instantiate(currentFrameData.HurtboxPrefab, o.transform);
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

    private static Func<GameObject, bool> simpleProjectileAttack(FrameDataProjectile frameDataProjectile)
    {
        return o =>
        {
            FrameDataProjectile tempFrameDataProjectile;
            if (o.GetComponent<PlayerPhysics>().Facing == Vector2.left)
                tempFrameDataProjectile = frameDataProjectile.Reversed();
            else
                tempFrameDataProjectile = frameDataProjectile;

            GameObject projectileGameObject = Instantiate(frameDataProjectile.ProjectilePrefab,o.transform.position, o.transform.localRotation);
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