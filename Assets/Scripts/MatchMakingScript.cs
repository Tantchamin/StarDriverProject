using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
//using QFSW.QC;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;
using System.Threading.Tasks;
//using ParrelSync;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using TMPro;

public class MatchMakingScript : MonoBehaviour
{
    public TMP_InputField playNameInput;
    public GameObject startButton;
    public GameObject matchMakingPanel, waitingText;
    private string playerName;
    string lobbyName = "MyLobby";
    private Lobby joinedLobby;
    bool firstPlayer = true;
    public BossSpawnScript bossSpawnScript;
    public List<GameObject> SpawnPoints;
    int spawnedPoint = 5;
    int playerCount = 0;



    public async void StartGame()
    {
        //startButton.SetActive(false);
        matchMakingPanel.SetActive(false);
        playerName = playNameInput.GetComponent<TMP_InputField>().text;
        //joinedLobby = await CreateLobby();
        joinedLobby = await JoinLobby() ?? await CreateLobby();
        if (joinedLobby == null)
        {
            startButton.SetActive(true);
            matchMakingPanel.SetActive(true);
        }


    }

    private async Task<Lobby> JoinLobby()
    {
        try
        {
            Lobby lobby = await FindRandomLobby();
            if (lobby == null) return null;

            if (lobby.Data["JoinCodeKey"].Value != null)
            {
                string joinCode = lobby.Data["JoinCodeKey"].Value;
                Debug.Log("joincode = " + joinCode);
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

                RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
                NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(playerName);
                NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                NetworkManager.Singleton.StartClient();
                return lobby;
            }
            return null;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("No lobby found");
            return null;
        }
    }

    private async Task<Lobby> FindRandomLobby()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"1",QueryFilter.OpOptions.GE)
                }
            };
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                return lobby;
            }
            return null;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    private async Task<Lobby> CreateLobby()
    {
        try
        {
            int maxPlayer = 4;
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayer);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = new Player
                {
                    Data = new Dictionary<string, PlayerDataObject>
                    {
                        {"PlayerName",
                            new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member,playerName)}
                    }
                },
                Data = new Dictionary<string, DataObject>
                {
                    {"JoinCodeKey", new DataObject(DataObject.VisibilityOptions.Public, joinCode) }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer, createLobbyOptions);
            StartCoroutine(HeartBeatLobbyCoroutine(lobby.Id, 15));
            Debug.Log("Created Lobby : " + lobby.Name + " , " + lobby.MaxPlayers + " , " + lobby.Id + " , " + lobby.LobbyCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();

            LobbyManagerScript.Instance.PrintPlayers(lobby);
            return lobby;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    private static IEnumerator HeartBeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request,
        NetworkManager.ConnectionApprovalResponse response)
    {

        // The client identifier to be authenticated
        var clientId = request.ClientNetworkId;

        // Additional connection data defined by user code
        var connectionData = request.Payload;

        int byteLength = connectionData.Length;
        bool isApprove = true;


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

        if (firstPlayer == false)
        {
            bossSpawnScript.SpawnBoss();
            waitingText.SetActive(false);
        }
        firstPlayer = false;
        response.Position = spawnPos; response.Rotation = spawnRot;
    }
}
