using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private Vector2Int index;
    [SerializeField] private Vector2Int direction; 
    [SerializeField] private GameObject arrow;   
    public void SetDirection(int val){
        if(val == Constant.upNum) direction = Constant.upVec;
        if(val == Constant.rightNum) direction = Constant.rightVec;
        if(val == Constant.downNum) direction = Constant.downVec;
        if(val == Constant.leftNum) direction = Constant.leftVec;
        RotateArrow(direction);
    }
    public void SetDirection(Vector2Int val){
        direction = val;
        RotateArrow(direction);
    }
    public void RotateArrow(Vector2Int val){
        if(val == Constant.upVec) arrow.transform.localRotation = Quaternion.Euler(0, 0, -90);
        if(val == Constant.rightVec) arrow.transform.localRotation = Quaternion.Euler(0, 0, 180);
        if(val == Constant.downVec) arrow.transform.localRotation = Quaternion.Euler(0, 0, 90);
        if(val == Constant.leftVec) arrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
    public void SetIndex(int x, int y){
        index = new Vector2Int(x, y);
    }
    public void SetIndex(Vector2Int val){
        index = val;
    }

    public Vector2Int GetIndex(){
        return index;
    }
    public Vector2Int GetDirection(){
        return direction;
    }
}
