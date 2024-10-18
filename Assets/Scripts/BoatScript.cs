using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatScript : MonoBehaviour
{
    [SerializeField] private GameObject[] containers;
    private int index = 0;
    public bool allDocked = false;
    private Vector3 finishPos;

    public Vector3 startPosition;


    void Start()
    {
        startPosition = gameObject.transform.position;
    }

    void Update()
    {
        if (allDocked)
        {
            Destroy(gameObject, 0.1f);
        }
    }

    public void ShowContainer()
    {
        if (index > containers.Length)
        {
            return;
        }
        containers[index].gameObject.SetActive(true);
        if (index == containers.Length - 1)
        {
            allDocked = true;
        }
        index++;
    }

    public void MoveToDock(Vector3 finishPosition)
    {
        allDocked = true;
        finishPos = finishPosition;
    }
}
