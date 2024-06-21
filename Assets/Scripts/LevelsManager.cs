using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    public GameObject levelContainer;
    public GameObject levelObj;
    public void ShowLevel(){
        int mapLen = Resources.Load<TextAsset>("map").ToString().Split("]],[[").Length;
        List<ScoreData> scores = ScoresManager.GetScore();
        int maxLevel = -1;
        List<GameObject> levels = new List<GameObject>();
        for(int i=0; i < mapLen; i++){
            GameObject obj = Instantiate(levelObj, transform.position, Quaternion.identity, levelContainer.transform);
            Level level = obj.GetComponent<Level>();
            if(scores != null){
                foreach(ScoreData score in scores){
                    Debug.Log(score.level + " " + score.score);
                    if(score.level == i){
                        level.SetScore(score.score);
                        level.SetLevel(i);
                        level.Toggle(true);
                        if(score.score > maxLevel) maxLevel = i;
                        break;
                    }
                }
            }
            obj.SetActive(true);
            levels.Add(obj);
        }
        if(maxLevel+1 < mapLen){
            Level level = levels[maxLevel+1].GetComponent<Level>();
            level.Toggle(true);
            level.HideStars();
            level.SetLevel(maxLevel+1);
        }
    }
    
    public void Start(){
        ShowLevel();
    }
}
