﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(
    typeof(PlayerPhysics),
    typeof(PlayerInfo))]

public class DefaultCharacter : CharacterData
{

    private static FrameOfAttack[] AAttack;

    static DefaultCharacter()
    {
        
    }

    private void Start()
    {
        AAttack = new FrameOfAttack[]
        {
        };
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Vector2 velocity = Rigidbody.velocity;


        Rigidbody.velocity = velocity;
    }
    
    protected override void Jump()
    {
        base.Jump();
    }
}