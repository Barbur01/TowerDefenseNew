using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower
{ 
    List<TowerLevel> m_TowerLevels = new List<TowerLevel>();
    int m_CurrentLevelIndex = 0;
    TowerLevel m_CurrentLevel = null;
    TowerController m_Controller = null;

    Transform m_EnemyTarget = null;

    Vector3 m_Position;

    public enum Type
    {
        BASIC = 0,
        COUNT,
        INVALID
    };

    Type m_Type = Type.INVALID;

    public Tower(Type type)
    {
        m_Type = type;
        m_Controller = new TowerController(this);
    }

    public void Destroy()
    {
        if (m_CurrentLevel)
        {
            GameObject.Destroy(m_CurrentLevel.gameObject);
            m_CurrentLevel = null;
        }
    }

    public TowerData GetCurrentLevelData()
    {
        if (m_CurrentLevel)
        {
            return m_CurrentLevel.m_Data;
        }

        return new TowerData();
    }

    public int GetCost()
    {
        if (m_CurrentLevel)
        {
            return m_CurrentLevel.GetCost();
        }
        else
        {
            if (m_TowerLevels.Count > 0)
            {
                return m_TowerLevels[0].m_Data.m_Cost;
            }
        }

        return 0;
    }

    public void SetEnemyTarget(Transform target)
    {
        m_EnemyTarget = target;
    }

    public Transform GetEnemyTarget()
    {
        return m_EnemyTarget;
    }

    public bool HasEnemyTarget()
    {
        return m_EnemyTarget != null;
    }

    public Vector3 GetCannonForward()
    {
        if (m_CurrentLevel)
        {
            return m_CurrentLevel.GetCannonTopForward();
        }

        return Vector3.forward;
    }

    public Vector3 GetCannonPosition()
    {
        return m_CurrentLevel.GetCannonPosition();
    }

    public void RotateCannon(float angle)
    {
        m_CurrentLevel.RotateCannon(angle);
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

    public Vector3 GetPosition()
    {
        return m_Position;
    }

    public void SetBuildColor(bool canBuild)
    {
        if (m_CurrentLevel)
        {
            m_CurrentLevel.SetTowerAlphaColor(canBuild ? 1.0f : 0.5f);
        }
    }

    public void Construct()
    {
        if (m_CurrentLevel)
        {
            m_CurrentLevel.GetComponent<CapsuleCollider>().enabled = true;
        }
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
        if (m_CurrentLevel)
        {
            m_CurrentLevel.GetComponent<CapsuleCollider>().enabled = false;
        }
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
        m_Controller.Update();
    }
}
