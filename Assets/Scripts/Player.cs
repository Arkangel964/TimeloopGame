using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera mainCamera;
    public LineRenderer lineRenderer;
    public DistanceJoint2D distanceJoint2D;
    public GameObject grappleArea;
    public Vector3 startPos;
    public float speed;
    public float maxSpeed;
    public float acceleration;
    public float grappleDistanceX;
    public float grappleDistanceY;
    public float jumpforce;
    public float dashforce;
    public bool canJump;
    public bool canMove;
    public bool canDash;
    public enum facingDirection { left, right };
    public facingDirection facing;
    public float upwardsVelocity;
    public float forwardsVelocity;
    public float dashAirTime;
    public float horizontalSpeedCap;
    public float verticalSpeedCap;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        canJump = true;
        canDash = true;
        canMove = true;
        distanceJoint2D.enabled = false;
        facing = facingDirection.right;
    }

    private void Reset()
    {
        transform.position = startPos;
        canJump = true;
        canDash = true;
        canMove = true;
        distanceJoint2D.enabled = false;
        facing = facingDirection.right;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            //TODO make it so that the player doesn't hit max speed immediately
            if (Input.GetKey(KeyCode.A))
            {
                facing = facingDirection.left;
                if (speed - acceleration >= -maxSpeed)
                {
                    speed -= acceleration;
                }
                grappleArea.transform.position = new Vector3(transform.position.x - grappleDistanceX, transform.position.y + grappleDistanceY, transform.position.z);
            }
            if (Input.GetKey(KeyCode.D))
            {
                facing = facingDirection.right;
                if (speed + acceleration <= maxSpeed)
                {
                    speed += acceleration;
                }
                grappleArea.transform.position = new Vector3(transform.position.x + grappleDistanceX, transform.position.y + grappleDistanceY, transform.position.z);
            }
            if (Input.GetKey(KeyCode.A) == Input.GetKey(KeyCode.D))
            {
                if (speed > 0)
                {
                    speed -= 2 * acceleration;
                }
                if (speed < 0)
                {
                    speed += 2 * acceleration;
                }
                if (Math.Abs(speed) < 2 * acceleration)
                {
                    speed = 0;
                }
            }
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        
        
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            canJump = false;
            canDash = false;
            GetComponent<Rigidbody2D>().gravityScale = 9;
            if (upwardsVelocity < 10)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpforce);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            canDash = false;
            Debug.Log(Math.Abs(GetComponent<Rigidbody2D>().velocity.x));
            if (Math.Abs(GetComponent<Rigidbody2D>().velocity.x) < 10)
            {
                if (facing == facingDirection.left)
                {
                    GetComponent<Rigidbody2D>().AddForce(Vector3.left * dashforce);
                }
                else
                {
                    GetComponent<Rigidbody2D>().AddForce(Vector3.right * dashforce);
                }
            }
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
            GetComponent<Rigidbody2D>().gravityScale = 0;
            StartCoroutine(Dash());
        }
        upwardsVelocity = GetComponent<Rigidbody2D>().velocity.y;
        forwardsVelocity = GetComponent<Rigidbody2D>().velocity.x;
        
        GetComponent<Rigidbody2D>().velocity = new Vector2(forwardsVelocity, upwardsVelocity);
    }
    

    private IEnumerator Dash()
    {
        float currentAirTime = 0f;
        while (currentAirTime < dashAirTime)
        {
            currentAirTime += Time.deltaTime;
            yield return null;
        }
        GetComponent<Rigidbody2D>().gravityScale = 9;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canJump = true;
            
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            canDash = true;
        }
    }
}
