using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    private const string SensorActivatorTag = "SensorActivator";
    private const string Product = "Product";
    public bool activeHigh;
    public int sensorNumber;
    private PortManager portHandlerScript;

    private bool collisionCheck = false;
    private void Start()
    {
        portHandlerScript = FindObjectOfType<PortManager>();
        portHandlerScript.ChangePortValue('I', sensorNumber,!activeHigh);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(SensorActivatorTag) || other.CompareTag(Product))
        {
            collisionCheck = true;
            portHandlerScript.ChangePortValue('I', sensorNumber, activeHigh);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(SensorActivatorTag) || other.CompareTag(Product))
        {
            portHandlerScript.ChangePortValue('I', sensorNumber, !activeHigh);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!collisionCheck)
        {
            if (other.CompareTag(SensorActivatorTag))
            {
                portHandlerScript.ChangePortValue('I', sensorNumber, activeHigh);
            }
            else
            {
                portHandlerScript.ChangePortValue('I', sensorNumber, !activeHigh);
            }
            collisionCheck = true;
        }
    }
}
