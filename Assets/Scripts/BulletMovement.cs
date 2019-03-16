using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletMovement : MonoBehaviour
{
    public AudioSource fireSound;
    public float bulletSpeed;
    public GameObject target;

    void Start()
    {
        fireSound.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collideWith = collision.gameObject;
        if (collideWith.tag == "LockedEnemy" || collideWith.tag == "Dead")
        {
            collideWith.GetComponent<Text>().text = (System.Int32.Parse(collideWith.GetComponent<Text>().text) - 1).ToString();
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Dead") != null)
        {
            target = GameObject.FindGameObjectWithTag("Dead");
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("LockedEnemy");
        }

        //target = GameObject.FindGameObjectWithTag("LockedEnemy");

        // Move our position a step closer to the target.
        float step = bulletSpeed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }
}
