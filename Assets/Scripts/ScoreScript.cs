using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using System;

public class ScoreScript : NetworkBehaviour
{
    TMP_Text p1Text, p2Text;
    MainPlayerScript mainPlayer;

    private void Start()
    {
        p1Text = GameObject.FindGameObjectWithTag("P1Text").GetComponent<TMP_Text>();
        p2Text = GameObject.FindGameObjectWithTag("P2Text").GetComponent<TMP_Text>();
        mainPlayer = GetComponent<MainPlayerScript>();

    }

    private void Update()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        if (IsOwnedByServer)
        {
            p1Text.text = $"{mainPlayer.playerNameA.Value}";
        }
        else
        {
            p2Text.text = $"{mainPlayer.playerNameB.Value}";
        }
    }

}
