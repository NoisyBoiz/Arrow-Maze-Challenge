using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Level : MonoBehaviour
{   
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject starContainer;
    [SerializeField] private List<Image> stars;
    [SerializeField] private Sprite starWin;
    [SerializeField] private Sprite starLose;
    [SerializeField] private TextMeshProUGUI levelText;
    private int level = -1;
    public void SetLevel(int val){
        level = val;
        levelText.text = (val+1).ToString();
    }
    public void SetScore(int val){
        for(int i = 0; i < stars.Count; i++){
            if(i < val) stars[i].sprite = starWin;
            else stars[i].sprite = starLose;
        }
    }
    public void HideStars(){
        starContainer.SetActive(false);
    }
    public void Toggle(bool val){
        if(val){
            content.SetActive(true);
            lockIcon.SetActive(false);
        }
        else{
            content.SetActive(false);
            lockIcon.SetActive(true);
        }
    }
    public void OnClick(){
        ScenesManager.LoadLevel(level);
    }
}
