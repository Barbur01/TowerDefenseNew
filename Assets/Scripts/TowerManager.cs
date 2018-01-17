using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager
{
    static string TOWER_PATH_BASE = "Towers/";
    static string[] TOWER_PATH = { "Basic/" };

    Dictionary<Tower.Type, List<Object>> m_TowerTemplates;
    List<Tower> m_Towers;

    public TowerManager()
    {
        m_TowerTemplates = new Dictionary<Tower.Type, List<Object>>();
        m_Towers = new List<Tower>();

        LoadTowers();
    }

    ~TowerManager()
    {
        for (int i = 0; i < m_Towers.Count; ++i)
        {
            m_Towers[i].Destroy();
        }

        m_Towers.Clear();
        m_TowerTemplates.Clear();
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
    }

    public Tower GetTower(GameObject towerObject)
    {
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

        if (found)
        {
            tower = new Tower(Tower.Type.BASIC);

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
        m_Towers.Add(tower);
    }

    public void DestroyTower(Tower tower)
    {
        tower.Destroy();
    }

    public int GetCost(Tower.Type type)
    {
        List<Object> towerData;
        bool found = m_TowerTemplates.TryGetValue(type, out towerData);

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
