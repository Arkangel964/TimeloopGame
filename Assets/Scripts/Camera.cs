using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public Vector3 initialOffset = new Vector3(0, 0, -10);
    public float smoothSpeed = 0.125f;
    public GameObject player;
    [SerializeField] public Transform target;
    public Vector3 cameraSpeed = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = initialOffset;
        if (Input.GetKey(KeyCode.A))
        {
            offset.x = -offset.x;
        }
        if (Input.GetKey(KeyCode.D))
        {
            offset.x = offset.x;
        }
        if (Input.GetKey(KeyCode.A) == Input.GetKey(KeyCode.D))
        {
            offset.x = 0;
        }
        if (player.GetComponent<Rigidbody2D>().velocity.y > 30)
        {
            offset.y += 10;
        }
        if (player.GetComponent<Rigidbody2D>().velocity.y < -30)
        {
            offset.y -= 10;
        }
        //TODO possibly make camera anticipate where player will be
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref cameraSpeed, smoothSpeed);
    }
}
