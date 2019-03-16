using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenButton : MonoBehaviour
{

    public AudioSource startSound;

    private void startGame()
    {
        SceneManager.LoadScene("_Scene_0");
    }

    public void onClick()
    {
        startSound.Play();
        Invoke("startGame", 1.5f);
            
    }
}
