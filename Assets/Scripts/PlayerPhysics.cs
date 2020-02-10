using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;

public enum Direction
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public enum PLAYER_STATE
{
    ON_GROUND, IN_AIR, ON_EDGE
}

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] private float mvSpeed = 0.025f, airResistance = 0.9f, gravity = -0.05f, jumpCooldown = 0.25f;

    [SerializeField] private GameObject DummyPrefab;

    public Vector2 velocity;
    private Vector2 startWindowPosition, endWindowPosition, windowSizeInWorld;

    BoxCollider2D bc;
    Rigidbody2D rb;

    public PLAYER_STATE PlayerState { get; private set; } = PLAYER_STATE.IN_AIR;

    private int playerLayer = 9, stateLayer = 8;

    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();


        Camera cam = Camera.main;

        startWindowPosition = cam.ScreenToWorldPoint(new Vector3(0, 0, 12));
        endWindowPosition = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 12));
        windowSizeInWorld = endWindowPosition - startWindowPosition;

        for (int i = 0; i < 2; ++i)
            Dummies[i] =
                Instantiate(DummyPrefab,
                    startWindowPosition - Vector2.down * 10,
                    Quaternion.identity);
    }

    private float lastCollision = 0;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        //TODO: Do damage depending on the velocity

        if (other.collider.gameObject.layer == stateLayer)
        {
            Vector3 normal = other.contacts[0].normal;

            if (normal == new Vector3(0, 1, 0))
            {
                PlayerState = PLAYER_STATE.ON_GROUND;
                transform.SetParent(other.transform);
            }

            if (normal.y != 0)
            {
                Vector2 position = transform.position;

                position.y = (PlayerState == PLAYER_STATE.ON_GROUND ? other.collider.bounds.max : other.collider.bounds.min).y +
                             bc.bounds.extents.y * normal.y;
                transform.position = position;
                velocity.y = 0;
            }
            else
            {
                //Vector2 position = transform.position;
                //position.x = (normal.x > 0 ? other.collider.bounds.max : other.collider.bounds.min).x +
                //             (bc.bounds.extents.x + 0.03f) * normal.x;
                //transform.position = position;
                //velocity.x = 0;
                velocity = Vector2.zero;

                PlayerState = PLAYER_STATE.ON_EDGE;
                transform.SetParent(other.transform);

                Bounds stageBounds = other.collider.bounds;

                Vector2 edgePosition;
                edgePosition.x = (normal.x > 0 ? stageBounds.max.x : stageBounds.min.x) + bc.bounds.extents.x * normal.x;
                edgePosition.y = stageBounds.max.y - bc.bounds.extents.y;

                transform.position = edgePosition;
            }
        }

        lastCollision = Time.time;
        
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.gameObject.layer == stateLayer)
        {
            PlayerState = PLAYER_STATE.IN_AIR;
            transform.SetParent(null);
        }
    }

    public void FixedUpdate()
    {
        velocity *= airResistance;

        if (PlayerState == PLAYER_STATE.IN_AIR)
            velocity.y += gravity;

        transform.Translate(velocity);

        checkWindowBorders();
    }

    public void Move(Direction direction)
    {
        if(PlayerState != PLAYER_STATE.ON_EDGE)
        switch (direction)
        {
            case Direction.LEFT:
                velocity += Vector2.left * mvSpeed;
                break;
            case Direction.RIGHT:
                velocity += Vector2.right * mvSpeed;
                break;
            case Direction.UP:

                break;
            case Direction.DOWN:
                break;
        }
    }

    public void StartMovement(Direction direction)
    {
        if(PlayerState != PLAYER_STATE.ON_EDGE)
        switch (direction)
        {
            case Direction.LEFT:
                velocity += Vector2.left * mvSpeed * 10;
                break;
            case Direction.RIGHT:
                velocity += Vector2.right * mvSpeed * 10;
                break;
            case Direction.UP:
                GameObject.Find("Stage").transform.Translate(Vector3.up * 0.1f);
                break;
            case Direction.DOWN:
                velocity += Vector2.down * mvSpeed * 10;
                break;
        }
    }

    private float lastJump;

    public void Jump()
    {
        if (Time.time - lastJump >= jumpCooldown)
        {
            velocity.y = 1;
            lastJump = Time.time;
        }
    }

    private bool[] onBorder = new bool[2];
    private GameObject[] Dummies = new GameObject[2];

    void checkWindowBorders()
    {
        Vector3 position = transform.position;
        Bounds bounds = bc.bounds;

        //x et y, x: 0, y: 1
        for (int axis = 0; axis < 2; ++axis)
        {
            //To the left / down
            //Is the player partly in the border
            if (bounds.min[axis] < startWindowPosition[axis])
            {
                //Is the player out of the screen
                if (bounds.max[axis] < startWindowPosition[axis])
                {
                    //Set to the other side of the window
                    position[axis] += windowSizeInWorld[axis];
                    transform.position = position;

                    onBorder[axis] = false;
                }
                else
                {
                    onBorder[axis] = true;

                    Vector3 dummyPosition = Dummies[axis].transform.position;
                    dummyPosition[axis] = position[axis] + windowSizeInWorld[axis];

                    if (!onBorder[(axis + 1) % 2])
                        dummyPosition[(axis + 1) % 2] = position[(axis + 1) % 2];

                    Dummies[axis].transform.position = dummyPosition;
                }
            }
            else
                onBorder[axis] = false;

            //To right / up
            if (endWindowPosition[axis] < bounds.max[axis])
            {
                //Is the player out of the screen
                if (endWindowPosition[axis] < bounds.min[axis])
                {
                    //Set to the other side of the window
                    position[axis] -= windowSizeInWorld[axis];
                    transform.position = position;

                    onBorder[axis] = false;
                }
                else
                {
                    onBorder[axis] = true;

                    Vector3 dummyPosition = Dummies[axis].transform.position;
                    dummyPosition[axis] = position[axis] - windowSizeInWorld[axis];

                    if (!onBorder[(axis + 1) % 2])
                        dummyPosition[(axis + 1) % 2] = position[(axis + 1) % 2];

                    Dummies[axis].transform.position = dummyPosition;
                }
            }
            else
                onBorder[axis] = false;

            if (!onBorder[axis])
                Dummies[axis].transform.position = -windowSizeInWorld;
        }
    }
}