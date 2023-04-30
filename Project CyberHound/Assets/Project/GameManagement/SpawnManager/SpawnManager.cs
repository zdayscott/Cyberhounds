using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private List<SpawnPoint> _spawnPoints;
    [SerializeField] private EnemyManifest manifest;
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnPoints = new List<SpawnPoint>( FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None));
        OnStartWave();
    }

    public void OnStartWave()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        do
        {
            var enemy = manifest.enemies.GetRandom();
            var spawnPoint = _spawnPoints.GetRandom();

            var go = Instantiate(enemy, spawnPoint.GetPoint(), quaternion.identity);
            go.Initialize(spawnPoint);

            yield return new WaitForSeconds(1);
        } while (true);
    }
}