using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


public struct ColliderFixInfo
{
    public Vector2 size;
    public Vector2 offset;

    public ColliderFixInfo(Vector2 size, Vector2 offset)
    {
        this.size = size;
        this.offset = offset;
    }
}

public class BoxCollider2DFix : MonoBehaviour
{
    [SerializeField] private Animator CharacterAnimator;
    private Dictionary<String, ColliderFixInfo> Fixes;
    [SerializeField] private BoxCollider2D CharacterBoxCollider;
    private String lastState;

    private void Awake()
    {
        if (CharacterAnimator == null)
            CharacterAnimator = GetComponent<Animator>();
        Fixes = new Dictionary<string, ColliderFixInfo>();
    }

    public void AddBoxColliderFix(String AnimationStateName, ColliderFixInfo ColliderInfo)
    {
        Fixes.Add(AnimationStateName, ColliderInfo);
    }

    private void Update()
    {
        var AnimationState = CharacterAnimator.GetNextAnimatorStateInfo(0);
        foreach (var Fix in Fixes)
        {
            if (AnimationState.IsName(Fix.Key))
            {
                if (lastState != Fix.Key)
                {
                    CharacterBoxCollider.size = Fix.Value.size;
                    CharacterBoxCollider.offset = Fix.Value.offset;
                    lastState = Fix.Key;
                }

                break;
            }
        }
    }
}