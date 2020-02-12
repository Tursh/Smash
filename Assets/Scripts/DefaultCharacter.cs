using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DefaultCharacter : CharacterData
{
    private AttackSet attackSet;

    public override AttackSet AttackSet
    {
        get { return attackSet; }
        set
        {
            attackSet.LightAerial = new AttackGroup(
                new Attack[] { },
                new Attack[] { },
                new Attack[]
                {
                    new Attack(Vector2.right, 0.1f, 1f,
                        new Vector2(2, 2), new Vector2(2,0)),
                    new Attack(Vector2.right, 0.1f, 1f,
                        new Vector2(4, 4), new Vector2(2,0))
                },
                new Attack[] { });
        }
    }
}
