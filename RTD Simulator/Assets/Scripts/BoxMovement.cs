using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovement : MonoBehaviour
{
    private Rigidbody boxRB;

    private Vector3 direction;

    private int beltsInContact = 0;
    private float boxSpeed = 0;

    private void Start()
    {
        boxRB = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        //Debug.Log(beltsInContact);
        if (beltsInContact <= 0)
        {
            return;
        }
        else
        {
            boxRB.velocity = direction * boxSpeed;
        }        
    }

    public void ChangeBoxSpeed(float newSpeed)
    {
        boxSpeed = newSpeed;
    }

    public void IncrementConveyorNumber(Vector3 newDirection)
    {
        beltsInContact++;
        direction = newDirection;
    }
    public void DecrementConveyorNumber()
    {
        beltsInContact--;
    }
}
