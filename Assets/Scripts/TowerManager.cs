using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;


public class TowerManager
{
    static string TOWER_PATH_BASE = "Towers/";
    static string[] TOWER_PATH = { "Basic/" };

    Dictionary<Tower.Type, List<Object>> m_TowerTemplates;
    List<Tower> m_Towers;

    public void Init()
    {
        m_TowerTemplates = new Dictionary<Tower.Type, List<Object>>();
        m_Towers = new List<Tower>();

        LoadTowers();
    }

    public void Reset()
    {
        DestroyTowerInstances();
    }

    void DestroyTowerInstances()
    {
        for (int i = 0; i < m_Towers.Count; ++i)
        {
            m_Towers[i].Destroy();
        }

        m_Towers.Clear();
    }

    void LoadTowers()
    {
        for (int i = 0; i < (int)Tower.Type.COUNT; ++i)
        {
            string fullpath = TOWER_PATH_BASE + TOWER_PATH[i];
            Object[] allTowerLevels = Resources.LoadAll(fullpath) as Object[];

            if (allTowerLevels != null)
            {
                m_TowerTemplates.Add((Tower.Type)i, new List<Object>(allTowerLevels));
            }
        }

        Assert.IsTrue(m_TowerTemplates.Count > 0);
    }

    public Tower GetTower(GameObject towerObject)
    {
        Assert.IsTrue(towerObject != null);

        foreach (Tower tower in m_Towers)
        {
            if (tower.IsOwnerOf(towerObject))
            {
                return tower;
            }
        }

        return null;
    }

    public Tower PrepareNewTower(Tower.Type type)
    {
        Tower tower = null;

        List<Object> towerTemplates;
        bool found = m_TowerTemplates.TryGetValue(type, out towerTemplates);
        Assert.IsTrue(found);

        if (found)
        {
            tower = new Tower();
            tower.Init();

            foreach (GameObject towerObject in towerTemplates)
            {
                TowerLevel newLevel = towerObject.GetComponent<TowerLevel>();
                tower.AddLevel(newLevel);
            }

            tower.PrepareToPlace();
        }

        return tower;
    }

    public void AddTower(Tower tower)
    {
        Assert.IsTrue(tower != null);

        if (tower != null)
        {
            m_Towers.Add(tower);
        }
    }

    public void DestroyTower(Tower tower)
    {
        Assert.IsTrue(tower != null);

        if (tower != null)
        {
            tower.Destroy();
        }
    }

    public int GetCost(Tower.Type type)
    {
        List<Object> towerData;
        bool found = m_TowerTemplates.TryGetValue(type, out towerData);
        Assert.IsTrue(found);

        if (found)
        {
            GameObject towerObject = towerData[0] as GameObject;

            return towerObject.GetComponent<TowerLevel>().m_Data.m_Cost;
        }

        return int.MaxValue;
    }

    public void Update()
    {
        foreach (Tower tower in m_Towers)
        {
            tower.Update();
        }
    }
}
