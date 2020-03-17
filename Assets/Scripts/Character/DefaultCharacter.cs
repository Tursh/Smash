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

    private PlayerPhysics playerPhysics;

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
        playerPhysics = GetComponent<PlayerPhysics>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        playerPhysics.velocity.x += LeftJoystickPosition.x * mvSpeed;
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