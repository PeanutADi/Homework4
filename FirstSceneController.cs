using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

class ScoreRecorder
{
    public int score;
    public Dictionary<Color, int> colorScore = new Dictionary<Color, int>();
    public Dictionary<float, int> speedScore = new Dictionary<float, int>();

    public ScoreRecorder()
    {
        score = 0;
        colorScore.Add(Color.white, 1);
        colorScore.Add(Color.yellow, 2);
        colorScore.Add(Color.black, 3);

        speedScore.Add(1f, 1);
        speedScore.Add(2f, 2);
        speedScore.Add(3f, 3);
    }

    public void record(DiskData disk,int round)
    {
       score += round*colorScore[disk.GetComponent<DiskData>().color];
    }

    public string toString() { return score.ToString(); }

    public void reset() { score = 0; }
}

public class FirstSceneController : MonoBehaviour {

    public Camera cam;

    public enum state { START, END, GAME_START, GAME_END, RUNNING };
    int round;

    List<DiskData> disklist = new List<DiskData>();
    DiskFactory factory;
    ScoreRecorder recorder;

    private float timeGap = 0;
    int left;
    public int disk;

    public float g = 1f;

    Color[] diskColors = { Color.white, Color.yellow, Color.black };
    float[] diskSpeed = { 0.3f, 0.6f, 1f };

    public void Load()
    {
        factory = Singleton<DiskFactory>.Instance;
        recorder = new ScoreRecorder();
    }

    void recycleDisk(DiskData disk)
    {
        disk.gameObject.transform.position = new Vector3(0, 0, 100f);
        factory.DiskWait(disk);
        disklist.Remove(disk);
    }

    void moveDisk(DiskData disk)
    {
        float px, py, pz;
        px = disk.gameObject.transform.position.x + disk.speed * disk.direction.x * Time.deltaTime;
        pz = disk.gameObject.transform.position.z + disk.speed * disk.direction.z * Time.deltaTime;
        py = disk.gameObject.transform.position.y + disk.speed * (disk.direction.y+g*Time.deltaTime) * Time.deltaTime;
        disk.gameObject.transform.position = new Vector3(px, py, pz);
    }

    void updateMethod()
    {
        if (round < 4)
        {
            timeGap += Time.deltaTime;
            if (timeGap >= 2f)
            {
                int wait = UnityEngine.Random.Range(3, disk / 3);
                left -= wait;
                if (left <= 0)
                {
                    round++;
                    left = disk;
                }
                for (int i = 0; i < wait; i++)
                {
                    Color color = diskColors[UnityEngine.Random.Range(0, 3)];
                    float speed = diskSpeed[round];
                    Vector3 direction = new Vector3(
                        UnityEngine.Random.Range(-1000f, 1000f),
                        UnityEngine.Random.Range(-1000f, 1000f),
                        0
                        );
                    direction.Normalize();
                    ruler tmpRuler = new ruler(color, direction, Vector3.zero);
                    tmpRuler.vx = UnityEngine.Random.Range(-1f, 1f);
                    tmpRuler.vy = UnityEngine.Random.Range(0f, 5f);
                    tmpRuler.vz = UnityEngine.Random.Range(-1f, 1f);
                    disklist.Add(factory.GetDisk(Vector3.zero, new Vector3(tmpRuler.vx, tmpRuler.vy, tmpRuler.vz), diskSpeed[round], color));
                }
                timeGap = 0;
            }
            for (int i = 0; i < disklist.Count; i++)
            {
                if (disklist[i].gameObject.transform.position.y < 0)
                {
                    recycleDisk(disklist[i]);
                }
            }
            for (int i = 0; i < disklist.Count; i += 1)
            {
                moveDisk(disklist[i]);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;
                if (Physics.Raycast(ray, out raycastHit))
                {
                    recorder.record(raycastHit.transform.gameObject.GetComponent<DiskData>(), round);
                    recycleDisk(raycastHit.transform.gameObject.GetComponent<DiskData>());
                    // GUI.Label(new Rect(10, 10, 50, 100),recorder.toString());
                }
            }
        }
    }

    void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        director.currentSceneController = this;
        director.currentSceneController.Load();
        round = 0;
    }

    public void Start()
    {

    }

    public void Update()
    {
        updateMethod();
    }

    enum GameState { WAITING, RUNNING };

    private void OnGUI()
    {
        GameState gs = GameState.RUNNING;
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;
        fontStyle.normal.textColor = new Color(1, 0, 0);
        fontStyle.fontSize = 20;
        GUI.color = Color.red;
        GUI.Label(new Rect(Screen.width - 150, Screen.height - 200, 150, 200), "White-1 pt\nYellow-2 pt\nBlack-3pt\nScore=pt*round");
        GUI.Label(new Rect(10, 10, 100, 100), "Score:" + recorder.toString());
        GUI.Label(new Rect(10, 30, 50, 100), "Round:" + round.ToString());
        if (round == 4 || GUI.Button(new Rect(Screen.width - 100, Screen.height -100, 100, 50), "End game")) GUI.Label(new Rect(Screen.width / 2 - 25, Screen.height / 2, 50, 100), "Game End! Your Score:" + recorder.toString(), fontStyle);
        if (GUI.Button(new Rect(Screen.width -100, Screen.height - 50, 100, 50), "Next Round"))
            round += 1;


    }
}
