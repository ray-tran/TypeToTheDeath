//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public struct Enemy
{
    public GameObject EnemyBall, TriggerWord;
    public int curShot;
    public bool locked;
    public bool dead;

    public Enemy(GameObject go1, GameObject go2)
    {
        EnemyBall = go1;
        TriggerWord = go2;
        curShot = -1;
        locked = false;
        dead = false;
    }
};


public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy1Prefab;
    public GameObject Enemy2Prefab;
    public GameObject Enemy3aPrefab;
    public GameObject Enemy3bPrefab;
    public GameObject Enemy3cPrefab;
    public GameObject TriggerWordPrefab;
    public GameObject BulletPrefab;
    public GameObject PlayerExplosionPrefab;
    public GameObject EndWaveTextPrefab;

    public static float topY = 10f;
    public int enemyCount = 5;
    private float lastAddTime = 0f;
    public Transform shipTransform;
    GameObject shipGO;
    public Queue<Enemy> enemyQueue;

    public Enemy lockedEnemy;
    public int wordIndex = 0;
    public List<List<string>> triggerWords;
    public List<int> wavesType;
    public List<float> addInterval;

    private float lastCheckKeyTime = 0f;
    public float checkInterval = 0.01f;
    public bool noLocked = true;

    public int curWave = 1;
    public int waveEnemySpawned = 0;
    public int waveEnemyKilled = 0;
    public Text waveGT;
    public float waveTotalPress = 0f;
    public float waveCorrectPress = 0f;
    public int waveTotalEnemy;

    public Text scoreGT;
    public int score = 0;
    public int highScore = 0;


    // Start is called before the first frame update
    void Start()
    {

        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
            //print("1 high score: " + highScore);
        }

        triggerWords = new List<List<string>>
        {
            new List<string> {"feet","dips", "haze","avid","gild","even","laid","tame","read","beet", "cake", "apls", "fist", "garb"},
            new List<string> {"bake","write", "tip","cheese","seven","counting","fried","out","eat","must"},
            new List<string> {"x","p","c","a","u","q","e","n","v","o","m","h","j","d"},
            new List<string> {"beget","hike", "onto","robe","calm","wire","mile","gobs","deem","voice","toter","jane","shun"},
            new List<string> {"f","y","q","o","h","w","g","m","k","g","e","s","p","a"},
            new List<string> {"lilt","peck", "wipes","relic","navel","grill","ache","zeta","coed","boils"},
            new List<string> {"uzcfh","rxmf", "lpccv","vitoe","fretc","zoeow","iopfb"},

            new List<string> {"listen", "crawl","happen", "raised","smiling","build","novice","flies","people"},
            new List<string> {"earth","fever","useful", "zebra", "cottage","usual","pioneer","terrible","surprise","library"},
            new List<string> {"u","y","a","i","k","w","o","l","b","p"},

            new List<string> {"mowed","inks", "taos","dent","saps","goers","ottos","gibe","teal","nobs"},
            new List<string> {"ermv","vhsr", "nopav","ofbh","mbus","flnu","wvoa","kmvi"},
            new List<string> {"ntzrl","mrtk", "wqjv","ltzsp","ojyr","gevg","kwfv","pqvds"},

            new List<string> {"numeral", "mammal","except", "favorite", "stomach","probably","different", "language","inventory","journey"},
            new List<string> {"z","n","j","e","i","k","p","u","q","i","f","m","s","g","o","w"},
            new List<string> {"q","v","t","y","w","s","k","s","u","g","e","l","n","z"},

            new List<string> {"especially", "league", "ancient", "disastrous", "vault", "honorable", "persuade", "requirement", "nationality", "scissors"},
            new List<string> {"yvwn","jzyt", "bync","whrqn","xtjj","akhcs","lnc","owxfp"},

            new List<string> {"wdhb","jigtd", "vjuw","yqfjt","gymdr","aezoe","meltm","ogisn"},
            new List<string> {"quotation", "phantom", "statistics", "endurance", "advantageous", "courtesy", "parallel", "influence", "souvenir", "questionnaire"},
            new List<string> {"fpdgmdalf","dkidjngbhd","plihbdnf","lebmmsba","nqdokfbbs"},
            new List<string> {"deficiency", "outrageous", "rectangular", "honorary", "malady", "necessity", "ubiquitous", "potpourri", "vivacious", "anachronistic"},
        };
        waveTotalEnemy = triggerWords[0].Count;
        wavesType = new List<int> {1,1,3,1,3,1,2,1,1,3,1,2,2,1,3,3,1,2,2,1,2,1};
        addInterval = new List<float> { 1f, 1f, 0.5f, 1f, 0.5f, 1f, 1.3f, 1f, 1f, 0.4f, 1f, 1.2f, 1.1f, 1f, 0.4f, 0.3f, 1f, 1f, 1f, 1f, 1f, 1f };
        shipGO = GameObject.Find("Ship");
        shipTransform = shipGO.transform;
        waveGT = GameObject.Find("Wave").GetComponent<Text>();
        waveGT.text = "Wave: 1";
        scoreGT = GameObject.Find("Score").GetComponent<Text>();
        scoreGT.text = "Score: 0";
        enemyQueue = new Queue<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        string pressedKey = DetectPressedKeyOrButton().ToLower();
        if (!isEndWaveArray())
        {
            if (lastAddTime + addInterval[curWave-1] < Time.time && enemyQueue.Count < enemyCount)
            {
                lastAddTime = Time.time;
                createRandomEnemy(wavesType[curWave-1]);
            }
        }

        for (int i = 0; i < enemyQueue.Count; i++)
        {
            Enemy curEnemy = enemyQueue.Dequeue();
            GameObject curEnemyBall = curEnemy.EnemyBall;
            GameObject curTriggerWord = curEnemy.TriggerWord;
            string curText = curTriggerWord.GetComponent<Text>().text;

            if (noLocked && pressedKey != "space")
            {
                if (pressedKey[0] == curText[0])
                {
                    //print("you just typed " + pressedKey[0] + " which will lock " + curText);
                    curEnemy.locked = true; noLocked = false; curEnemyBall.tag = "LockedEnemy";
                    shoot(curEnemyBall);
                    curText = "<color=#ffa500ff>" + curText[0].ToString() + "</color>" + curText.Substring(1); 
                    curEnemy.curShot = curText.LastIndexOf('>') + 1;
                    curTriggerWord.GetComponent<Text>().text = curText;
                    //print("Waitng for: " + curText[curEnemy.curShot]);
                    waveTotalPress += 1f;
                    waveCorrectPress += 1f;
                }
            }
            else if (pressedKey != "space" && noLocked == false && curEnemy.locked == true)
            {
                waveTotalPress += 1f;
                if (curEnemy.curShot < curText.Length && pressedKey[0] == curText[curEnemy.curShot])
                {
                    shoot(curEnemyBall);
                    curText = curText.Replace("</color>" + curText[curEnemy.curShot].ToString(), "</color><color=#ffa500ff>" + curText[curEnemy.curShot].ToString() + "</color>");
                    curEnemy.TriggerWord.GetComponent<Text>().text = curText;

                    curEnemy.curShot = curText.LastIndexOf('>') + 1;
                    waveCorrectPress += 1f;
                }
            }

            if (shipTransform != null && Vector3.Distance(curEnemy.EnemyBall.transform.position, shipTransform.position) < 0.8f)
            {
                PlayerDie();
            }
            if (curText[curText.Length - 1] == '>')
            {
                destroyEnemy(curEnemy);
            }
            enemyQueue.Enqueue(curEnemy);
        }
    }

    public void NewWave()
    {
        if (curWave != triggerWords.Count)
        {
            CalculateBonus();
            curWave++;
            wordIndex = 0;
            waveGT.text = "<color=#ffa500ff>Wave: " + curWave.ToString()+ "</color>";
            waveTotalEnemy = triggerWords[curWave - 1].Count;
            waveEnemyKilled = 0;
            Invoke("ChangeWaveTextColor", 0.5f);
        }
        else
        {
            CalculateBonus();
            Invoke("LoadEndScene", 3f);
        }
    }

    public void LoadEndScene()
    {
        SceneManager.LoadScene("_End_Scene");
    }

    public void CalculateBonus()
    {
        GameObject endWaveGO = Instantiate(EndWaveTextPrefab);
        endWaveGO.transform.SetParent(GameObject.Find("Canvas").transform, false);
        float accuracy = Mathf.Floor((waveCorrectPress / waveTotalPress) * 100f);
        endWaveGO.GetComponent<Text>().text = "Accuracy: " + accuracy.ToString() + "%";

        if (accuracy >= 95f)
        {
            endWaveGO.GetComponent<Text>().text += "\n<size=20>Accuracy bonus: " + accuracy.ToString() + " points</size>";
            UpdateScore((int)accuracy);
        }
        else
        {
            endWaveGO.GetComponent<Text>().text += "\n<size=20>Less than 95%, no bonus</size>";
        }
        waveTotalPress = 0f; waveCorrectPress = 0f;

    }

    void ChangeWaveTextColor()
    {
        Invoke("ResetWaveCounters", 1.5f);
        waveGT.text = "Wave: " + curWave.ToString();
    }
    void ResetWaveCounters()
    {
        waveEnemySpawned = 0;
        //waveEnemyKilled = 0;
    }

    public void destroyEnemy(Enemy curEnemy)
    {
        if (curEnemy.locked == true)
        {
            curEnemy.EnemyBall.tag = "Dead";
            curEnemy.TriggerWord.tag = "Dead";
            curEnemy.locked = false;

        }
        //else if (curEnemy.EnemyBall.tag != "Dead")
        //{
        //    PlayerDie();
        //}
    }

    public void PlayerDie()
    {
        Instantiate(PlayerExplosionPrefab, shipTransform.position, shipTransform.rotation);
        Destroy(shipGO);
        Invoke("LoadEndScence", 2f);
    }

    public void LoadEndScence()
    {
        SceneManager.LoadScene("_End_Scene");
    }

    public bool isEndWaveArray()
    {
        if (wordIndex < triggerWords[curWave - 1].Count)
            return false;
        return true;
    }

    public void createRandomEnemy(int type)
    {
        waveEnemySpawned++;
        // Creating an enemy ship
        GameObject newEnemyBall; 
        switch(type)
        {
            case 1: newEnemyBall = Instantiate(Enemy1Prefab);
                break;
            case 2:
                newEnemyBall = Instantiate(Enemy2Prefab);
                break;
            case 3:

                int ranNum = Random.Range(1, 4);
                if (ranNum == 1) newEnemyBall = Instantiate(Enemy3aPrefab);
                else if (ranNum == 2) newEnemyBall = Instantiate(Enemy3bPrefab);
                else newEnemyBall = Instantiate(Enemy3cPrefab);
                newEnemyBall.transform.rotation = Random.rotation;
                break;
            default:
                newEnemyBall = Instantiate(Enemy1Prefab);
                break;
        }

        float x = Random.Range(-8f, 8f);
        Vector3 pos = newEnemyBall.transform.position;
        pos.x = x;
        pos.y = topY;
        newEnemyBall.transform.position = pos;

        // Creating trigger word
        GameObject newTriggerWord = Instantiate(TriggerWordPrefab);
        newTriggerWord.transform.SetParent(GameObject.Find("Canvas").transform, false);
        newTriggerWord.GetComponent<Text>().text = triggerWords[curWave-1][wordIndex];
        newEnemyBall.GetComponent<Text>().text = triggerWords[curWave-1][wordIndex].Length.ToString();
        wordIndex++;
        if (wordIndex == triggerWords[curWave-1].Count)
        {
            //wordIndex = 0;
        }
        pos.x += 4.0f;
        Vector3 wordPos = Camera.main.WorldToScreenPoint(pos);
        newTriggerWord.transform.position = wordPos;
        enemyQueue.Enqueue(new Enemy(newEnemyBall, newTriggerWord));
    }

    public void shoot(GameObject target)
    {
        Vector3 pos = shipTransform.position;
        pos.z = -1;
        Vector3 bulletDirection = target.transform.position - pos;
        Instantiate(BulletPrefab, pos, Quaternion.Euler(0, 0, 0));
    }

    public string DetectPressedKeyOrButton()
    {
        if (lastCheckKeyTime + checkInterval < Time.time)
        {
            lastCheckKeyTime = Time.time;
            foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                    return kcode.ToString();
            }
        }
        return KeyCode.Space.ToString();
    }

    public void UpdateScore(int addition)
    {
        score += addition;
        scoreGT.text = "Score: " + score.ToString();
        ScoreTextHandler.score = score;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);

            //HighScoreHandler.highScore = score;
            
        }
    }
}
