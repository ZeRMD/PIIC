using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanism : MonoBehaviour
{
    private const float slider1Speed = 1;
    private const float slider2Speed = 1;
    private const float conveyorAnimationSpeed = .3f;
    private const float millingRotatingVelocity = 20;
    private const float drillingVerticalRotatingVelocity = 20;
    private const float drillingHorizontalRotatingVelocity = 20;
    public static float boxSpeed = 3;

    [SerializeField]
    Rigidbody slider1;

    [SerializeField]
    Rigidbody slider2;

    [SerializeField]
    Conveyor[] conveyors; //Feed, Milling, Drilling, Swap

    [SerializeField]
    Rigidbody millingMachine;

    [SerializeField]
    Rigidbody drillingVerticalMachine;

    [SerializeField]
    Rigidbody drillingHorizontalMachine;

    [SerializeField]
    BoxMovement boxMovement;

    private void Start()
    {
        millingMachine.maxAngularVelocity = millingRotatingVelocity;
        drillingVerticalMachine.maxAngularVelocity = drillingVerticalRotatingVelocity;
        drillingHorizontalMachine.maxAngularVelocity = drillingHorizontalRotatingVelocity;
    }

    public void MoveSlider1(bool forward)
    {
        Vector3 direction;
        if (forward)
        {
            direction = Vector3.right;
        }
        else
        {
            direction = Vector3.left;
        }
        slider1.velocity = direction * slider1Speed;
    }
    public void StopSlider1()
    {
        slider1.velocity = Vector3.zero;
    }

    public void MoveSlider2(bool forward)
    {
        Vector3 direction;
        if (forward)
        {
            direction = Vector3.back;
        }
        else
        {
            direction = Vector3.forward;
        }
        slider2.velocity = direction * slider2Speed;
    }
    public void StopSlider2()
    {
        slider2.velocity = Vector3.zero;
    }

    public void MoveConveyorFeed()
    {
        conveyors[0].ChangeAnimationSpeed(conveyorAnimationSpeed);
        //boxMovement.ChangeBoxSpeed(boxSpeed);
    }
    public void StopConveyorFeed()
    {
        conveyors[0].ChangeAnimationSpeed(0);
        //boxMovement.ChangeBoxSpeed(0);
    }

    public void MoveConveyorMilling()
    {
        conveyors[1].ChangeAnimationSpeed(conveyorAnimationSpeed);
        //boxMovement.ChangeBoxSpeed(boxSpeed);
    }
    public void StopConveyorMilling()
    {
        conveyors[1].ChangeAnimationSpeed(0);
        //boxMovement.ChangeBoxSpeed(0);
    }

    public void MoveConveyorDrilling()
    {
        conveyors[2].ChangeAnimationSpeed(conveyorAnimationSpeed);
        //boxMovement.ChangeBoxSpeed(boxSpeed);
    }
    public void StopConveyorDrilling()
    {
        conveyors[2].ChangeAnimationSpeed(0);
       //boxMovement.ChangeBoxSpeed(0);
    }

    public void MoveConveyorSwap()
    {
        conveyors[3].ChangeAnimationSpeed(conveyorAnimationSpeed);
        //boxMovement.ChangeBoxSpeed(boxSpeed);
    }
    public void StopConveyorSwap()
    {
        conveyors[3].ChangeAnimationSpeed(0);
        //boxMovement.ChangeBoxSpeed(0);
    }

    public void RotateMilling()
    {
        millingMachine.angularVelocity = millingRotatingVelocity * Vector3.up;
    }
    public void StopMilling()
    {
        millingMachine.angularVelocity = Vector3.zero;
    }

    public void RotateDrilling()
    {
        drillingVerticalMachine.angularVelocity = drillingVerticalRotatingVelocity * Vector3.down;
        drillingHorizontalMachine.angularVelocity = drillingHorizontalRotatingVelocity * Vector3.back;
    }
    public void StopDrilling()
    {
        drillingVerticalMachine.angularVelocity = Vector3.zero;
        drillingHorizontalMachine.angularVelocity = Vector3.zero;
    }
}
