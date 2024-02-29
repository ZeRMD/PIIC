using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Launch : MonoBehaviour
{
    /*
    private string skill1 = "Red";
    private string skill2 = "Blue";
    private string action = "Screw";
    private string priority = "Human";
    */
    private bool enviarPedido = false;

    struct paraJson
    {
        public int type; //0 para action / 1 para cor pos1 / 2 para cor pos2
        public string skill;
    }

    private IEnumerator SendPostLaunch()
    {
        WWWForm form = new WWWForm();
        string url = "http://192.168.2.104:8080/launchProduct";

        // Create a UnityWebRequest with the POST method
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        /*
        // Set the request body with your JSON data
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        */
        // Set the content type header
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Post has been sent");
            Debug.Log("Request successful!");
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogWarning("Request failed. Error: " + request.error);
        }
    }

    string JsonBodyStringBuilder(paraJson[] dados, string priority)
    {
        string jsonBody;
        string paraArray;
        JObject total = new JObject();

        JArray instructions = new JArray();
        
        for (int i = 0; i < dados.Length; i++)
        {
            if (dados[i].type == 0)
            {
                paraArray = "Skill_" + dados[i].skill;
                instructions.Add(paraArray);
            }
            else
            {
                if(dados[i].type == 1)
                {
                    paraArray = "Skill_" + dados[i].skill + "_1";
                    instructions.Add(paraArray);
                }
                else // == 2
                {
                    paraArray = "Skill_" + dados[i].skill + "_2";
                    instructions.Add(paraArray);
                }
            }
        }

        total["ListOfSkills"] = instructions;
        total["Priority"] = priority;

        jsonBody = total.ToString();
        Debug.Log(jsonBody);
        return jsonBody;
    }

    public void SubmitLaunch()
    {
        StartCoroutine(SendPostLaunch());
    }
}
