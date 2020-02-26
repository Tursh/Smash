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

public enum PlayerSubState
{
    Idle,
    Attacking,
    Moving
}

public class OnGroundEnventArgs
{
}

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] private float mvSpeed = 0.025f, airResistance = 0.9f, gravity = -0.05f, jumpCooldown = 0.25f;
    [SerializeField] private GameObject dummyPrefab;
    [SerializeField] private PlayerState _playerState;
    [SerializeField] private PlayerSubState _playerSubState;

    public Vector2 Facing = Vector2.left;
    public Vector2 velocity;
    private Vector2 startWindowPosition, endWindowPosition, windowSizeInWorld;

    BoxCollider2D BoxCollider;
    public PlayerInfo playerInfo;
    public PlayerState PlayerState { get; private set; } = PlayerState.InAir;
    public PlayerSubState PlayerSubState { get; set; } = PlayerSubState.Idle;

    private int playerLayer = 9, stageLayer = 8, semiPlatformLayer = 12;

    private void Start()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
        playerInfo = GetComponent<PlayerInfo>();


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

        Vector3 normal = other.contacts[0].normal;

        if (other.collider.gameObject.layer == stageLayer)
        {
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
                position.y =
                    (PlayerState == PlayerState.OnGround ? other.collider.bounds.max : other.collider.bounds.min).y
                    + BoxCollider.bounds.extents.y * normal.y;
                transform.position = position;

                velocity.y = 0;
            }
            else
            {
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
        else if (other.gameObject.layer == semiPlatformLayer && normal == new Vector3(0, 1, 0))
        {
            if (PlayerState != PlayerState.OnGround)
            {
                Debug.Log("OnGround");
                //Set the player on ground
                PlayerState = PlayerState.OnGround;
                transform.SetParent(other.transform);
                OnGroundEventHandler?.Invoke(this, new OnGroundEnventArgs());

                //Set player to the top of the platform
                Vector2 position = transform.position;
                position.y = other.collider.bounds.max.y + BoxCollider.bounds.extents.y;
                transform.position = position;

                //Remove all vertical velocity
                velocity.y = 0;
            }
            else if(transform.parent != other.transform)
            {
                transform.SetParent(other.transform);
                Vector2 position = transform.position;
                position.y = other.collider.bounds.max.y + BoxCollider.bounds.extents.y;
                transform.position = position;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform == transform.parent)
        {
            PlayerState = PlayerState.InAir;
            transform.SetParent(null);
        }
    }

    public void FixedUpdate()
    {
        _playerState = PlayerState;
        _playerSubState = PlayerSubState;

        velocity *= airResistance;

        if (PlayerState == PlayerState.InAir)
            velocity.y += gravity;

        transform.Translate(velocity);

        CheckWindowBorders();
    }

    public void Move(Direction direction)
    {
        if (PlayerState != PlayerState.OnEdge && PlayerSubState != PlayerSubState.Attacking)
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
        if (PlayerState != PlayerState.OnEdge && PlayerSubState != PlayerSubState.Attacking)
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
        if (Time.time - lastJump >= jumpCooldown && PlayerSubState != PlayerSubState.Attacking)
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
            dummyPositon[axis] = transform.position[axis] +
                                 (transform.position[axis] > 0 ? -1 : 1) * windowSizeInWorld[axis];
            dummyPositon[(axis + 1) % 2] = transform.position[(axis + 1) % 2];
            dummies[axis].transform.position = dummyPositon;
        }
    }
}