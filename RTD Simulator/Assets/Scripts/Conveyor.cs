using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField]
    private Vector3 movementDirection;

    private List<Product> products = new List<Product>();

    private float animationSpeed = 0;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void FixedUpdate()
    {
        meshRenderer.material.mainTextureOffset += animationSpeed * Time.deltaTime * Vector2.left;
    }

    public void ChangeAnimationSpeed(float newAnimSpeed)
    {
        Debug.Log("Animation speed of " + gameObject.name + " to " + newAnimSpeed);
        animationSpeed = newAnimSpeed;
        if (newAnimSpeed == 0)
        {
            foreach (var product in products)
            {
                TurnOffConveyorPhysics(product);
            }
        }
        else
        {
            foreach (var product in products)
            {
                TurnOnConveyorPhysics(product);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Product"))
        {
            Product product = new Product(collision.gameObject.GetComponent<BoxMovement>(), true);

            products.Add(product);

            TurnOnConveyorPhysics(product);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Product"))
        {
            Product product = new Product(collision.gameObject.GetComponent<BoxMovement>(), true);

            products.Remove(product);

            TurnOffConveyorPhysics(product);
        }
    }

    private void TurnOnConveyorPhysics(Product product)
    {
        if (product.productColliding && animationSpeed > 0)
        {
            product.boxMovement.IncrementConveyorNumber(movementDirection);
            product.boxMovement.ChangeBoxSpeed(Mechanism.boxSpeed);
            Debug.Log("START: " + product.boxMovement.gameObject.name);
        }
    }
    private void TurnOffConveyorPhysics(Product product)
    {
        if (product.productColliding)
        {
            product.boxMovement.DecrementConveyorNumber();
            Debug.Log("STOP: " + product.boxMovement.gameObject.name);
        }
    }
    struct Product
    {
        public BoxMovement boxMovement;
        public bool productColliding;

        public Product(BoxMovement boxMovement, bool productColliding)
        {
            this.boxMovement = boxMovement;
            this.productColliding = productColliding;
        }
    }
}

