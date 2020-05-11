using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyComponent : MonoBehaviour
{
    public GameObject PlayerReference;

    private Animator Animator;

    public void Start()
    {
        Animator = GetComponent<Animator>();
    }

    public void SetAnimationState(string state, bool status)
    {
        Animator.SetBool(state, status);
    }
    
    public void SetAnimationState(int state, bool status)
    {
        Animator.SetBool(state, status);
    }
    
    public void SetAnimationState(int state, int status)
    {
        Animator.SetInteger(state, status);
    }
}
