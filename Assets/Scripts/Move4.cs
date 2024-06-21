using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move4 : MonoBehaviour
{
    [SerializeField] private Vector2Int index;
    public void SetIndex(int x, int y){
        index = new Vector2Int(x, y);
    }
    public void SetIndex(Vector2Int val){
        index = val;
    }
    public Vector2Int GetIndex(){
        return index;
    }
}
