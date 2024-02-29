using UnityEngine;

public class CairdoBloco : MonoBehaviour
{
    [SerializeField]
    private GameObject peca;
    private Rigidbody rb;
    private Transform t;
    private MeshRenderer mr;

    private bool cair = false;
    float smoothVal = 0.4f; // Higher = 'Smoother'  
    private Vector3 posInicial;
    Vector3 refVel = Vector3.zero;

    public Material blue;
    public Material green;
    public Material yellow;
    public Material red;

    private void Start()
    {
        peca.SetActive(false);
        rb = peca.GetComponent<Rigidbody>();
        t = peca.transform;
        mr = peca.GetComponent<MeshRenderer>();
        posInicial = t.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cair)
        {
            if(t.position.y < posInicial.y+0.1) {
                t.position = new Vector3 (posInicial.x,posInicial.y +0.7f,posInicial.z);
            }
            t.position = Vector3.SmoothDamp(t.position, posInicial, ref refVel, smoothVal, 0.8f); 
        } else
        {
            rb.velocity = Vector3.zero;
            t.position = posInicial;
        }
    }

    public void StartCair()
    {
        cair = true;
        peca.SetActive(true);
        t.position = new Vector3(posInicial.x, posInicial.y + 0.7f, posInicial.z);
    }
    
    public void StopCair()
    {
        cair = false;
        peca.SetActive(false);
    }

    public void AtualizaCor(string newColor)
    {
        peca.SetActive (true);
        Material matTroca = null;

        switch(newColor)
        {
            case "Blue":
                matTroca = blue;
                break;
            case "Yellow":
                matTroca = yellow;
                break;
            case "Red":
                matTroca = red;
                break;
            case "Green":
                matTroca = green;
                break;
        }

        mr.material = matTroca;
    }
}
