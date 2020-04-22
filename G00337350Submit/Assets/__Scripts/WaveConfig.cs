using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{

    [SerializeField] private Sprite enemySprite;
    [SerializeField] private int enemiesPerWave = 10;
    [SerializeField] private float enemySpeed = 3.5f;
    [SerializeField] private float spawnInterval = 0.45f;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private int damageValue = 5;


    //== Public Methods ==
    public int GetEnemiesPerWave() { return enemiesPerWave; }
    public float GetEnemySpeed() { return enemySpeed; }
    public float GetSpawnInterval() { return spawnInterval; }
    public int GetScoreValue() { return scoreValue; }
    public int GetDamageValue() { return damageValue; }
    public Sprite GetEnemySprite() { return enemySprite; }
}
