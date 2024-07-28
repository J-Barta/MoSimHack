using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShambotsClimb : MonoBehaviour {

    [SerializeField] private ShambotsClimbSide[] climbers;
    [SerializeField] private float extendAmount;

    public float extendSpeed = 50f;


    private bool climb;
    private bool prevClimb = false;
    private bool isClimbing = false;
    private bool prepped = false;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (climbers[0].atTarget && isClimbing)
        {
            prepped = true;
        }

        bool climbFreshPress = climb && !prevClimb;
        if(climbFreshPress && !isClimbing)
        {
            isClimbing = true;
            foreach (ShambotsClimbSide climber in climbers)
            {
                climber.target = climber.upDirection * extendAmount;
            }

        }
        else if (climbFreshPress && prepped)
        {
            prepped = false;
            isClimbing = false;
            foreach (ShambotsClimbSide climber in climbers)
            {
                climber.target = Vector3.zero;
            }
        }

        prevClimb = climb;
    }

    private IEnumerator ClimbSequence(GameObject[] climber)
    {
        Debug.Log("Extend");
        Vector3 targetPosition = climber[0].transform.up * extendAmount;

        while (Vector3.Distance(climber[climber.Length-1].transform.localPosition, targetPosition) > 0.001)
        {
            for (int stage = 0; stage < climber.Length; stage++)
            {
                climber[stage].transform.localPosition = Vector3.MoveTowards(climber[stage].transform.localPosition, targetPosition / climber.Length, extendSpeed * Time.deltaTime);

            }

            yield return null;
        }

        prepped = true;

        //Debug.Log(climber.transform.localPosition);

        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator HangSequence(GameObject[] climber)
    {
        Debug.Log("Retract");
        //Debug.Log(Vector3.Distance(climberStartingPos, climber.transform.localPosition));
        Vector3 targetPosition = new Vector3(0,0,0);

        while (Vector3.Distance(climber[climber.Length - 1].transform.localPosition, targetPosition) > 0.001)
        {
            for (int stage = 0; stage < climber.Length; stage++)
            {
                climber[stage].transform.localPosition = Vector3.MoveTowards(climber[stage].transform.localPosition, targetPosition / climber.Length, extendSpeed * Time.deltaTime);

            }

            yield return null;
        }

        isClimbing = false;
        prepped = false;

        yield return new WaitForSeconds(0.5f);
    }

    public void OnClimb(InputAction.CallbackContext ctx)
    {
        climb = ctx.action.triggered;
    }
}
