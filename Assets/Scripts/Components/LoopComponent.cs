using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopComponent : MonoBehaviour
{
    
    [SerializeField] private GameObject dummyPrefab;
    
    private BoxCollider2D BoxCollider;
    private Vector2 startWindowPosition, endWindowPosition, windowSizeInWorld;
    
    private GameObject[] dummies = new GameObject[2];
    private DummyComponent[] dummyComponents = new DummyComponent[2];
    
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
        
        Camera cam = Camera.main;
        startWindowPosition = cam.ScreenToWorldPoint(new Vector3(0, 0, 12));
        endWindowPosition = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 12));
        windowSizeInWorld = endWindowPosition - startWindowPosition;
        
        for (int i = 0; i < 2; ++i)
        {
            dummies[i] =
                Instantiate(dummyPrefab,
                    startWindowPosition - Vector2.down * 10,
                    Quaternion.identity);
            dummyComponents[i] = dummies[i].GetComponent<DummyComponent>(); 
            dummyComponents[i].PlayerReference = gameObject;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 position = transform.position;
        Bounds bounds = BoxCollider.bounds;

        //x et y, x: 0, y: 1
        for (int axis = 0; axis < 2; ++axis)
        {
            //To the left / down
            //Is the player partly in the border
            if (bounds.min[axis] < startWindowPosition[axis] && bounds.max[axis] < startWindowPosition[axis])
            {
                //Set to the other side of the window
                position[axis] += windowSizeInWorld[axis];
                transform.position = position;
            }

            //To right / up
            else if (endWindowPosition[axis] < bounds.max[axis] && endWindowPosition[axis] < bounds.min[axis])
            {
                //Set to the other side of the window
                position[axis] -= windowSizeInWorld[axis];
                transform.position = position;
            }

            Vector3 dummyPositon = dummies[axis].transform.position;
            dummyPositon[axis] = transform.position[axis] +
                                 (transform.position[axis] > 0 ? -1 : 1) * windowSizeInWorld[axis];
            dummyPositon[(axis + 1) % 2] = transform.position[(axis + 1) % 2];
            dummies[axis].transform.position = dummyPositon;
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
}
