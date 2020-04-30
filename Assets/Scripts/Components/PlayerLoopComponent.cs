using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoopComponent : LoopComponent
{
    private DummyComponent[] dummyComponents = new DummyComponent[2];
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        for (int i = 0; i < 2; ++i)
        {
            dummyComponents[i] = dummies[i].GetComponent<DummyComponent>(); 
            dummyComponents[i].PlayerReference = gameObject;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.FixedUpdate();
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
}
