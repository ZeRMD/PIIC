using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PortManager : MonoBehaviour
{
    private const int ISlots = 9;
    private const int QSlots = 10;

    public static BitArray portI;
    public static BitArray portQ;

    [SerializeField]
    public TMP_Text portIText;

    [SerializeField]
    public TMP_Text portQText;

    private MovementManager movementManager;

    private void Awake()
    {
        portI = new BitArray(ISlots, false);

        portQ = new BitArray(QSlots, false);

        movementManager = GetComponent<MovementManager>();
    }
    public void ChangePortValue(char portLetter, int portNumber, bool value)
    {
        //Na documentação não há 0, começa no 1
        Debug.LogWarning(portLetter + "_" + portNumber + " changed to " + value);
        portNumber--;
        if (portLetter == 'I')
        {
            portI.Set(portNumber, value);
            UpdatePortUI(portI, portIText);
        }
        else if (portLetter == 'Q')
        {
            portQ.Set(portNumber, value);
            UpdatePortUI(portQ, portQText);

            movementManager.OnPortValueChanged(portLetter, portNumber, value);
        }
    }
    public bool GetPortValue(char portLetter, int portNumber)
    {
        //Na documentação não há 0, começa no 1
        portNumber--;
        if (portLetter == 'I')
        {
            return portI[portNumber];
        }
        else if (portLetter == 'Q')
        {
            return portQ[portNumber];
        }
        Debug.LogError("Erro a mandar info para o C");
        return false;
    }
    private void UpdatePortUI(BitArray bitArray, TMP_Text textToUpdate)
    {
        string portText = "";

        for (int j = 0; j < bitArray.Length; j++)
        {
            string booleanText = bitArray[j] ? "1" : "0";
            portText += booleanText;
        }
        portText += "\n";
        textToUpdate.text = portText;

    }
}