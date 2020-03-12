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
    
    protected PlayerControls PlayerControls;
    protected PlayerPhysics PlayerPhysics;

    protected Vector2 LeftJoystickPosition;
    protected Vector2 RightJoystickPosition;
    protected float LTPosition;
    protected float RTPosition;

    protected Attack Attack;
    public AttackState AttackState = AttackState.Idle;

    protected virtual void Awake()
    {
        Attack = new Attack();
        PlayerControls = new PlayerControls();
        PlayerPhysics = GetComponent<PlayerPhysics>();
        PlayerPhysics.OnGroundEventHandler += OnGround;
        
        //vector2
        PlayerControls.Gameplay.LeftJoystick.performed += LeftJoystickOnperformed;
        PlayerControls.Gameplay.RightJoystick.performed += RightJoystickOnperformed;
        PlayerControls.Gameplay.DPad.performed += DPadOnperformed;
        
        //value
        PlayerControls.Gameplay.LT.performed += LTOnperformed;
        PlayerControls.Gameplay.RT.performed += RTOnperformed;
        
        //boolean
        PlayerControls.Gameplay.A.performed += AOnperformed;
        PlayerControls.Gameplay.B.performed += BOnperformed;
        PlayerControls.Gameplay.X.performed += XOnperformed;
        PlayerControls.Gameplay.Y.performed += YOnperformed;
        PlayerControls.Gameplay.LB.performed += LBOnperformed;
        PlayerControls.Gameplay.RB.performed += RBOnperformed;
        PlayerControls.Gameplay.Start.performed += StartOnperformed;
        PlayerControls.Gameplay.Select.performed += SelectOnperformed;
        PlayerControls.Gameplay.LeftJoystickPress.performed += LeftJoystickPressOnperformed;
        PlayerControls.Gameplay.RightJoystickPress.performed += RightJoystickPressOnperformed;
        
        
        //debug stuff with keyboard
        PlayerControls.Gameplay.KeyA.performed += KeyAOnperformed;
        PlayerControls.Gameplay.KeyD.performed += KeyDOnperformed;
        PlayerControls.Gameplay.KeySpace.performed += KeySpaceOnperformed;
    }

    protected virtual void LeftJoystickOnperformed(InputAction.CallbackContext ctx)
    {
        LeftJoystickPosition = ctx.ReadValue<Vector2>();
    }

    protected virtual void RightJoystickOnperformed(InputAction.CallbackContext ctx)
    {
        RightJoystickPosition = ctx.ReadValue<Vector2>();
    }

    protected virtual void DPadOnperformed(InputAction.CallbackContext ctx) { }

    protected virtual void LTOnperformed(InputAction.CallbackContext ctx)
    {
        LTPosition = ctx.ReadValue<float>();
    }

    protected virtual void RTOnperformed(InputAction.CallbackContext ctx)
    {
        RTPosition = ctx.ReadValue<float>();
    }
    protected virtual void AOnperformed(InputAction.CallbackContext ctx) { }
    protected virtual void BOnperformed(InputAction.CallbackContext ctx) { }
    protected virtual void XOnperformed(InputAction.CallbackContext ctx) { }
    protected virtual void YOnperformed(InputAction.CallbackContext ctx) { }
    protected virtual void LBOnperformed(InputAction.CallbackContext ctx) { }
    protected virtual void RBOnperformed(InputAction.CallbackContext ctx) { }
    protected virtual void StartOnperformed(InputAction.CallbackContext ctx) { }
    protected virtual void SelectOnperformed(InputAction.CallbackContext ctx) { }
    protected virtual void LeftJoystickPressOnperformed(InputAction.CallbackContext obj) { }
    protected virtual void RightJoystickPressOnperformed(InputAction.CallbackContext obj) { }


    //debug purposes
    protected virtual void KeySpaceOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton()) Jump();
    }

    protected virtual void KeyAOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton())
            LeftJoystickPosition += Vector2.left;
        else
            LeftJoystickPosition += Vector2.right;
    }

    protected virtual void KeyDOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton())
            LeftJoystickPosition += Vector2.right;
        else
            LeftJoystickPosition += Vector2.left;
    }

    protected float lastJump;
    protected virtual void Jump()
    {
        if (Time.time - lastJump >= jumpCooldown && AttackState != AttackState.Attacking)
        {
            PlayerPhysics.velocity.y = 1;
            lastJump = Time.time;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!Attack.IsEmpty())
        {
            AttackState = AttackState.Attacking;
            Attack.Pop().Act(gameObject);
        }
        else
        {
            AttackState = AttackState.Idle;
        }
    }

    protected virtual void OnGround(object sender, OnGroundEnventArgs args)
    {
        DisableAttack();
    }

    protected void DisableAttack()
    {
        AttackState = AttackState.Idle;
        Attack.Clear();
    }

    private void OnEnable()
    {
        PlayerControls.Enable();
    }

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

    private static Func<GameObject, bool> SimpleProjectileAttack(FrameDataProjectile frameDataProjectile)
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