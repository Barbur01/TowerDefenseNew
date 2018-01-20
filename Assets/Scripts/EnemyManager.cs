using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyManager
{
    static string ENEMY_PATH = "Enemies/";
    List<Object> m_EnemyTemplates = null;
    List<GameObject> m_Enemies;

    List<CreepSpawner> m_Spawners;

    public void Init ()
    {
        m_Spawners = new List<CreepSpawner>();
        m_Enemies = new List<GameObject>();
        LoadEnemies();
        SetupSpawners();
    }

    public void Reset()
    {
        DestroyEnemyInstances();

        foreach (CreepSpawner item in m_Spawners)
        {
            item.Reset();
        }
    }

    void DestroyEnemyInstances()
    {
        for (int i = 0; i < m_Enemies.Count; ++i)
        {
            GameObject.Destroy(m_Enemies[i]);
        }

        m_Enemies.Clear();
    }

    void SetupSpawners()
    {
        CreepSpawner[] spawners = GameObject.FindObjectsOfType<CreepSpawner>();

        Assert.IsTrue(spawners.Length > 0);

        foreach (CreepSpawner item in spawners)
        {
            m_Spawners.Add(item);
            item.SetEnemyManager(this);
        }
    }

    void LoadEnemies()
    {
        Object[] allTowerLevels = Resources.LoadAll(ENEMY_PATH) as Object[];

        if (allTowerLevels != null)
        {
            m_EnemyTemplates = new List<Object>(allTowerLevels);
        }

        Assert.IsTrue(m_EnemyTemplates.Count > 0);
    }

    int GetEnemyIndexFromDifficulty(int difficulty)
    {
        difficulty = Mathf.Clamp(difficulty, 0, m_EnemyTemplates.Count - 1);

        return Random.Range(0, difficulty + 1);
    }

    public GameObject CreateRandomEnemy(Vector3 pos, Transform parent, int difficulty)
    {
        GameObject enemy = null;

        if (m_EnemyTemplates.Count > 0)
        {
            int random = GetEnemyIndexFromDifficulty(difficulty);

            Debug.Log("Picking enemy wity index  " + random);
            enemy = GameObject.Instantiate(m_EnemyTemplates[random] as GameObject, pos, Quaternion.identity, parent);

            m_Enemies.Add(enemy);
        }

        return enemy;
    }
}
