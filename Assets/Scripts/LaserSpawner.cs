using System;
using System.Collections;
using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    [SerializeField] private Laser laserPrefab;
    [SerializeField] private Transform hallStart;
    [SerializeField] private float spawnInterval = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private IEnumerator Start()
    {
        var wait = new WaitForSeconds(spawnInterval);
        while (true)
        {
            bool vertical = false;

            if (UnityEngine.Random.value < 0.5f)
            {
                vertical = true;
            }


            float randomHeight = UnityEngine.Random.Range(1.0f, 2.5f);

            Vector3 spawnPosition = new Vector3(transform.position.x, randomHeight, transform.position.z);
            Laser laser = Instantiate(laserPrefab, spawnPosition, transform.rotation);

            if (vertical)
            {
                laser.transform.Rotate(0f, 0f, 90f);
            }
            Vector3 target = new Vector3(hallStart.position.x, randomHeight, hallStart.position.z);
            laser.Init(target, randomHeight);

            yield return wait;
        }
    }
}
