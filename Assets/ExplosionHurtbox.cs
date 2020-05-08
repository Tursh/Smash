using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHurtbox : HurtboxComponent
{
    protected override void Hurt(GameObject o)
    {
        o.GetComponent<CharacterData>().Hurt((o.transform.position - transform.position).normalized,Multiplier,Damage);
    }
    
}
