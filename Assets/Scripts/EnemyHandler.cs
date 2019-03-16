using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHandler : MonoBehaviour
{
    public List<float> speed;
    public Transform shipTransform;
    EnemySpawner esScript;

    public GameObject enemyExplosionPrefab;
    public GameObject floatingTextPrefab;

    void Start()
    {
        speed = new List<float>
        {
            1.1f,1.1f,1.5f,1.1f,1.6f,1.1f,1.1f,1.1f,1.1f,1.8f,1.1f,1.1f,1.1f,1.1f,2.0f,2.2f,1.1f,1.1f,1.1f,1.1f,1.1f
        };
        esScript = Camera.main.GetComponent<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Dead")
        {
            if (this.gameObject.GetComponent<Text>().text == "0")
            {
                GameObject[] deadArray = GameObject.FindGameObjectsWithTag("Dead");
                foreach (GameObject dead in deadArray)
                {
                    if (Regex.Match(dead.name, "^Enemy\\d", RegexOptions.IgnoreCase).Success) {
                        esScript.waveEnemyKilled++;
                        Instantiate(enemyExplosionPrefab, dead.transform.position, dead.transform.rotation);
                        esScript.noLocked = true;
                        showFloatingScore();
                        esScript.UpdateScore(100);

                        if (esScript.waveEnemyKilled == esScript.waveTotalEnemy)
                        {
                            esScript.NewWave();
                        }
                    }
                    Destroy(dead);

                    //Finally dequeue it from the queue
                    for (int i = 0; i < esScript.enemyQueue.Count; i++)
                    {
                        Enemy curEnemy = esScript.enemyQueue.Dequeue();
                        if (!curEnemy.TriggerWord.Equals(dead))
                        {
                            esScript.enemyQueue.Enqueue(curEnemy);
                        }
                    }
                }
            }
        }

        GameObject shipGO = GameObject.Find("Ship");
        if (shipGO != null)
        {
            shipTransform = shipGO.transform;
            // Move our position a step closer to the target.
            float step = speed[esScript.curWave - 1] * Time.deltaTime; // calculate distance to move

            transform.position = Vector3.MoveTowards(transform.position, shipTransform.position, step);
        }
    }

    void showFloatingScore()
    {
        GameObject textGO = Instantiate(floatingTextPrefab);
        textGO.transform.SetParent(GameObject.Find("Canvas").transform, false);
        Vector3 pos = transform.position;
        //pos.x += 2f;
        Vector3 wordPos = Camera.main.WorldToScreenPoint(pos);
        textGO.transform.position = wordPos;

        textGO.GetComponent<Text>().text = "100";
    }
}
