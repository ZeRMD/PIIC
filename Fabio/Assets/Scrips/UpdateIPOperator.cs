using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UpdateIPOperator : MonoBehaviour
{

    private string ipMudar;
    private string ipAgentes;

    private bool enviar = false;

    private IEnumerator SendPostUpdateTabletIP()
    {
        WWWForm form = new WWWForm();
        string url = "http://"+ ipAgentes + ":8080/updateTabletIP";

        string body = ipMudar;

        // Create a UnityWebRequest with the POST method
        UnityWebRequest request = UnityWebRequest.Post(url, form);

        // Set the request body with your JSON data
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(body);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        // Set the content type header
        request.SetRequestHeader("Content-Type", "text/plain");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Post has been sent");
            Debug.Log("Request successful!");
            Debug.Log("Response: " + request.downloadHandler.text);
            enviar = false;
            StopAllCoroutines();
        }
        else
        {
            enviar = true;
        }
    }

    public void UpdateTabletIP()
    {
        enviar = true;
    }

    private void Update()
    {
        if (enviar)
        {
        StartCoroutine(SendPostUpdateTabletIP());
        }
    }
}
