using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager
{
    List<Tower> m_Towers = new List<Tower>();

    Tower m_TowerReadyToPlace = null;

    public Tower PrepareNewTower(Tower.Type type)
    {
        Tower tower = TowerFactory.CreateTower(type);
        tower.PrepareToPlace();

        return tower;
    }

    public void PlacePreparedTower()
    {
        if (m_TowerReadyToPlace != null)
        {
            m_Towers.Add(m_TowerReadyToPlace);
            m_TowerReadyToPlace = null;
        }
    }

    public void DestroyTower(Tower tower)
    {
        tower.Destroy();
    }

    public void Update()
    {
        foreach (Tower tower in m_Towers)
        {
            tower.Update();
        }
    }
}
