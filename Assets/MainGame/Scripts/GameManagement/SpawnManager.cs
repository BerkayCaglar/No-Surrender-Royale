using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager _Instance { get; private set; }
    [SerializeField] private List<GameObject> _spawnObjects = new List<GameObject>();

    public List<GameObject> SpawnObjects
    {
        get => _spawnObjects;
    }

    [SerializeField] private float _spawnRate = 1f;

    private void Awake()
    {
        if (_Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _Instance = this;
        }
    }
    /// <summary>
    /// This method is called from UIManager.cs to start the spawn coroutine. It is called after the download is complete.
    /// </summary>
    public void StartSpawnCoroutine()
    {
        // Start the spawn coroutine
        StartCoroutine(SpawnObject());
    }

    /// <summary>
    /// This method is spawns the objects at random positions and random intervals. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnObject()
    {
        // This loop will run forever
        while (true)
        {
            // Wait for the spawn rate
            yield return new WaitForSeconds(_spawnRate);

            // Get a random index from the spawn objects list
            int randomIndex = Random.Range(0, _spawnObjects.Count);

            // Get a random position
            Vector3 randomPosition = new Vector3(Random.Range(-8.5f, 8.5f), 0f, Random.Range(3f, 14.5f));

            // Instantiate the object at the random position
            Instantiate(_spawnObjects[randomIndex], randomPosition, Quaternion.Euler(0f, -180f, 0f));
        }
    }
    /// <summary>
    /// This method is Increases the spawn rate of the objects when the countdown timer is multiple of 15.
    /// </summary>
    public void IncereaseSpawnRate()
    {
        // Decrease the spawn rate by 0.2f
        _spawnRate -= 0.2f;
    }
}
