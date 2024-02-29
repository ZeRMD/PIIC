using System.Collections;
using UnityEngine;

public class MovementManager : MonoBehaviour
{

    private BitArray portQ;
    private Mechanism mechanism;

    private void Awake()
    {
        portQ = PortManager.portQ;
        mechanism = GetComponent<Mechanism>();
    }
    public void OnPortValueChanged(char portLetter, int portNumber, bool value)
    {
        if (portLetter == 'Q')
        {
            switch (portNumber)
            {
                case 0:
                    //Move slider 1 forward
                    if (value)
                    {
                        mechanism.MoveSlider1(true);
                    }
                    else
                    {
                        mechanism.StopSlider1();
                    }
                    break;
                case 1:
                    //Move slider 1 backward
                    if (value)
                    {
                        mechanism.MoveSlider1(false);
                    }
                    else
                    {
                        mechanism.StopSlider1();
                    }
                    break;
                case 2:
                    //Move slider 2 forward
                    if (value)
                    {
                        mechanism.MoveSlider2(true);
                    }
                    else
                    {
                        mechanism.StopSlider2();
                    }
                    break;
                case 3:
                    //Move slider 2 backward
                    if (value)
                    {
                        mechanism.MoveSlider2(false);
                    }
                    else
                    {
                        mechanism.StopSlider2();
                    }
                    break;
                case 4:
                    //Move conveyor belt feed
                    if (value)
                    {
                        mechanism.MoveConveyorFeed();
                    }
                    else
                    {
                        mechanism.StopConveyorFeed();
                    }
                    break;
                case 5:
                    //Move conveyor belt milling
                    if (value)
                    {
                        mechanism.MoveConveyorMilling();
                    }
                    else
                    {
                        mechanism.StopConveyorMilling();
                    }
                    break;
                case 6:
                    //Rotate milling machine
                    if (value)
                    {
                        mechanism.RotateMilling();
                    }
                    else
                    {
                        mechanism.StopMilling();
                    }
                    break;
                case 7:
                    //Move conveyor belt drilling
                    if (value)
                    {
                        mechanism.MoveConveyorDrilling();
                    }
                    else
                    {
                        mechanism.StopConveyorDrilling();
                    }
                    break;
                case 8:
                    //Rotate drilling machine
                    if (value)
                    {
                        mechanism.RotateDrilling();
                    }
                    else
                    {
                        mechanism.StopDrilling();
                    }
                    break;
                case 9:
                    //Move conveyor belt swap
                    if (value)
                    {
                        mechanism.MoveConveyorSwap();
                    }
                    else
                    {
                        mechanism.StopConveyorSwap();
                    }
                    break;
                default:
                    PrintErrorMessage(portNumber);
                    break;
            }
        }
    }
    private void PrintErrorMessage(int portNumber)
    {
        Debug.LogWarning("Port" + portNumber + " changed not referenced to any movement");
    }
}
