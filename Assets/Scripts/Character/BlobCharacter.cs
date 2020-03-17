using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobCharacter : CharacterData
{
    private PlayerPhysics playerPhysics;

    private static FrameOfAttack[] AAttack;
    static BlobCharacter()
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
}
