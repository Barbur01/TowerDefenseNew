using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower
{ 
    List<TowerLevel> m_TowerLevels = new List<TowerLevel>();
    int m_CurrentLevelIndex = 0;
    TowerLevel m_CurrentLevel = null;

    Vector3 m_Position;

    public enum Type
    {
        BASIC = 0,
        INVALID
    };

    Type m_Type = Type.INVALID;

    public Tower(Type type)
    {
        m_Type = type;
    }

    public void Destroy()
    {
        if (m_CurrentLevel)
        {
            GameObject.Destroy(m_CurrentLevel);
        }
    }

    public int GetCost()
    {
        if (m_CurrentLevel)
        {
            return m_CurrentLevel.GetCost();
        }

        return 0;
    }

    public bool IsOwnCollider(Collider col)
    {
        if (m_CurrentLevel)
        {
            return m_CurrentLevel.IsOwnCollider(col);
        }

        return false;
    }

    public float GetConstructionRadius()
    {
        if (m_CurrentLevel)
        {
            return m_CurrentLevel.GetConstructionRadius();
        }

        return 0.0f;
    }

    public void SetPosition(Vector3 pos)
    {
        m_Position = pos;

        if (m_CurrentLevel)
        {
            m_CurrentLevel.SetPosition(m_Position);
        }
    }

    public void SetBuildColor(bool canBuild)
    {
        if (m_CurrentLevel)
        {
            m_CurrentLevel.SetTowerAlphaColor(canBuild ? 1.0f : 0.5f);
        }
    }

    public void Build()
    {

    }

    public void AddLevel(TowerLevel level)
    {
        m_TowerLevels.Add(level);
    }

    TowerLevel CreateTowerLevel()
    {
        GameObject towerObject = GameObject.Instantiate(m_TowerLevels[m_CurrentLevelIndex].gameObject, Vector3.zero, Quaternion.identity) as GameObject;
        return towerObject.GetComponent<TowerLevel>();
    }

    public void PrepareToPlace()
    {
        m_CurrentLevel = CreateTowerLevel();
    }

    public void Upgrade()
    {
        if (m_CurrentLevelIndex + 1 < m_TowerLevels.Count)
        {
            if (m_CurrentLevel != null)
            {
                GameObject.Destroy(m_CurrentLevel);
            }

            ++m_CurrentLevelIndex;

            m_CurrentLevel = CreateTowerLevel();
            m_CurrentLevel.SetPosition(m_Position);
        }
    }

    public void Update()
    {

    }
}
