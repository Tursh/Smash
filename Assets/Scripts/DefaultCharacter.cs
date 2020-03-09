using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCharacter : CharacterData
{
    private static GameObject[] ProjectilePrefabs;
    private static GameObject[] HurtboxPrefabs;
    
    public override AttackSet AttackSet { get; set; } = new AttackSet(new AttackGroup(
            new Attack[] { },
            new Attack[] { },
            new Attack[]
            {
                new Attack(SimplePhysicalAttack(new FrameDataPhysical(HurtboxPrefabs[0])))
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