using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LoadMenu(){
        SceneManager.LoadScene(0);
    }

    public void LoadPlayer(){
        SceneManager.LoadScene(1);
    }

    public void LoadAI(){
        SceneManager.LoadScene(2);
    }

    public void LoadRandom(){
        SceneManager.LoadScene(3);
    }
    
    public void Quit(){
        Application.Quit();
    }
}
