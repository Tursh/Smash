using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoopComponent : LoopComponent
{
    private DummyComponent[] dummyComponents = new DummyComponent[2];
    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < 2; ++i)
        {
            dummyComponents[i] = dummies[i].GetComponent<DummyComponent>(); 
            dummyComponents[i].PlayerReference = gameObject;
        }
    }

    public void SetDummyAnimatorState(string state, bool status)
    {
        foreach(var dummyComponent in dummyComponents)
           dummyComponent.SetAnimationState(state, status);
    }
    
    public void SetDummyAnimatorState(int state, bool status)
    {
        foreach(var dummyComponent in dummyComponents)
            dummyComponent.SetAnimationState(state, status);
    }

    public void TriggerDummyAnimatorState(string state)
    {
        foreach (var dummyComponent in dummyComponents)
            dummyComponent.TriggerAnimationState(state);
    }
    public void TriggerDummyAnimatorState(int state)
    {
        foreach (var dummyComponent in dummyComponents)
            dummyComponent.TriggerAnimationState(state);
    }
    
    public void SetDummyAnimatorState(int state, int status)
    {
        foreach (var dummyComponent in dummyComponents)
            dummyComponent.SetAnimationState(state, status);
    }

    public void setDummyRotation(Quaternion rotation)
    {
        foreach (var dummy in dummies)
            dummy.transform.rotation = rotation;
    }
}
