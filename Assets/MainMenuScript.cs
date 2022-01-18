using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    int previousScene;
    public void PlayGame()
    {
        previousScene = 1;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HostGame()
    {
        previousScene = 2;
        NetworkManager.Singleton.StartServer();
        if ( NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
        {
            Debug.Log("Server running");
        }

        
        //SceneManager.LoadScene(2);
    }

    public void JoinGame()
    {
        previousScene = 3;
        NetworkManager.Singleton.StartClient();
        if (NetworkManager.Singleton.IsClient)
        {
            Debug.Log("I am client");
        }
        
        //SceneManager.LoadScene(3);
    }

    public void Back()
    {
        SceneManager.LoadScene(previousScene);
    }
}
