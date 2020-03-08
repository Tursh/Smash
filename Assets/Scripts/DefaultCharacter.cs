using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCharacter : CharacterData
{

    public override AttackSet AttackSet { get; set; } = new AttackSet(new AttackGroup(
            new Attack[] { },
            new Attack[] { },
            new Attack[]
            {
                new Attack(SimplePhysicalAttack(new FrameDataPhysical(2,1,1,Vector2.right,Vector2.right))) 
            },
            new Attack[] { }),
        new AttackGroup(),
        new AttackGroup(
            new Attack[] { },
            new Attack[] { },
            new Attack[] { },
            new Attack[] { }),
        new AttackGroup());
}