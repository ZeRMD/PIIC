using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using TMPro;
using System.Linq;

public class UIButton : MonoBehaviour
{

    [SerializeField]
    private GameObject tela1;
    [SerializeField]
    private GameObject tela2;
    [SerializeField]
    private GameObject barraEscrita;
    [SerializeField]
    private GameObject buttonChangeIp;
    [SerializeField]
    private GameObject button;
    [SerializeField]
    private GameObject changeIP;
    [SerializeField]
    private TMP_Text color;
    [SerializeField]
    private TMP_Text position;
    [SerializeField]
    private TMP_Text side;
    [SerializeField]
    private TMP_Text PCIP;
    [SerializeField]
    private TMP_Text myIP;
    [SerializeField]
    private TMP_Text erroConectarMaquina;
    [SerializeField]
    private TMP_InputField ipInput;
    [SerializeField]
    private GameObject conveyorEBloco;

    private HttpListenerContext context;
    private HttpListener listener;
    private Thread listenerThread;

    private string ipMaquinaACorrer;
    private string myIPstring;
    private char pos;
    private string[] Skill;
    private bool atualizarUI = false;

    [SerializeField]
    private CairdoBloco cairdoBlocoPos1;
    [SerializeField]
    private CairdoBloco cairdoBlocoPos2;

    private CairdoBloco cairdoBlocoAtivo;

    private string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
    }

    private void Start()
    {
        ipMaquinaACorrer = PlayerPrefs.GetString("ipMaquinaACorrer", "No IP Set");
        myIPstring = GetLocalIPv4();
        PCIP.text = "PC IP: " + ipMaquinaACorrer;
        myIP.text = "My IP: " + myIPstring;
        tela1.SetActive(true);
        tela2.SetActive(false);
        erroConectarMaquina.gameObject.SetActive(false);
        conveyorEBloco.SetActive(false);
        barraEscrita.SetActive(false);
        buttonChangeIp.SetActive(true);
        StartServer();
        button.GetComponent<UnityEngine.UI.Image>().color = Color.grey;
    }

    private IEnumerator SendPostRequest(int instruction)
    {
        WWWForm form = new WWWForm();

        string url = "";
        string jsonBody = "";

        if (instruction == 1)
        {
            url = "http://" + ipMaquinaACorrer + ":8080/plugOperator?ResourceID=Operator1&Location=" + pos;
            // Replace this with your actual JSON data
            jsonBody = "{\"ResourceID\": \"Operator1\",\"Location\": \"" + pos + "\"}";
        }

        if (instruction == 2)
        {
            url = "http://" + ipMaquinaACorrer + ":8080/unplugOperator?ResourceID=Operator1&Location=" + pos;
            // Replace this with your actual JSON data
            jsonBody = "{\"ResourceID\": \"Operator1\",\"Location\": \"" + pos + "\"}";
        }

        if (instruction == 3)
        {
            url = "http://" + ipMaquinaACorrer + ":8082";
            // Replace this with your actual JSON data
            jsonBody = "{\"Position\": \""+ Skill[2] +"\",\"Color\": \"" + Skill[1] + "\"}";
        }

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
            if (instruction == 1)
            {
                erroConectarMaquina.gameObject.SetActive(false);
                tela1.SetActive(false);
                tela2.SetActive(true);
                conveyorEBloco.SetActive(true);
            }
            Debug.Log("Post has been sent");
            Debug.Log("Request successful!");
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogWarning("Request failed. Error: " + request.error);
            if (instruction == 1)
            {
                erroConectarMaquina.gameObject.SetActive(true);
            }
        }
    }
    void StartServer()
    {
        // Replace this with the port you want to listen on
        string url = "http://"+myIPstring+":3001/receive/";

        listener = new HttpListener();
        listener.Prefixes.Add(url);

        listener.Start();

        Debug.Log("Server started. Listening for incoming requests...");

        // Start listening for incoming requests on a separate thread
        listenerThread = new Thread(new ThreadStart(ListenForRequests));
        listenerThread.Start();
    }

    void OnDisable()
    {
        // Stop the listener when the script is disabled or the game is closed
        if (listener != null && listener.IsListening)
        {
            listener.Stop();
            Debug.Log("Server stopped.");
        }

        // Stop the listener thread
        if (listenerThread != null && listenerThread.IsAlive)
        {
            listenerThread.Abort();
        }
    }

    void ListenForRequests()
    {
        while (listener.IsListening)
        {
            // Wait for a request to come in
            context = listener.GetContext();

            // Process the request on the main thread
            HandleRequest(context);
        }
    }

    void EnviaResposta(string str)
    {
        string responseString = str;
        byte[] responseBytes = Encoding.UTF8.GetBytes(responseString);
        context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
        context.Response.Close();
    }

    void HandleRequest(HttpListenerContext context)
    {
        if (context.Request.HttpMethod == "POST")
        {
            // Read the request body
            using (Stream body = context.Request.InputStream)
            {
                using (StreamReader reader = new StreamReader(body, context.Request.ContentEncoding))
                {
                    string receivedJson = reader.ReadToEnd();

                    Debug.Log("Received JSON: " + receivedJson);

                    JObject json = JObject.Parse(receivedJson);

                    // Access values from the JObject directly
                    string ExecuteSkill = (string)json["ExecuteSkill"];

                    Debug.Log(ExecuteSkill);

                    Skill = ExecuteSkill.Split('_');

                    atualizarUI = true;

                }
            }
        }
        else
        {
            // Handle other HTTP methods or provide an error response
            context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            context.Response.Close();
        }
    }

    public void Menu1(bool left)
    {
        string side_ui;

        if (left)
        {
            pos = 'B';
            side_ui = "Left";
        }
        else
        {
            pos = 'F';
            side_ui = "Right";
        }

        StartCoroutine(SendPostRequest(1));
        side.text = "Side: " + side_ui;
    }

    public void VoltarMenu1(bool left)
    {
        StartCoroutine(SendPostRequest(2));
        tela1.SetActive(true);
        tela2.SetActive(false);
        conveyorEBloco.SetActive(false);
    }

    public void Ok()
    {
        Image ButtonImage = button.GetComponent<Image>();

        if (ButtonImage.color == Color.green)
        {
            Debug.Log("Toma");
            EnviaResposta("HTTP/1.1 200 OK\n");
            ButtonImage.color = Color.grey;
            color.text = "Color: ";
            position.text = "Pos: ";
            cairdoBlocoAtivo.StopCair();
            StartCoroutine(SendPostRequest(3));
        }
    }

    public void ChangeIPAparecerButtons()
    {
        barraEscrita.SetActive(true);
        buttonChangeIp.SetActive(false);
    }

    public void ChangeIP()
    {
        if (ipInput.text == "")
            return;
        ipMaquinaACorrer = ipInput.text;
        PlayerPrefs.SetString("ipMaquinaACorrer", ipMaquinaACorrer);
        PCIP.text = "PC IP: " + PlayerPrefs.GetString("ipMaquinaACorrer");
        barraEscrita.SetActive(false);
        buttonChangeIp.SetActive(true);
    }

    private void Update()
    {
        if (atualizarUI)
        {
            color.text = "Color: " + Skill[1];
            position.text = "Pos: " + Skill[2];
            button.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            if (Skill[2] == "1")
            {
                cairdoBlocoAtivo = cairdoBlocoPos1;
            }
            else
            {
                cairdoBlocoAtivo = cairdoBlocoPos2;
            }
            cairdoBlocoAtivo.AtualizaCor(Skill[1]);
            cairdoBlocoAtivo.StartCair();
            atualizarUI = false;
        }
    }
}
