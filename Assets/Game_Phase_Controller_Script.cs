﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Phase_Controller_Script : MonoBehaviour
{
    public enum phase_Enum {GameNotStarted, Pause, Build, putCannons, Battle }
    private phase_Enum GameState;
    public bool friendlyFireOn;
    public UnityEngine.UI.Slider slider;
    public UnityEngine.UI.Text displayText;
    public PlayerManager[] listOfPlayers;
    // Start is called before the first frame update
    void Start()
    {
        GameState = phase_Enum.GameNotStarted;
        listOfPlayers = this.gameObject.transform.GetComponentsInChildren<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (slider.value)
        {
            case 0:
                GameState = phase_Enum.GameNotStarted;
                break;
            case 1:
                GameState = phase_Enum.Pause;
                break;
            case 2:
                GameState = phase_Enum.Build;
                break;
            case 3:
                GameState = phase_Enum.putCannons;
                break;
            case 4:
                GameState = phase_Enum.Battle;
                break;


        }
        displayText.text = GameState.ToString();
    }

    public phase_Enum getGameStateStatus()
    {
        return GameState;
    }

    public void hitPlayerMapField(Vector3 _targerPos , PlayerManager _ballSourcePlayer)
    {
        if (listOfPlayers == null)
            return;
        foreach(PlayerManager player in listOfPlayers)
        {
            if ((_ballSourcePlayer == player && friendlyFireOn) || _ballSourcePlayer != player)
            {
                Vector3Int targetPosInt = Vector3Int.FloorToInt(_targerPos);
                //Debug.Log("Vector3 original: " + _targerPos + " Vector3Int: " + targetPosInt);
                player.verifyPlayerFieldHit(targetPosInt);
            }
        }

    }
}

