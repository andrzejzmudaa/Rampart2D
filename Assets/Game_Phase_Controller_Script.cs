using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Phase_Controller_Script : MonoBehaviour
{
    public enum phase_Enum {GameNotStarted, Pause, Build, putCannons, Battle }
    private phase_Enum GameState;
    public UnityEngine.UI.Slider slider;
    public UnityEngine.UI.Text displayText;
    // Start is called before the first frame update
    void Start()
    {
        GameState = phase_Enum.GameNotStarted;
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

}

