using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float maxTime;
    public float currentTime;
    public float timeToAdd;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI addedTimeText;
    public List<GameObject> resettableObjects;
    public GameObject player;
    public Animator transitionAnimator;
    // Start is called before the first frame update
    void Start()
    {
        timeToAdd = 0;
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
        timerText.text = TimeSpan.FromSeconds(currentTime + 1).ToString(@"mm\:ss");
        if (timeToAdd > 0)
        {
            addedTimeText.text = "+" + (int)Math.Floor(timeToAdd);
        }
        else
        {
            addedTimeText.text = "";
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
        currentTime = maxTime;
        timeToAdd = 0;
        yield return new WaitForSecondsRealtime(0.5f);
        player.GetComponent<Player>().canMove = true;
        StartCoroutine(timer());
    }
}
