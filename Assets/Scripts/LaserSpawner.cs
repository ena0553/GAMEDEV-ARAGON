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
            float randomHeight = Random.Range(1.0f, 2.5f);

            Vector3 spawnPosition = new Vector3(transform.position.x, randomHeight, transform.position.z);
            Laser laser = Instantiate(laserPrefab, spawnPosition, transform.rotation);
            laser.Init(hallStart.position, randomHeight);
            yield return wait;
        }
    }
}
