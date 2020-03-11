using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public abstract class CharacterData : MonoBehaviour
{
    public virtual float mvSpeed { get; set; } = 0.025f;
    public virtual float airResistance { get; set; } = 0.9f;
    public virtual float gravity { get; set; } = -0.05f;
    public virtual float jumpCooldown { get; set; } = 0.25f;
    public abstract GameObject[] Prefabs { get; set; }
    
    protected Vector2 LeftJoystickPosition;
    protected Vector2 RightJoystickPosition;
    protected float LTPosition;
    protected float RTPosition;

    protected virtual void OnKeySpace()
    {
        gameObject.SendMessage("OnB");
    }

    protected virtual void OnKeyA(InputValue value)
    { 
        Debug.Log(value.Get<bool>());
        
        LeftJoystickPosition = Vector2.left;
    }

    protected virtual void OnKeyD(InputAction.CallbackContext value)
    {
        if (value.performed)
            LeftJoystickPosition = Vector2.right;
        else
            LeftJoystickPosition = Vector2.zero;
    }
    
    protected virtual void OnLeftJoystick(InputAction.CallbackContext value)
    {
        LeftJoystickPosition = value.ReadValue<Vector2>();
    }

    protected virtual void OnLeftJoystickPress(){}

    protected virtual void OnRightJoystick(InputAction.CallbackContext value)
    {
        RightJoystickPosition = value.ReadValue<Vector2>();
    }
    protected virtual void OnRightJoystickPress(){}
    protected virtual void OnA(){}
    protected virtual void OnB(){}
    protected virtual void OnX(){}
    protected virtual void OnY(){}
    protected virtual void OnLB(){}
    protected virtual void OnRB(){}

    protected virtual void OnLT(InputValue value)
    {
        LTPosition = value.Get<float>();
    }

    protected virtual void OnRT(InputValue value)
    {
        RTPosition = value.Get<float>();
    }
    protected virtual void OnDPad(InputValue value){}
    protected virtual void OnStart(){}
    protected virtual void OnSelect(){}
    
    
    protected static Func<GameObject, bool> SimplePhysicalAttack(FrameDataPhysical frameDataPhysical)
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

    protected static Func<GameObject, bool> ComplexPhysicalAttack(FrameDataPhysical[] framesDataPhysical)
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

    private static Func<GameObject, bool> simpleProjectileAttack(FrameDataProjectile frameDataProjectile)
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