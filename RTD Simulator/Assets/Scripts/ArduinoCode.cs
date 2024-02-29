using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoCode : MonoBehaviour
{
    public BitArray portI;
    public BitArray portQ;

    [SerializeField]
    private bool debugMode;

    [SerializeField]
    private Transform productSpawnLocation;

    [SerializeField]
    private GameObject productPrefab;

    private PortManager portManager;

    void Start()
    {
        portI = PortManager.portI; 
        portQ = PortManager.portQ;

        portManager = FindObjectOfType<PortManager>();
    }

    IEnumerator StartArduinoCode()
    {
        yield return new WaitUntil(() => portI[6]);

        portManager.ChangePortValue('Q',5,true);

        yield return new WaitUntil(() => portI[4]);

        yield return new WaitForSeconds(1);

        portManager.ChangePortValue('Q', 5, false);
        portManager.ChangePortValue('Q', 1, true);

        yield return new WaitUntil(() => portI[0]);

        portManager.ChangePortValue('Q', 1, false);
        portManager.ChangePortValue('Q', 2, true);

        yield return new WaitUntil(() => portI[1]);
        yield return new WaitUntil(() => !portI[1]);
        
        portManager.ChangePortValue('Q', 2, false);
        portManager.ChangePortValue('Q', 6, true);

        yield return new WaitUntil(() => portI[5]);

        portManager.ChangePortValue('Q', 6, false);
        portManager.ChangePortValue('Q', 7, true);

        yield return new WaitForSeconds(3);

        portManager.ChangePortValue('Q', 6, true);
        portManager.ChangePortValue('Q', 7, false);
        portManager.ChangePortValue('Q', 8, true);

        yield return new WaitUntil(() => portI[7]);

        portManager.ChangePortValue('Q', 6, false);
        portManager.ChangePortValue('Q', 8, false);
        portManager.ChangePortValue('Q', 9, true);

        yield return new WaitForSeconds(3);

        portManager.ChangePortValue('Q', 8, true);
        portManager.ChangePortValue('Q', 9, false);

        yield return new WaitForSeconds(1);

        portManager.ChangePortValue('Q', 8, false);
        portManager.ChangePortValue('Q', 3, true);

        yield return new WaitUntil(() => portI[2]);

        portManager.ChangePortValue('Q', 3, false);
        portManager.ChangePortValue('Q', 4, true);

        yield return new WaitUntil(() => portI[3]);
        yield return new WaitUntil(() => !portI[3]);

        portManager.ChangePortValue('Q', 4, false);
        portManager.ChangePortValue('Q', 10, true);

        yield return new WaitUntil(() => portI[8]);

        yield return new WaitForSeconds(1);

        portManager.ChangePortValue('Q', 10, false);
    }
    public void StartParallel()
    {
        if (!debugMode)
            return;
        Instantiate(productPrefab, productSpawnLocation);
        StartCoroutine(StartArduinoCode());
    }
}
