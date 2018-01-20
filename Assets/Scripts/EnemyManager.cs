using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    static string ENEMY_PATH = "Enemies/";
    List<Object> m_EnemyTemplates = null;
    List<GameObject> m_Enemies;

    private static EnemyManager instance;
    private EnemyManager() { }
    public static EnemyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EnemyManager();               
            }
            return instance;
        }
    }

    // Use this for initialization
    public void Init ()
    {
        m_Enemies = new List<GameObject>();
        LoadEnemies();
	}

    public void Destroy()
    {
        DestroyEnemyInstances();
        m_EnemyTemplates.Clear();
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

    bool LoadEnemies()
    {
        Object[] allTowerLevels = Resources.LoadAll(ENEMY_PATH) as Object[];

        if (allTowerLevels != null)
        {
            m_EnemyTemplates = new List<Object>(allTowerLevels);
        }

        return m_EnemyTemplates.Count > 0;
    }

    public GameObject CreateRandomEnemy(Vector3 pos, Transform parent)
    {
        int random = Random.Range(0, m_EnemyTemplates.Count);
        Debug.Log("Index pickj enemy is " + random);
        GameObject enemy = GameObject.Instantiate(m_EnemyTemplates[random] as GameObject, pos, Quaternion.identity, parent);

        m_Enemies.Add(enemy);

        return enemy;
    }
}
