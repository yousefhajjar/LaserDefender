using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigsList;

    int startingWave = 0;

    // Start is called before the first frame update
    void Start()
    {
        var currentWave = waveConfigsList[startingWave];
        StartCoroutine(SpawnAllEnemiesInWave(currentWave));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveToSpawn)
    {
        for (int enemyCount = 1; enemyCount <= waveToSpawn.GetNumberofEnemies(); enemyCount++)
        {
            //spawn enemy from waveToSpawn
            //at the position specified by the waypoint(0) of waveToSpawn
            Instantiate(
                waveToSpawn.GetEnemyPrefab(),
                waveToSpawn.GetWaypoints()[0].transform.position,
                Quaternion.identity);

            yield return new WaitForSeconds(waveToSpawn.GetTimeBetweenSpawns());
        }
    }
}
