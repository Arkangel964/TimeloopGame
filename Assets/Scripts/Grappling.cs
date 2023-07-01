using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    public GameObject player;
    public GameObject grappleTarget;
    public bool isGrappling;
    // Start is called before the first frame update
    void Start()
    {
        grappleTarget = null;
    }

    public void Reset()
    {
        isGrappling = false;
        player.GetComponent<DistanceJoint2D>().enabled = false;
        player.GetComponent<LineRenderer>().enabled = false;
        if (grappleTarget)
        {
            grappleTarget.GetComponent<SpriteRenderer>().color = Color.white;
        }
        player.GetComponent<Player>().canMove = true;
        grappleTarget = null;
        player.GetComponent<Player>().canJump = false;
        player.GetComponent<Rigidbody2D>().gravityScale = 9;
    }

    // Update is called once per frame
    void Update()
    {
        if(grappleTarget != null)
        {
            player.GetComponent<LineRenderer>().SetPosition(0, player.transform.position);
            player.GetComponent<LineRenderer>().SetPosition(1, grappleTarget.transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && grappleTarget != null)
        {
            isGrappling = true;
            player.GetComponent<DistanceJoint2D>().enabled = true;
            player.GetComponent<DistanceJoint2D>().connectedBody = grappleTarget.GetComponent<Rigidbody2D>();
            player.GetComponent<LineRenderer>().enabled = true;
            player.GetComponent<Player>().canMove = false;
        }
        if (isGrappling && (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Space)))
        {
            isGrappling = false;
            player.GetComponent<DistanceJoint2D>().enabled = false;
            player.GetComponent<LineRenderer>().enabled = false;
            if (grappleTarget)
            {
                grappleTarget.GetComponent<SpriteRenderer>().color = Color.white;
            }
            player.GetComponent<Player>().canMove = true;
            grappleTarget = null;
            player.GetComponent<Player>().canJump = false;
            player.GetComponent<Rigidbody2D>().gravityScale = 9;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.GetComponent<Rigidbody2D>().AddForce(Vector3.up * player.GetComponent<Player>().jumpforce);
            }
        }
        player.GetComponent<Animator>().SetBool("isGrappling", isGrappling);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Grapple")
        {
            grappleTarget = collision.gameObject;
            grappleTarget.GetComponent<SpriteRenderer>().color = Color.magenta;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isGrappling && collision.gameObject.tag == "Grapple")
        {
            grappleTarget.GetComponent<SpriteRenderer>().color = Color.white;
            grappleTarget = null;
        }
    }

}
