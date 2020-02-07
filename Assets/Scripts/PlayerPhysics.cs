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

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] private float mvSpeed = 0.025f, airResistance = 0.9f, gravity = -0.1f, jumpCooldown = 0.25f;

    [SerializeField] private GameObject DummyPrefab;

    public Vector2 velocity;
    private Vector2 startWindowPosition, endWindowPosition, windowSizeInWorld;

    BoxCollider2D bc;
    Rigidbody2D rb;

    public bool onGround { get; private set; } = false;

    private int playerLayer = 9, stateLayer = 8;

    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();


        Camera cam = Camera.main;

        startWindowPosition = cam.ScreenToWorldPoint(new Vector3(0, 0, 12));
        endWindowPosition = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 12));
        windowSizeInWorld = endWindowPosition - startWindowPosition;

        Dummy =
            Instantiate(DummyPrefab,
                startWindowPosition - Vector2.down * 10,
                Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //TODO: Do damage depending on the velocity

        if (other.collider.gameObject.layer == stateLayer)
        {
            Vector3 normal = other.contacts[0].normal;

            if (normal == new Vector3(0, 1, 0))
            {
                onGround = true;
                transform.SetParent(other.transform);
            }

            if (normal.y != 0)
            {
                Vector2 position = transform.position;

                position.y = (onGround ? other.collider.bounds.max : other.collider.bounds.min).y +
                             bc.bounds.extents.y * normal.y;
                transform.position = position;
                velocity.y = 0;
            }
            else
            {
                Vector2 position = transform.position;
                position.x = (normal.x > 0 ? other.collider.bounds.max : other.collider.bounds.min).x +
                             (bc.bounds.extents.x + 0.03f) * normal.x;
                transform.position = position;
                velocity.x = 0;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.gameObject.layer == stateLayer)
        {
            onGround = false;
            transform.SetParent(null);
        }
    }

    public void FixedUpdate()
    {
        velocity *= airResistance;

        if (!onGround)
            velocity.y += gravity;

        transform.Translate(velocity);

        checkWindowBorders();
    }

    public void Move(Direction direction)
    {
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
    private GameObject Dummy;

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

                    Vector3 dummyPosition = Dummy.transform.position;
                    dummyPosition[axis] = position[axis] + windowSizeInWorld[axis];

                    if (!onBorder[(axis + 1) % 2])
                        dummyPosition[(axis + 1) % 2] = position[(axis + 1) % 2];

                    Dummy.transform.position = dummyPosition;
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

                    Vector3 dummyPosition = Dummy.transform.position;
                    dummyPosition[axis] = position[axis] - windowSizeInWorld[axis];

                    if (!onBorder[(axis + 1) % 2])
                        dummyPosition[(axis + 1) % 2] = position[(axis + 1) % 2];

                    Dummy.transform.position = dummyPosition;
                }
            }
            else
                onBorder[axis] = false;

            if (!onBorder[0] && !onBorder[1])
                Dummy.transform.position = -windowSizeInWorld;
        }
    }
}