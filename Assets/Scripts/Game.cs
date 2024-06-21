using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Game : MonoBehaviour{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject grid;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private GameObject move;
    [SerializeField] private GameObject step;
    [SerializeField] private GameObject move4;
    [SerializeField] private GameObject goat;
    [SerializeField] private GameObject blank;
    [SerializeField] private GameObject stepArrow;
    [SerializeField] private GameObject stepRed;

    [SerializeField] private GameObject winAlert;
    [SerializeField] private GameObject loseAlert;
    [SerializeField] private TextMeshProUGUI timer;

    private List<List<int>> map = new List<List<int>>();
    private List<List<GameObject>> objMap = new List<List<GameObject>>();
    List<Vector2Int> goatList = new List<Vector2Int>();
    List<GameObject> moveList = new List<GameObject>();
    List<GameObject> changeDirList = new List<GameObject>();    
    private GameObject moveObjSelect;
    private GameObject move4ObjSelect;
    private List<GameObject> stepRedList = new List<GameObject>();
    private Vector3 destination;
    private float speed = 8.0f;
    private float timeDuration = 45.0f;
    Vector2Int[] dir = {Constant.upVec, Constant.rightVec, Constant.downVec, Constant.leftVec};

    private bool isMove = false;
    private bool isMove4 = false;
    private bool isEnd = false;
    private int mapLen = 0;
    private Coroutine coroutine;
    void LoadMap(int index){
        string jsonString = Resources.Load<TextAsset>("map").ToString();
        jsonString = jsonString.Substring(1, jsonString.Length-2);
        string[] mapArr = jsonString.Split("]],[[");
        mapLen = mapArr.Length;
        string mapStr = mapArr[index].Replace("[[", "").Replace("]]", "");
        string[] row = mapStr.Split("],[");

        for(int i=0;i<row.Length;i++){
            string[] colStr = row[i].Split(",");
            List<int> col = new List<int>();
            for(int j=0;j<colStr.Length;j++){
                col.Add(int.Parse(colStr[j]));
            }
            map.Add(col);
        }
        gridLayoutGroup.constraintCount = map.Count;
    }
    void DrawMap(){
        for(int i=0;i<map.Count;i++){
            List<GameObject> row = new List<GameObject>();
            for(int j=0;j<map[i].Count;j++){
                GameObject obj = null;
                if(map[i][j] == Constant.blank){
                    obj = Instantiate(blank, new Vector3(j, -i, 0), Quaternion.identity, grid.transform);
                }
                if(map[i][j] == Constant.step || map[i][j] == Constant.move4 || map[i][j] == Constant.moveU || map[i][j] == Constant.moveR || map[i][j] == Constant.moveD || map[i][j] == Constant.moveL){
                    obj = Instantiate(step, new Vector3(j, -i, 0), Quaternion.identity, grid.transform);
                }
                if(map[i][j] == Constant.stepArrowL || map[i][j] == Constant.stepArrowR || map[i][j] == Constant.stepArrowU || map[i][j] == Constant.stepArrowD){
                    obj = Instantiate(stepArrow, new Vector3(j, -i, 0), Quaternion.identity, grid.transform);
                    obj.GetComponent<Move>().SetIndex(i, j);
                    obj.GetComponent<Move>().SetDirection(map[i][j]-7);
                    changeDirList.Add(obj);
                    map[i][j] = Constant.step;
                }
                if(map[i][j] == Constant.goat){
                    obj = Instantiate(goat, new Vector3(j, -i, 0), Quaternion.identity, grid.transform);
                    goatList.Add(new Vector2Int(i, j));
                    map[i][j] = Constant.step;
                }
                row.Add(obj);
                if(obj!=null) obj.SetActive(true);
            }
            objMap.Add(row);
        }
        StartCoroutine(LoadObject());
    }

    IEnumerator LoadObject(){
        yield return new WaitForSeconds(0.0f);
        for(int i=0;i<map.Count; i++){
            for(int j=0;j<map[i].Count;j++){
                if(map[i][j] == Constant.moveU || map[i][j] == Constant.moveR || map[i][j] == Constant.moveD || map[i][j] == Constant.moveL){
                    GameObject obj = Instantiate(move, objMap[i][j].transform.position , Quaternion.identity, panel.transform);
                    obj.GetComponent<Move>().SetIndex(i, j);
                    obj.GetComponent<Move>().SetDirection(map[i][j]-3);
                    obj.SetActive(true);
                    moveList.Add(obj);
                }
                if(map[i][j] == Constant.move4){
                    GameObject obj = Instantiate(move4, objMap[i][j].transform.position , Quaternion.identity, panel.transform);
                    obj.GetComponent<Move4>().SetIndex(i, j);
                    obj.SetActive(true);
                }
            }
        }
    }
    public void HandleMove(GameObject obj){
        if(isMove4 || isEnd || isMove || move4ObjSelect!=null) return;
        int stX = obj.GetComponent<Move>().GetIndex().x;
        int stY = obj.GetComponent<Move>().GetIndex().y;
        int desX = stX;
        int desY = stY;
        Vector2Int objDir =  obj.GetComponent<Move>().GetDirection();

        if(objDir == Constant.rightVec){
            for(int i=stY+1;i<map[stX].Count;i++){
                if(map[stX][i] == Constant.step) desY = i;
                else break;
            }
        }
        else if(objDir == Constant.leftVec){
            for(int i=stY-1;i>=0;i--){
                if(map[stX][i] == Constant.step) desY = i;
                else break;
            }
        }
        else if(objDir == Constant.downVec){
            for(int i=stX+1;i<map.Count;i++){
                if(map[i][stY] == Constant.step) desX = i;
                else break;
            }
        }
        else if(objDir == Constant.upVec){
            for(int i=stX-1;i>=0;i--){
                if(map[i][stY] == Constant.step) desX = i;
                else break;
            }
        }
        if(desX == stX && desY == stY) return;
        moveObjSelect = obj;
        map[desX][desY] = map[stX][stY];
        map[stX][stY] = Constant.step;
        obj.GetComponent<Move>().SetIndex(desX, desY);

        destination = objMap[desX][desY].transform.position;
        isMove = true;  
    }

    public void StartMove4(GameObject obj){
        if(isMove4 || isEnd || isMove) return;
        if(move4ObjSelect == null){
            move4ObjSelect = obj;
            Vector2Int stIndex = obj.GetComponent<Move4>().GetIndex();
            for(int i=0;i<4;i++){
                int x = stIndex.x + dir[i].x;
                int y = stIndex.y + dir[i].y;
                if(x>=0 && x<map.Count && y>=0 && y<map[x].Count && map[x][y] == Constant.step && !InGoat(x, y)){
                    objMap[x][y].GetComponent<Step>().Hide();
                    GameObject newStepRed = Instantiate(stepRed, objMap[x][y].transform.position, Quaternion.identity, panel.transform);
                    newStepRed.GetComponent<Move4>().SetIndex(x, y);
                    newStepRed.SetActive(true);
                    stepRedList.Add(newStepRed);
                }
            }
        }
        else{
            if(move4ObjSelect == obj) {
                EndMove4();
                move4ObjSelect = null;
            }
        }
    }
   
    public void HandleMove4(GameObject obj){
        if(move4ObjSelect == null) return;
        destination = obj.transform.position;
        Vector2Int stIndex = move4ObjSelect.GetComponent<Move4>().GetIndex();
        Vector2Int desIndex = obj.GetComponent<Move4>().GetIndex();
        map[desIndex.x][desIndex.y] = map[stIndex.x][stIndex.y];
        map[stIndex.x][stIndex.y] = Constant.step;
        move4ObjSelect.GetComponent<Move4>().SetIndex(desIndex);
        StartCoroutine(DelayMove4());
        EndMove4();
    }
    IEnumerator DelayMove4(){
        yield return new WaitForSeconds(0.2f);
        isMove4 = true;
    }
    public void EndMove4(){
        for(int i=0;i<stepRedList.Count;i++){
            int x = stepRedList[i].GetComponent<Move4>().GetIndex().x;
            int y = stepRedList[i].GetComponent<Move4>().GetIndex().y;
            objMap[x][y].GetComponent<Step>().Show();
            stepRedList[i].GetComponent<Step>().Hide();
        }
        if(coroutine!=null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(HideStepRed());
    }
    
    IEnumerator HideStepRed(){
        yield return new WaitForSeconds(0.15f);
        for(int i=0;i<stepRedList.Count;i++){
            Destroy(stepRedList[i]);
        }
        stepRedList.Clear();
    }

    bool InGoat(int x, int y){
        for (int i = 0; i < goatList.Count; i++){
            if(goatList[i].x == x && goatList[i].y == y) return true;
        }
        return false;
    }

    bool IsWin(){
        for(int i = 0; i < moveList.Count; i++){
            Vector2Int stIndex = moveList[i].GetComponent<Move>().GetIndex();
            if(!InGoat(stIndex.x, stIndex.y)) return false;
        }
        return true;
    }

    void UpdateTimer(){
        if(isEnd) return;
        timeDuration -= Time.deltaTime;
        string timeStr = "";
        if(timeDuration < 10) timeStr = "00 : 0" + timeDuration.ToString("0");
        else timeStr = "00 : " + timeDuration.ToString("0");
        timer.text = timeStr;
        if(timeDuration <= 0){
            isEnd = true;
            StartCoroutine(ShowLoseAlert());
        }
    }

    void Start(){
        LoadMap(ScenesManager.level);
        DrawMap();
    }
    void Update()
    {
        UpdateTimer();
        if(isEnd) return;
        if(isMove){
            if(moveObjSelect == null) {
                isMove = false;
                return;
            }

            if(Vector3.Distance(moveObjSelect.transform.position, destination) > 0.5f){
                for(int i=0;i<changeDirList.Count;i++){
                    if(Vector3.Distance(moveObjSelect.transform.position, changeDirList[i].transform.position) < speed){
                        moveObjSelect.GetComponent<Move>().SetDirection(changeDirList[i].GetComponent<Move>().GetDirection());
                    }
                }
                moveObjSelect.transform.position = Vector3.MoveTowards(moveObjSelect.transform.position, destination, speed);
            }
            else{
                moveObjSelect.transform.position = destination;
                for(int i=0;i<changeDirList.Count;i++){
                    if(Vector3.Distance(moveObjSelect.transform.position, changeDirList[i].transform.position) < speed){
                        moveObjSelect.GetComponent<Move>().SetDirection(changeDirList[i].GetComponent<Move>().GetDirection());
                        break;
                    }
                }
                isMove = false;
                moveObjSelect = null;
                if(IsWin()){
                    isEnd = true;
                    StartCoroutine(ShowWinAlert());
                }
            }
        }

        if(isMove4){
            if(move4ObjSelect == null) {
                isMove = false;
                return;
            }

            if(Vector3.Distance(move4ObjSelect.transform.position, destination) > 0.5f){
                move4ObjSelect.transform.position = Vector3.MoveTowards(move4ObjSelect.transform.position, destination, speed);
            }
            else{
                move4ObjSelect.transform.position = destination;
                isMove4 = false;
                move4ObjSelect = null;
            }
        }
    }

    IEnumerator ShowLoseAlert(){
        yield return new WaitForSeconds(0.5f);
        loseAlert.SetActive(true);
    }
    IEnumerator ShowWinAlert(){
        yield return new WaitForSeconds(0.5f);
        int score = 1;
        if(timeDuration >= 30) score = 3;
        else if(timeDuration >= 15) score = 2;
        ScoresManager.SaveScore(ScenesManager.level, score);
        winAlert.GetComponent<WinAlert>().SetScore(score);
        if(ScenesManager.level >= mapLen-1) winAlert.GetComponent<WinAlert>().HideBtnNextLevel();
        winAlert.SetActive(true);
    }

    public void ChangeScene(string scene){
        if(isEnd) return;
        ScenesManager.ChangeScene(scene);
    }
}
