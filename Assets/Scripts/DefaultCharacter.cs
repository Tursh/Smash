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
                new Attack(delegate(GameObject o) { return SimplePhysicalAttack(o, new FrameDataPhysical()); })
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