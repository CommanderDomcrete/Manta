using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Wave[] waves;

    PlayerManager player;
    Transform playerT;

    Wave currentWave;
    [SerializeField] int currentWaveNumber;

    int score;
    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;
    bool isDisabled;
    float waitTime = 5f;
    public bool gameOver;

    public GameObject enemyPrefab;
    public float spawnRange = 500;
    Vector3 spawnPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnSceneChange;

        instance.enabled = false;

        gameOver = false;
        player = FindObjectOfType<PlayerManager>();
        playerT = player.transform;
        player.OnDeath += GameOver;
        score = 0;
        PlayerUIManager.instance.playerUIHUDManager.SetNewScoreValue(score);
        NextWave();
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
        }
        else
        {
            instance.enabled = false;
        }
    }

    private void Update()
    {
        spawnPosition = new Vector3(Random.Range(-spawnRange, spawnRange), 600, Random.Range(-spawnRange, spawnRange));
        if (!isDisabled)
        {

            if ((enemiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > nextSpawnTime)
            { 
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
                SpawnEnemy();
            }
        }
    }

    public void SpawnEnemy()
    {
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        spawnedEnemy.GetComponent<CharacterManager>().OnDeath += OnEnemyDeath;
    }
    void OnEnemyDeath()
    {
        score++;
        enemiesRemainingAlive--;
        PlayerUIManager.instance.playerUIHUDManager.SetNewScoreValue(score);
        if (enemiesRemainingAlive == 0)
        {    //when no enemies are alive, the next wave begins
            NextWave();
        }
    }
    void ResetPlayerPosition()
    {
        if(playerT != null)
        {
            playerT.position = Vector3.zero;
        }
    }

    void NextWave()
    {
        if (currentWaveNumber > 0)
        {
            AudioManager.instance.PlaySound2D("Level Completed");
        }
        currentWaveNumber++;                                    //current wave number is increased by 1

        if (currentWaveNumber - 1 < waves.Length)
        {             //checking that wave number does not exceed array length
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;   //setting number of enemies to spawn, to number specified in the current wave, which is 5
            enemiesRemainingAlive = enemiesRemainingToSpawn;    //setting number of enemies alive, to the number of enemies to spawn, which is 5
        }
    }

    public void GameOver()
    {
        gameOver = true;
        StartCoroutine(GameEnding());
    }

    public IEnumerator GameEnding()
    {
        yield return new WaitForSeconds(waitTime);
        ResetPlayerPosition();
        StartCoroutine(WorldSaveGameManager.instance.Restart());

        currentWaveNumber = 0;
        gameOver = false;
        player.OnDeath += GameOver;
        score = 0;
        PlayerUIManager.instance.playerUIHUDManager.SetNewScoreValue(score);
        NextWave();
    }

    [System.Serializable]
    public class Wave
    {
        public bool infinite;
        public int enemyCount;
        public float timeBetweenSpawns;

        public float moveSpeed;
        public int hitsToKillPlayer;
        public float enemyHealth;
        public Color skinColour;
    }
}
