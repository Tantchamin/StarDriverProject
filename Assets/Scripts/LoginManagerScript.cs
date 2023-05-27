using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;
//using QSFW.QC;

public class LoginManagerScript : MonoBehaviour
{
    public TMP_InputField userNameInput, passNameInput;
    public GameObject loginPanel, leaveButton, scorePanel, mainMenuPanel, LoseButton, winButton, waitingText;
    public List<GameObject> SpawnPoints;
    public List<string> usedNames;
    public BossSpawnScript bossSpawnScript;
    int spawnedPoint = 5;
    int playerCount = 0;
    bool firstPlayer = true;
    string pass;
    string joinCode;
    public Button clientButton;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
        winButton.SetActive(false);
        LoseButton.SetActive(false);
        leaveButton.SetActive(false);
        waitingText.SetActive(false);

    }

    private void SetUIVisible(bool isUserLogin)
    {
        loginPanel.SetActive(!isUserLogin);
        leaveButton.SetActive(isUserLogin);
        scorePanel.SetActive(isUserLogin);
    }

    private void Update()
    {
        if(playerCount == 3)
        {
            clientButton.interactable = false;
        }
    }

    private void OnDestroy()
    {
        if(NetworkManager.Singleton == null) { return; }
        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
    }


    private void HandleClientDisconnect(ulong clientId)
    {

    }

    public void Leave()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown();
        }
        SetUIVisible(false);
        if (winButton == true)
        {
            winButton.SetActive(false);
        }
        if(waitingText == true)
        {
            waitingText.SetActive(false);
        }
        mainMenuPanel.SetActive(true);
        loginPanel.SetActive(false);
        LoseButton.SetActive(false);
    }

    private void HandleClientConnected(ulong clientId)
    {
        if(clientId == NetworkManager.Singleton.LocalClientId)
        {
            SetUIVisible(true);
            mainMenuPanel.SetActive(false);
        }
    }

    private void HandleServerStarted()
    {
        //throw new System.NotImplementedException();

    }

    public async void Host()
    {
        if (RelayManagerScript.Instance.IsRelayEnabled)
        {
            await RelayManagerScript.Instance.CreateRelay();
        }
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        NetworkManager.Singleton.StartHost();
        waitingText.SetActive(true);
    }

    public async void Client()
    { //setIpAddress();
        joinCode = passNameInput.GetComponent<TMP_InputField>().text;
        if (RelayManagerScript.Instance.IsRelayEnabled && !string.IsNullOrEmpty(joinCode))
        {
            await RelayManagerScript.Instance.JoinRelay(joinCode);
        }

        string userPass = passNameInput.GetComponent<TMP_InputField>().text;
        string userName = userNameInput.GetComponent<TMP_InputField>().text;
        NetworkManager.Singleton.NetworkConfig.ConnectionData =
        System.Text.Encoding.ASCII.GetBytes(userName+"/"+userPass); //String > Byte

        NetworkManager.Singleton.StartClient();
        waitingText.SetActive(false);

    }

    private bool approveConnection(string clientData, string serverData)
    {
        int counter = 0;
        bool isApprove = System.String.Equals(clientData.Trim(), serverData.Trim()) ? false : true;
        string[] strlist = clientData.Split('/', 2,System.StringSplitOptions.None);
        Debug.Log("1: " + strlist[0] + " 2: " + strlist[1] + " 3: " + serverData + " 4: " + pass);
        foreach (string name in usedNames)
        {
            if (strlist[0] == name)
            {
                counter++;
            }
        }
        if(strlist[1] != pass || counter != 0)
        {
            isApprove = false;
        }

        if (isApprove != false)
        {
            usedNames.Add(strlist[0]);
        }
        return isApprove;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request,
        NetworkManager.ConnectionApprovalResponse response)
    {
        if(firstPlayer == true)
        {
            pass = passNameInput.GetComponent<TMP_InputField>().text;
            usedNames.Add(userNameInput.GetComponent<TMP_InputField>().text);
        }

        // The client identifier to be authenticated
        var clientId = request.ClientNetworkId;

        // Additional connection data defined by user code
        var connectionData = request.Payload;
        
        int byteLength = connectionData.Length;
        bool isApprove = true;
        //if (byteLength > 0)
        //{
        //    string clientData = System.Text.Encoding.ASCII.GetString(connectionData, 0, byteLength);
        //    string hostData = userNameInput.GetComponent<TMP_InputField>().text;
        //    //string clientPass = passNameInput.GetComponent<TMP_InputField>().text;
        //    //Debug.Log("ClienentData: " + clientData + " ClientPass: " + clientPass + " HostData: " + hostData + "ServerPass: " + pass);
        //    isApprove = approveConnection(clientData, hostData);
        //}

        // Your approval logic determines the following values
        response.Approved = isApprove;
        response.CreatePlayerObject = true;

        // The prefab hash value of the NetworkPrefab, if null the default NetworkManager player prefab is used
        response.PlayerPrefabHash = null;

        // Position to spawn the player object (if null it uses default of Vector3.zero)
        response.Position = Vector3.zero;

        // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
        response.Rotation = SpawnPoints[0].transform.rotation;

        setSpawnLocation(clientId, response);

        // If response.Approved is false, you can provide a message that explains the reason why via ConnectionApprovalResponse.Reason
        // On the client-side, NetworkManager.DisconnectReason will be populated with this message via DisconnectReasonMessage
        response.Reason = "Some reason for not approving the client";

        // If additional approval steps are needed, set this to true until the additional steps are complete
        // once it transitions from true to false the connection approval response will be processed.
        response.Pending = false;
    }

    private void setSpawnLocation(ulong clientId, NetworkManager.ConnectionApprovalResponse response)
    {
        Vector3 spawnPos = Vector3.zero;
        Quaternion spawnRot = SpawnPoints[0].transform.rotation;

        if (firstPlayer == true)
        {
            spawnedPoint = 0;
        }
        else
        {
            spawnedPoint = 1;
        }

        spawnPos = new Vector3(SpawnPoints[spawnedPoint].transform.position.x, SpawnPoints[spawnedPoint].transform.position.y, SpawnPoints[spawnedPoint].transform.position.z);
        playerCount += 1;

        if(firstPlayer == false)
        {
            bossSpawnScript.SpawnBoss();
            waitingText.SetActive(false);
        }
        firstPlayer = false;
        response.Position = spawnPos; response.Rotation = spawnRot;
    }
}

