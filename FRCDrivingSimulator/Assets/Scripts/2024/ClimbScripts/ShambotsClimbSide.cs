using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShambotsClimbSide : MonoBehaviour
{
    public Vector3 target;
    public GameObject[] stages;
    public float extendSpeed = 0.02f;

    public bool atTarget = false;

    public Vector3 upDirection;

    // Start is called before the first frame update
    void Start()
    {
        upDirection = stages[0].transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (stages.Length == 0)
        {
            Debug.LogWarning("Stages array is empty.");
            return;
        }

        for (int i=0; i<stages.Length; i++)
        {
            stages[i].transform.localPosition = Vector3.MoveTowards(stages[i].transform.localPosition, (i+1)* target/(stages.Length), extendSpeed * Time.deltaTime);
        }

        atTarget = Vector3.Distance(stages[stages.Length - 1].transform.localPosition, target) < 0.01;
    }

    private void Reset()
    {
        foreach (GameObject stage in stages)
        {
            stage.transform.localPosition = Vector3.zero;
        }
    }
}
