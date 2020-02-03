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
    [SerializeField] private float mvSpeed = 0.5f, airResistance = 0.9f, gravity = -0.1f, jumpCooldown = 0.25f;
    public Vector2 velocity;

    BoxCollider2D bc;
    Rigidbody2D rb;

    public bool onGround { get; private set; } = false;

    private int playerLayer = 9, stateLayer = 8;

    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
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
                transform.SetParent(GameObject.Find("Stage").transform);
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

    private bool first = true;

    public void FixedUpdate()
    {
        velocity *= airResistance;

        if (!onGround)
            velocity.y += gravity;

        transform.Translate(velocity);

        if (onGround && first)
        {
            first = false;
        }
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
}