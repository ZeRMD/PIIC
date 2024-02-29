using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductManager : MonoBehaviour
{
    [SerializeField]
    private Transform productSpawnLocation;

    [SerializeField]
    private GameObject productPrefab;

    private SwapCollider swapCollider;
    private void Start()
    {
        swapCollider = FindObjectOfType<SwapCollider>();
    }

    public void SpawnProduct()
    {
        Instantiate(productPrefab, productSpawnLocation);
    }
    public void SwapProduct()
    {
        GameObject toDestroy = swapCollider.GetProductToSwap();

        if (toDestroy == null)
            return;

        Destroy(toDestroy);
        Instantiate(productPrefab, productSpawnLocation);
    }
}
