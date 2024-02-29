using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ConfigUpdate : MonoBehaviour
{
    private string yellow1 = "Skill_Yellow_1";
    private string yellow2 = "Skill_Yellow_2";
    private string green1 = "Skill_Green_1";
    private string green2 = "Skill_Green_2";
    private string red1 = "Skill_Red_1";
    private string red2 = "Skill_Red_2";
    private string blue1 = "Skill_Blue_1";
    private string blue2 = "Skill_Blue_2";
    private string skillDrill = "Skill_Drill";
    private string skillScrew = "Skill_Screw";
    private string operador = "Operator1";
    private string robo = "";

    private bool station = true;
    private bool yOP = false;
    private bool yRB = false;
    private bool rOP = true;
    private bool rRB = false;
    private bool gOP = false;
    private bool gRB = false;
    private bool bOP = true;
    private bool bRB = false;
    private bool sDrill = true;
    private bool sScrew = true;

    bool atualiza = false;

    private IEnumerator SendPostUpdateConfiguration()
    {
        WWWForm form = new WWWForm();

        string url = "http://192.168.2.104:8080/updateConfigurations";

        // construcao da string
        string jsonBody = JsonBodyStringBuilder();

        // Create a UnityWebRequest with the POST method
        UnityWebRequest request = UnityWebRequest.Post(url, form);

        // Set the request body with your JSON data
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

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

    string JsonBodyStringBuilder()
    {
        string jsonBody;
        JObject total = new JObject();

        if (station)
        {
            JObject resources = new JObject();

            JArray torre = new JArray();

            if (sScrew)
            {
                torre.Add(skillScrew);
            }
            if (sDrill)
            {
                torre.Add(skillDrill);
            }

            total["Station"] = torre;
        }

        if (this.operador != "")
        {
            JArray operador = new JArray();
            
            if (rOP)
            {
                operador.Add(red1);
                operador.Add(red2);
            }
            if (gOP)
            {
                operador.Add(green1);
                operador.Add(green2);
            }
            if (yOP)
            {
                operador.Add(yellow1);
                operador.Add(yellow2);
            }
            if (bOP)
            {
                operador.Add(blue1);
                operador.Add(blue2);
            }
            total[this.operador] = operador;
        }


        if (robo != "")
        {
            JArray robot = new JArray();
            
            if (yRB)
            {
                robot.Add(yellow1);
                robot.Add(yellow2);
            }
            if (rRB)
            {
                robot.Add(red1);
                robot.Add(red2);
            }
            if (bRB)
            {
                robot.Add(blue1);
                robot.Add(blue2);
            }
            if (gRB)
            {
                robot.Add(green1);
                robot.Add(green2);
            }
            total[robo] = robot;
        }

        JObject Resources = new JObject();

        Resources["Resources"] = total;

        jsonBody = Resources.ToString();
        Debug.Log(jsonBody);
        return jsonBody;
    }

    void YellowOP(bool toggleON)
    {
        if (toggleON)
        {
            yOP = true;
        }
        else
        {
            yOP = false;
        }
    }
    void BlueOP(bool toggleON)
    {
        if (toggleON)
        {
            bOP = true;
        }
        else
        {
            bOP = false;
        }
    }

    void GreenOP(bool toggleON)
    {
        if (toggleON)
        {
            gOP = true;
        }
        else
        {
            gOP = false;
        }
    }

    void RedOP(bool toggleON)
    {
        if (toggleON)
        {
            rOP = true;
        }
        else
        {
            rOP = false;
        }
    }

    void YellowRB(bool toggleON)
    {
        if (toggleON)
        {
            yRB = true;
        }
        else
        {
            yRB = false;
        }
    }

    void BlueRB(bool toggleON)
    {
        if (toggleON)
        {
            bRB = true;
        }
        else
        {
            bRB = false;
        }
    }

    void GreenRB(bool toggleON)
    {
        if (toggleON)
        {
            gRB = true;
        }
        else
        {
            gRB = false;
        }
    }

    void RedRB(bool toggleON)
    {
        if (toggleON)
        {
            rRB = true;
            atualiza = true;
        }
        else
        {
            rRB = false;
            atualiza = true;
        }
    }

    public void UpdateAgr()
    {
        atualiza = true;
    }

    private void Update()
    {
        if (atualiza)
        {
            StartCoroutine(SendPostUpdateConfiguration());
            atualiza = false;
        }
    }
}
