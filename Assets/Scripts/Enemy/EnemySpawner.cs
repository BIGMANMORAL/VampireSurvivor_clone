using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups; //List of enemy groups in this wave
        public int waveQuata;        //The total number of enemies to spawn in this wave
        public float spawnInterval; // The interval at which to spawn enemies
        public int spawnCount;     // The number of enemies already spawned in this wave
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        public int spawnCount; // The number of enemies already spawned in this group
        public GameObject enemyPrefab;
    }

    public List<Wave> waves; //List of waves to spawn

    public int currentWaveCount; // The index of the current wave [A list starts from 0] 

    [Header("Spawner Attributes")]
    float spawnTimer; // Timer to track spawn intervals
    public int enemiesAlive;
    public int maxEnemiesAllowed; // The maximum number of enemies allowed on the map at once
    public bool maxEnemiesReached = false;// A flag indicating if the maximum number of enemies has been reached
    public float waveInterval; // Interval between waves

    [Header("Spawn Points")]
    public List<Transform> relativeSpawnPoints; // A list that stores all the relative spawn point of enemies

    Transform player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindAnyObjectByType<PlayerStats>().transform;
        CalculateWaveQuota();
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0) //check if the wave has ended and the next wave should start
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer += Time.deltaTime;

        //check if it's time to spawn the next enemy
        if(spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }


    IEnumerator BeginNextWave()
    {
        //Wave for 'waveInterval' seconds before starting the next wave
        yield return new WaitForSeconds(waveInterval);

        // Iof there are more waves to start after the current wave, move on to the next wave
        if(currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach(var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuata = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }


    /// <summary>
    /// This method will stop spawning enemies if the amount of enemies on the map is maximum
    /// The will only spawn enemies in a particular wave untill it is time for the next wave's enemies to be spawned
    /// </summary>
    void SpawnEnemies()
    {
        //Check if the minimum number of enemies in the wave have been spawned
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuata && !maxEnemiesReached)
        {
            //spawn each type of enemy until the quota s filled
            foreach(var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                //check if the minimum numbe of enemies of this type have been spawned
                if(enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    if(enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }

                    //spawns the enemies at random position near the player
                    Instantiate(enemyGroup.enemyPrefab,player.position + relativeSpawnPoints[Random.Range(0,relativeSpawnPoints.Count)].position,Quaternion.identity);
                   
                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }

        //Resets the maxEnemiesReached flag if the number of enemies alive has dropped below the maximum amount
        if(enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }


    //call this function when an enemy is killed
    public void OnEnemyKilled()
    {
        //decrement the number of enemies alive
        enemiesAlive--;
    }
}
