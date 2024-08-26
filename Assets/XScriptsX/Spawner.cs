using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public bool devMode;

    public Wave[] waves;
    public Enemy enemy;

    LivingEntity playerEntity;
    Transform playerT;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    MapGenerator map;

    float timeBetweenCampingChecks = 2;
    float campThresholdDistance = 1.5f;
    float nextCampCheckTime;
    Vector3 campPositionOld;
    bool isCamping;

    bool isDisabled;

    public event System.Action<int> OnNewWave;

    private void Start() {
        playerEntity = FindObjectOfType<Player>();
        playerT = playerEntity.transform;

        nextCampCheckTime = timeBetweenCampingChecks + Time.time;
        campPositionOld = playerT.position;
        playerEntity.OnDeath += OnPlayerDeath;

        map = FindObjectOfType<MapGenerator>();
        NextWave();
    }

    private void Update() {
        if (!isDisabled) {
            if (Time.time > nextCampCheckTime) {
                nextCampCheckTime = Time.time + timeBetweenCampingChecks;

                isCamping = (Vector3.Distance(playerT.position, campPositionOld) < campThresholdDistance);
                campPositionOld = playerT.position - campPositionOld;
            }

            if ((enemiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > nextSpawnTime) {     //if there are enemies still to be spawned and the value of time is larger than the next spawn time then...
                enemiesRemainingToSpawn--;                                      //reduce number of enemies remaining to spawn by 1
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;      //the next spawn time becomes the current time plus the interval between spawns (i.e timeBetweenSpawns).

                StartCoroutine("SpawnEnemy");
            }
        }
        if (devMode) {
            if (Input.GetKeyDown(KeyCode.Return)){
                StopCoroutine("SpawnEnemy");
               foreach (Enemy enemy in FindObjectsOfType<Enemy>()) {
                    Destroy(enemy.gameObject);
                }
                NextWave();
            }
        }
    }

    IEnumerator SpawnEnemy() {
        float spawnDelay = 1;
        float tileFlashSpeed = 4;

        Transform spawnTile = map.GetRandomOpenTile();
        if (isCamping) {
            spawnTile = map.GetTileFromPosition(playerT.position);
        }
        Material tileMat = spawnTile.GetComponent<Renderer>().material;
        Color initialColour = Color.white;
        Color flashColour = Color.red;
        float spawnTimer = 0;

        while (spawnTimer < spawnDelay) {

            tileMat.color = Color.Lerp(initialColour, flashColour, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));

            spawnTimer += Time.deltaTime;
            yield return null;
        }

        Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;    //spawn Enemy
        spawnedEnemy.OnDeath += OnEnemyDeath;                                                   //when an Enemy dies (livingEntity), OnEnemyDeath is called
        spawnedEnemy.SetCharacteristics(currentWave.moveSpeed, currentWave.hitsToKillPlayer, currentWave.enemyHealth, currentWave.skinColour);
    }

    void OnPlayerDeath() {
        isDisabled = true;

    }

    void OnEnemyDeath() {
        enemiesRemainingAlive--;    //when an enemy dies it reduces the number of enemies remaining alive

        if(enemiesRemainingAlive == 0) {    //when no enemies are alive, the next wave begins
            NextWave();
        }
    }

    void ResetPlayerPosition() {
        playerT.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up * 3;
    }

    void NextWave() {
        if (currentWaveNumber > 0) {
            AudioManager.instance.PlaySound2D("Level Completed");
        }
        currentWaveNumber++;                                    //current wave number is increased by 1
        
        if (currentWaveNumber - 1 < waves.Length) {             //checking that wave number does not exceed array length
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;   //setting number of enemies to spawn, to number specified in the current wave, which is 5
            enemiesRemainingAlive = enemiesRemainingToSpawn;    //setting number of enemies alive, to the number of enemies to spawn, which is 5
            
            if (OnNewWave != null) {
                OnNewWave(currentWaveNumber);
            }
            ResetPlayerPosition();
        }
    }

    [System.Serializable]
    public class Wave {
        public bool infinite;
        public int enemyCount;
        public float timeBetweenSpawns;

        public float moveSpeed;
        public int hitsToKillPlayer;
        public float enemyHealth;
        public Color skinColour;
    }

}
