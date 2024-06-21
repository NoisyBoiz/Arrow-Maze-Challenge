using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoresManager : MonoBehaviour{
    public static List<ScoreData> listData = new List<ScoreData>();
    public static void SaveScore(int level, int score){
        ScoreData data = new ScoreData(level, score);
        if(listData.Count == 0){
            listData.Add(data);
        }
        else{
            for(int i = 0; i < listData.Count; i++){
                if(listData[i].level == level && listData[i].score < score){
                    listData[i].score = score;
                    return;
                }
            }
            listData.Add(data);
        }

        // saveInFile(level, score);
    }

    public static List<ScoreData> GetScore(){
        return listData;
        
        // getInFile(); 
    }


    public void SaveInFile(int level, int score){
        ScoreData data = new ScoreData(level, score);
        string json = PlayerPrefs.GetString("Score");
        if(json != ""){
            List<ScoreData> oldData = JsonUtility.FromJson<List<ScoreData>>(json);
            for(int i = 0; i < oldData.Count; i++){
                if(oldData[i].level == level && oldData[i].score < score){
                    oldData[i].score = score;
                    json = JsonUtility.ToJson(oldData);
                    PlayerPrefs.SetString("Score", json);
                    return;
                }
            }
            oldData.Add(data);
            json = JsonUtility.ToJson(oldData);
            PlayerPrefs.SetString("Score", json);
        }
        else{
            List<ScoreData> list = new List<ScoreData>();
            list.Add(data);
            json = JsonUtility.ToJson(list);
            PlayerPrefs.SetString("Score", json);
        }
    }

    public List<ScoreData> GetInFile(){
        string json = PlayerPrefs.GetString("Score");
        if(json != ""){
            return JsonUtility.FromJson<List<ScoreData>>(json);
        }
        return null;
    }
}

