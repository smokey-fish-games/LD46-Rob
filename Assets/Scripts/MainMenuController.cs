using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public void setMusic(bool value)
    {
        Settings.setMusic(value);
    }

    public void setDifficulty(Int32 di)
    {
        Settings.setDiff(di);
    }

    public void startGame()
    {
        //load the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
