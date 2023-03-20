using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;

public class MainPlayerScript : NetworkBehaviour
{
    public float speed = 5.0f;
    public float sideSpeed = 10.0f;
    Rigidbody rb;


    public NetworkVariable<NetworkString> playerNameA = new NetworkVariable<NetworkString>(
        new NetworkString { info = "Player A" },
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<NetworkString> playerNameB = new NetworkVariable<NetworkString>(
        new NetworkString { info = "Player B" },
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private LoginManagerScript LoginManager;

    public struct NetworkString : INetworkSerializable
    {
        public FixedString32Bytes info;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref info);
        }
        public override string ToString()
        {
            return info.ToString();
        }
        public static implicit operator NetworkString(string v) =>
            new NetworkString() { info = new FixedString32Bytes(v) };
    }

    public override void OnNetworkSpawn()
    {
        //GameObject canvas = GameObject.FindWithTag("MainCanvas");
        //nameLabel = Instantiate(namePrefab, Vector3.zero, Quaternion.identity) as TMP_Text;
        //nameLabel.transform.SetParent(canvas.transform);



        if (IsOwner)
            LoginManager = GameObject.FindObjectOfType<LoginManagerScript>();
        if (LoginManager != null)
        {
            string name = LoginManager.userNameInput.text;
            if (IsOwnedByServer) { playerNameA.Value = name; }
            else { playerNameB.Value = name; }
        }

    }


    private void Start()
    {
    }

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            float translation = Input.GetAxis("Vertical") * speed;
            float translationSide = Input.GetAxis("Horizontal") * sideSpeed;
            translation *= Time.deltaTime;
            translationSide *= Time.deltaTime;

            transform.position += new Vector3(translationSide, translation, 0);

        }
    }
}
