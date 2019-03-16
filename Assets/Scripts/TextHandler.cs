using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextHandler : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        EnemySpawner esScript = Camera.main.GetComponent<EnemySpawner>();
        
        Enemy[] list = esScript.enemyQueue.ToArray();

        if (list.Length > 0)
        {
            GameObject ball;
            int i = 0;
            //print("THIS: " + this.GetComponent<Text>().text + " --- " + this.tag);

            while (i < list.Length - 1 && !list[i].TriggerWord.Equals(this.gameObject))
            {
                //print("COMPARE " + i + ": " + list[i].TriggerWord.GetComponent<Text>().text + " --- " + list[i].TriggerWord.tag);

                i++;
            }
            //print("COMPARE " + i + ": " + list[i].TriggerWord.GetComponent<Text>().text + " --- " + list[i].TriggerWord.tag);
            if (list[i].TriggerWord.Equals(this.gameObject))
            {
                ball = list[i].EnemyBall;
                Vector3 pos = ball.transform.position;
                pos.x += 4.0f;
                Vector3 wordPos = Camera.main.WorldToScreenPoint(pos);
                transform.position = wordPos;
            }
        }
    }
}
