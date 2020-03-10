using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(
    typeof(AttackManager))]
[RequireComponent(
    typeof(PlayerPhysics),
    typeof(PlayerInfo))]

public class DefaultCharacter : CharacterData
{

    public override GameObject[] Prefabs { get; set; }

    private PlayerPhysics playerPhysics;
    
    private void Start()
    {
        playerPhysics = GetComponent<PlayerPhysics>();
    }

    private void FixedUpdate()
    {
        playerPhysics.velocity.x += LeftJoystickPosition.x * mvSpeed;
    }

    protected override void OnB()
    {
        playerPhysics.Jump();
    }

    protected override void OnY()
    {
        OnB();
    }

}