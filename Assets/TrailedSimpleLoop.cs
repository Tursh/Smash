using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TrailedSimpleLoop : LoopComponent
{
    protected override void FixedUpdate()
    {
        for (int axis = 0; axis < 2; ++axis)
        {
            Vector3 dummyPositon = dummies[axis].transform.position;
            dummyPositon[axis] = transform.position[axis] +
                                 (transform.position[axis] > 0 ? -1 : 1) * windowSizeInWorld[axis];
            dummyPositon[(axis + 1) % 2] = transform.position[(axis + 1) % 2];
            dummies[axis].transform.position = dummyPositon;
            dummies[axis].transform.rotation = transform.rotation;
        }
    }
}
