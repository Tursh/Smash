﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

public enum Direction
{
    Left,
    Right,
    Up,
    Down
}

public enum PlayerState
{
    OnGround,
    InAir,
    OnEdge
}



public class OnGroundEnventArgs
{
}

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] private float mvSpeed = 0.025f, airResistance = 0.9f, gravity = -0.05f, jumpCooldown = 0.25f;
    [SerializeField] private GameObject dummyPrefab;
    [SerializeField] public PlayerState PlayerState = PlayerState.InAir;

    private AttackManager AttackManager;

    public Vector2 Facing = Vector2.left;
    public Vector2 velocity;
    private Vector2 startWindowPosition, endWindowPosition, windowSizeInWorld;

    BoxCollider2D BoxCollider;
    
    private int playerLayer = 9, stateLayer = 8;

    private void Start()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
        AttackManager = GetComponent<AttackManager>();
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
            dummies[i].GetComponent<DummyComponent>().PlayerReference = gameObject;
        }
    }
    public EventHandler<OnGroundEnventArgs> OnGroundEventHandler;

    private void OnCollisionEnter2D(Collision2D other)
    {
        //TODO: Do damage depending on the velocity

        if (other.collider.gameObject.layer == stateLayer)
        {
            Vector3 normal = other.contacts[0].normal;

            //Check if normal is pointed downwards
            if (normal == new Vector3(0, 1, 0))
            {
                PlayerState = PlayerState.OnGround;
                transform.SetParent(other.transform);
                OnGroundEventHandler?.Invoke(this, new OnGroundEnventArgs());
            }

            if (normal.y != 0)
            {
                Vector2 position = transform.position;

                position.y = (PlayerState == PlayerState.OnGround
                                 ? other.collider.bounds.max
                                 : other.collider.bounds.min).y +
                             BoxCollider.bounds.extents.y * normal.y;

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

                PlayerState = PlayerState.OnEdge;
                transform.SetParent(other.transform);

                Bounds stageBounds = other.collider.bounds;

                Vector2 edgePosition;
                edgePosition.x = (normal.x > 0 ? stageBounds.max.x : stageBounds.min.x) +
                                 BoxCollider.bounds.extents.x * normal.x;
                edgePosition.y = stageBounds.max.y - BoxCollider.bounds.extents.y;

                transform.position = edgePosition;
            }
        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.gameObject.layer == stateLayer)
        {
            PlayerState = PlayerState.InAir;
            transform.SetParent(null);
        }
    }

    public void FixedUpdate()
    {
        velocity *= airResistance;

        if (PlayerState == PlayerState.InAir)
            velocity.y += gravity;
        
        transform.Translate(velocity);

        CheckWindowBorders();
    }

    public void Move(Direction direction)
    {
        if (PlayerState != PlayerState.OnEdge && AttackManager.AttackState != AttackState.Attacking)
            switch (direction)
            {
                case Direction.Left:
                    velocity += Vector2.left * mvSpeed;
                    
                    if (PlayerState == PlayerState.OnGround)
                        Facing = Vector2.left;
                    break;
                case Direction.Right:
                    velocity += Vector2.right * mvSpeed;
                    if (PlayerState == PlayerState.OnGround)
                        Facing = Vector2.right;
                    break;
                case Direction.Up:

                    break;
                case Direction.Down:
                    break;
            }
    }

    public void StartMovement(Direction direction)
    {
        if (PlayerState != PlayerState.OnEdge && AttackManager.AttackState != AttackState.Attacking)
            switch (direction)
            {
                case Direction.Left:
                    velocity += Vector2.left * (mvSpeed * 10);
                    if (PlayerState == PlayerState.OnGround)
                        Facing = Vector2.left;
                    break;
                case Direction.Right:
                    velocity += Vector2.right * (mvSpeed * 10);
                    if (PlayerState == PlayerState.OnGround)
                        Facing = Vector2.right;
                    break;
                
                case Direction.Up:
                    //GameObject.Find("Stage").transform.Translate(Vector3.up * 0.1f);
                    break;
                case Direction.Down:
                    velocity += Vector2.down * (mvSpeed * 10);
                    break;
            }
    }

    private float lastJump;

    public void Jump()
    {
        if (Time.time - lastJump >= jumpCooldown && AttackManager.AttackState != AttackState.Attacking)
        {
            velocity.y = 1;
            lastJump = Time.time;
        }
    }

    private GameObject[] dummies = new GameObject[2];

    void CheckWindowBorders()
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
            dummyPositon[axis] = transform.position[axis] + (transform.position[axis] > 0 ? -1 : 1) * windowSizeInWorld[axis];
            dummyPositon[(axis + 1) % 2] = transform.position[(axis + 1) % 2];
            dummies[axis].transform.position = dummyPositon;
        }
    }
}