using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;    // text mesh pro library

public class GameController : MonoBehaviour
{
    // == public fields ==
    public int StartingLives { get { return startingLives; } }

    public int RemainingLives { get { return remainingLives; } }

    // == private fields ==
    private int playerScore = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int startingLives = 3;
    
    [SerializeField] private List<WaveConfig> waveConfigList;
    private int startingWave = 0;
    private int waveNumber = 0;
    private int remainingLives = 0;
    private int enemyRemainingCount;
    


    // for the enemy waves
   // [SerializeField] private WaveConfig waveConfig;
    [SerializeField] private int enemyCountPerWave = 20;
    [SerializeField] private TextMeshProUGUI remainingEnemyText;
    private int remainingEnemyCount;
    
    // audio sound to indicate "between wave" moment
    [SerializeField] private AudioClip waveReadySound;
    private SoundController sc;

    // == private methods ==
    private void Awake()
    {
        SetupSingleton();
    }
    private void Start()
    {
        UpdateScore();
        remainingLives = startingLives;
        // set up for enemy waves
        sc = SoundController.FindSoundController();
        StartCoroutine(SpawnAllWaves());
    }

    private void SetupSingleton()
    {
        // this method gets called on creation
        // check for any other objects of the same type
        // if there is one, then use that one.
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);    // destroy the current object
        }
        else
        {
            // keep the new one
            DontDestroyOnLoad(gameObject);  // persist across scenes
        }
    }

    private void OnEnable()
    {
        Enemy.EnemyKilledEvent += OnEnemyKilledEvent;
        PointSpawners.EnemySpawnedEvent += PointSpawners_OnEnemySpawnedEvent;
    }

    private void OnDisable()
    {
        Enemy.EnemyKilledEvent -= OnEnemyKilledEvent;
        PointSpawners.EnemySpawnedEvent -= PointSpawners_OnEnemySpawnedEvent;
    }

    //Set up method that will spawn waves
    private IEnumerator SpawnAllWaves()
    {
        //Use to loop through list
        for(int waveIndex = startingWave; waveIndex < waveConfigList.Count; waveIndex++)
        {
            var waveConfig = waveConfigList[waveIndex];
            yield return StartCoroutine(SetupNextWave(waveConfig));
        }
    }

    private void PointSpawners_OnEnemySpawnedEvent()
    {
        remainingEnemyCount--;
        UpdateEnemyRemainingText();
        if(remainingEnemyCount == 0)
        {
            // stop the point spawner from spawning
            DisableSpawning();
        }
    }

    private IEnumerator SetupNextWave(WaveConfig currentWave)
    {
        yield return new WaitForSeconds(10.3f);
        sc.PlayOneShot(waveReadySound);
        FindObjectOfType<PointSpawners>().SetWaveConfig(currentWave);
        remainingEnemyCount = currentWave.GetEnemiesPerWave();
        EnableSpawning();
    }

    private void DisableSpawning()
    {
        // find each PointSpawner, call a public method to disable spawning
        foreach(var spawner in FindObjectsOfType<PointSpawners>())
        {
            spawner.DisableSpawning();
        }
    }

    private void EnableSpawning()
    {
        // find each PointSpawner, call a public method to disable spawning
        foreach (var spawner in FindObjectsOfType<PointSpawners>())
        {
            spawner.EnableSpawning();
        }
    }

    private void OnEnemyKilledEvent(Enemy enemy)
    {
        // add the score value for the enemy to the player score
        playerScore += enemy.ScoreValue;
        UpdateScore();
    }

    private void UpdateEnemyRemainingText()
    {
        remainingEnemyText.text = remainingEnemyCount.ToString("0");
    }

    private void UpdateScore()
    {
        //Debug.Log("Score: " + playerScore);
        // display on screen
        scoreText.text = playerScore.ToString();
    }

    public void LoseOneLife()
    {
        remainingLives--;
    }

}
