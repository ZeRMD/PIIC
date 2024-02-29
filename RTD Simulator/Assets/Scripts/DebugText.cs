using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{

    [SerializeField]
    public TMP_Text portIText;

    [SerializeField]
    public TMP_Text portQText;


    private BitArray portI;
    private BitArray portQ;

    private void Start()
    {
        portI = PortManager.portI;
        portQ=PortManager.portQ;
    }
    
}
