using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script attaches to ‘VisualEffect’ objects. It destroys or deactivates them after the defined time.
/// </summary>
public class EnemyExplosionEffect : MonoBehaviour
{

    [Tooltip("the time after object will be destroyed")]

    public AudioSource enemyExplosionSound;
    public float destructionTime;

    private void OnEnable()
    {
        StartCoroutine(Destruction()); //launching the timer of destruction
    }

    IEnumerator Destruction() //wait for the estimated time, and destroying or deactivating the object
    {
        enemyExplosionSound.Play();
        yield return new WaitForSeconds(destructionTime);
        Destroy(gameObject);
    }
}
