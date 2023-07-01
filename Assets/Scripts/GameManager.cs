using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float maxTime;
    public float currentTime;
    public float timeToAdd;
    public List<GameObject> resettableObjects;
    public GameObject player;
    public Animator transitionAnimator;
    // Start is called before the first frame update
    void Start()
    {
        resettableObjects = new List<GameObject>();
        currentTime = maxTime;
        resettableObjects.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        resettableObjects.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        resettableObjects.AddRange(GameObject.FindGameObjectsWithTag("GrappleArea"));
        StartCoroutine(timer());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentTime = 0;
        }
    }

    private IEnumerator timer()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        transitionAnimator.SetBool("Transition", true);
        yield return new WaitForSeconds(0.5f);
        resettableObjects.ForEach(delegate (GameObject obj)
        {
            obj.SendMessage("Reset");
        });
        transitionAnimator.SetBool("Transition", false);
        yield return new WaitForSecondsRealtime(0.5f);
        currentTime = maxTime;
        player.GetComponent<Player>().canMove = true;
        StartCoroutine(timer());
    }
}
