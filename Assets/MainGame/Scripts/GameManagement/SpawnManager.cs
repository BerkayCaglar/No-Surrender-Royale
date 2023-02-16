using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _spawnObjects = new List<GameObject>();

    public List<GameObject> SpawnObjects
    {
        get => _spawnObjects;
    }

    [SerializeField] private float _spawnRate = 1f;

    private void Start()
    {
        StartCoroutine(SpawnObject());
    }

    private IEnumerator SpawnObject()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnRate);

            int randomIndex = Random.Range(0, _spawnObjects.Count);

            Vector3 randomPosition = new Vector3(Random.Range(-8.5f, 8.5f), 0f, Random.Range(3f, 14.5f));

            Instantiate(_spawnObjects[randomIndex], randomPosition, Quaternion.Euler(0f, -180f, 0f));
        }
    }
}
