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
        m_Enemies = new List<GameObject>();
        LoadEnemies();
        SetupSpawners();
    }

    public void Reset()
    {
        DestroyEnemyInstances();
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

    public GameObject CreateRandomEnemy(Vector3 pos, Transform parent)
    {
        GameObject enemy = null;

        if (m_EnemyTemplates.Count > 0)
        {
            int random = Random.Range(0, m_EnemyTemplates.Count);
            Debug.Log("Picking enemy wity index  " + random);
            enemy = GameObject.Instantiate(m_EnemyTemplates[random] as GameObject, pos, Quaternion.identity, parent);

            m_Enemies.Add(enemy);
        }

        return enemy;
    }
}
