using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreData : MonoBehaviour
{
    public int level;
    public int score;
    public ScoreData(int level, int score){
        this.level = level;
        this.score = score;
    }
}
