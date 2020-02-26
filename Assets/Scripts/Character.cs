using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public abstract class CharacterData : MonoBehaviour
{
    public abstract AttackSet AttackSet { get; set; }

    static protected bool SimplePhysicalAttack(GameObject player, FrameDataPhysical fdp)
    {
        CircleCollider2D[] colliders = player.GetComponentsInChildren<CircleCollider2D>();
        HurtboxComponent[] hurtboxComponents = player.GetComponentsInChildren<HurtboxComponent>();

        colliders[0].radius = fdp.Radius;
        colliders[0].offset = fdp.Offset;
        hurtboxComponents[0].Damage = fdp.Damage;
        hurtboxComponents[0].Direction = fdp.Direction;
        hurtboxComponents[0].Multiplier = fdp.Multiplier;
        
        return true;
    }
    
    static protected bool ComplexPhysicalAttack(GameObject player, FrameDataPhysical[] fdps)
    {
        CircleCollider2D[] colliders = player.GetComponentsInChildren<CircleCollider2D>();
        HurtboxComponent[] hurtboxComponents = player.GetComponentsInChildren<HurtboxComponent>();

        if (colliders.Length != hurtboxComponents.Length)
            Debug.Log("The amount of colliders do not fit the amount of hurtboxComponents which do not make sense \n" +
                      "possible crash in the next frames");
        if (fdps.Length > colliders.Length)
        {
            Debug.Log("Not enough hurtboxes, ignoring the last " + (fdps.Length - colliders.Length).ToString() +
                " frame datas");
            FrameDataPhysical[] newFdps = new FrameDataPhysical[colliders.Length] ;

            for (int i = 0; i < colliders.Length; ++i)
            {
                newFdps[i] = fdps[i];
            }

            fdps = newFdps;
        }

        for (int i = 0; i < fdps.Length; ++i)
        {
            colliders[i].radius = fdps[i].Radius;
            colliders[i].offset = fdps[i].Offset;
            hurtboxComponents[i].Damage = fdps[i].Damage;
            hurtboxComponents[i].Direction = fdps[i].Direction;
            hurtboxComponents[i].Multiplier = fdps[i].Multiplier;
        }
        
        return true;
    }
}