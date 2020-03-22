using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(
    typeof(PlayerPhysics),
    typeof(PlayerInfo))]

public class DefaultCharacter : CharacterData
{

    private Rigidbody2D Rigidbody;

    private static FrameOfAttack[] AAttack;
    static DefaultCharacter()
    {
        AAttack = new FrameOfAttack[]
        {
            new FrameOfAttack(SimplePhysicalAttack(new FrameDataPhysical()))
        };
    }
    
    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Rigidbody.AddForce(Vector2.right * LeftJoystickPosition.x * mvSpeed);
    }

    protected override void Jump()
    {
        base.Jump();
    }

    protected override void AOnperformed(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton())
        {
            
        }
    }
}