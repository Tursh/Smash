using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHurtbox : HurtboxComponent
{
    protected override void Hurt(GameObject o)
    {
        CharacterData characterData = o.GetComponent<CharacterData>();
        if (characterData == null)
            characterData = o.transform.parent.GetComponent<CharacterData>();

        characterData.Hurt(Direction,Multiplier,Damage);
    }
    
}
