using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    private ProductManager productManager;

    private void Start()
    {
        productManager = FindObjectOfType<ProductManager>();
    }
    public void Spawn()
    {
        productManager.SpawnProduct();
    }
    public void Swap()
    {
        productManager.SwapProduct();
    }
}
