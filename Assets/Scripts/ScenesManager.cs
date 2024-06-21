using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static int level;
    public static void LoadNextLevel(){
        level++;
        SceneManager.LoadScene("Game");
    }
    public static void LoadLevel(int index){
        if(index == -1) return;
        level = index;
        SceneManager.LoadScene("Game");
    }
    public static void ChangeScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
    public static void QuitGame()
    {
        Application.Quit();
    }
}
