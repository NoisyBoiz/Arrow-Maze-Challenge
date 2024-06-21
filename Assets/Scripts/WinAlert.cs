using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinAlert : MonoBehaviour
{
    [SerializeField] private List<Image> stars;
    [SerializeField] private Sprite starWin;
    [SerializeField] private Sprite starLose;
    [SerializeField] private GameObject btnNextLevel;

    public void SetScore(int val){
        for(int i = 0; i < stars.Count; i++){
            if(i < val) stars[i].sprite = starWin;
            else stars[i].sprite = starLose;
        }
    }
    public void ShowBtnNextLevel(){
        btnNextLevel.SetActive(true);
    }
    public void HideBtnNextLevel(){
        btnNextLevel.SetActive(false);
    }
}
