using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopComponent : MonoBehaviour
{
    
    [SerializeField] protected GameObject dummyPrefab;
    protected GameObject objectLooping;
    protected Collider2D Collider2D;
    protected Vector2 startWindowPosition, endWindowPosition, windowSizeInWorld;
    
    protected GameObject[] dummies = new GameObject[2];

    // Start is called before the first frame update
    protected void Start()
    {
        Collider2D = GetComponent<Collider2D>();
        
        Camera cam = Camera.main;
        startWindowPosition = cam.ScreenToWorldPoint(new Vector3(0, 0, 12));
        endWindowPosition = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 12));
        windowSizeInWorld = endWindowPosition - startWindowPosition;
        bool isDummyPrefabSelf = dummyPrefab == null;
        
        if (isDummyPrefabSelf)
            dummyPrefab = gameObject;
        
        for (int i = 0; i < 2; ++i)
        {
            dummies[i] =
                Instantiate(dummyPrefab,
                    startWindowPosition - Vector2.down * 10,
                    Quaternion.identity);
            if (isDummyPrefabSelf)
            {
                dummies[i].GetComponent<LoopComponent>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        Vector3 position = transform.position;
        Bounds bounds = Collider2D.bounds;

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
            dummies[axis].transform.rotation = transform.rotation;
        }
    }
}
