using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwapCollider : MonoBehaviour
{
    private const string Product = "Product";

    List<GameObject> productsToSwap = new List<GameObject>();


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Product))
            productsToSwap.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Product))
        {
            productsToSwap.Remove(other.gameObject);
        }
    }

    public GameObject GetProductToSwap()
    {
        return productsToSwap.First();
    }
}
