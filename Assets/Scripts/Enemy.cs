using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float patrolDistance;
    public bool isPatrolling;
    public enum patrolDirections { left, right };
    public patrolDirections patrolDirection;
    public Vector3 startPos;
    public float speed;
    public float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        isPatrolling = true;
        patrolDirection = patrolDirections.left;
    }

    private void Reset()
    {
        transform.position = startPos;
        isPatrolling = true;
        patrolDirection = patrolDirections.left;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPatrolling)
        {
            if (patrolDirection == patrolDirections.left)
            {
                if(Vector3.Distance(transform.position + Vector3.left * Time.deltaTime * speed, startPos) >= patrolDistance)
                {
                    transform.position = startPos + Vector3.left * patrolDistance;
                } else
                {
                    transform.position += Vector3.left * Time.deltaTime * speed;
                }
            }
            else
            {
                if (Vector3.Distance(transform.position + Vector3.right * Time.deltaTime * speed, startPos) >= patrolDistance)
                {
                    transform.position = startPos + Vector3.right * patrolDistance;
                }
                else
                {
                    transform.position += Vector3.right * Time.deltaTime * speed;
                }
            }
            
            if (Vector3.Distance(transform.position, startPos) >= patrolDistance)
            {
                StartCoroutine(waitAtPatrol());
            }
        }
    }

    private IEnumerator waitAtPatrol()
    {
        isPatrolling = false;
        yield return new WaitForSeconds(waitTime);
        if (patrolDirection == patrolDirections.left)
        {
            patrolDirection = patrolDirections.right;
        }
        else
        {
            patrolDirection = patrolDirections.left;
        }
        isPatrolling = true;
    }
}
