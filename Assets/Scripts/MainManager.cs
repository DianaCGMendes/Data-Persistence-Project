using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public string CurrentPlayerName; //holds the name of the player
    public string PlayerName;
    //public string LastPlayerName;
    public string BestScore;
    public Text ScoreText;
    public Text LastScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    public int HighScore;
    
    private bool m_GameOver = false;

    public void Awake()
    {
        LoadNameandScore();
    }
    // Start is called before the first frame update
    void Start()
    {
        LastScoreText.text = "Best Score: " + BestScore;

        PlayerName = UIManager.Instance.playerName;
        ScoreText.text = PlayerName + " - " + $"Score : 0";

        
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = CurrentPlayerName + " - " + $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveNameAndScore();
    }

    [System.Serializable]
    class SaveData
    {
        //public string playerName;
        //public int highScore;
        public string BestScore;
    }

    public void SaveNameAndScore()
    {
        SaveData data = new SaveData();
        //data.playerName = PlayerName;
        //data.highScore = m_Points; //change for highest score
        data.BestScore = PlayerName + " - " + m_Points;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

        
    }

    public void LoadNameandScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            //PlayerName = data.playerName;
            //HighScore = data.highScore;
            BestScore = data.BestScore;                                                               
        }
    }
}
