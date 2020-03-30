using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;



public abstract class CharacterData : MonoBehaviour
{
    public float mvSpeed = 0.025f;
    public float airResistance = 0.9f;
    public float gravity = -0.05f;
    public float jumpCooldown = 0.25f;
    public float JumpMultiplier = 10;
    public float distanceToGround = 1;

    public GameObject[] Prefabs;
    
    protected PlayerControls PlayerControls;
    protected Rigidbody2D Rigidbody;

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
        Rigidbody = GetComponent<Rigidbody2D>();
        
        //PlayerPhysics.OnGroundEventHandler += OnGround;
        
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
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x,JumpMultiplier);
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

    protected bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector2.down, distanceToGround);
    }
}