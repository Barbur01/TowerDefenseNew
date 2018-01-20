using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    Player m_Player;
    EnemyManager m_EnemyManager;
    TowerManager m_TowerManager;

    private void OnEnable()
    {
        Player.OnPlayerLost += OnPlayerLost;
        UI.OnReplayGameRequested += OnReplayGameRequested;
    }

    private void OnDisable()
    {
        Player.OnPlayerLost -= OnPlayerLost;
        UI.OnReplayGameRequested -= OnReplayGameRequested;
    }

    // Use this for initialization
    void Start ()
    {
        m_TowerManager = new TowerManager();
        m_TowerManager.Init();

        m_EnemyManager = new EnemyManager();
        m_EnemyManager.Init();

        m_Player = new Player();
        m_Player.Init(m_TowerManager);
	}
    
    void OnReplayGameRequested()
    {
        m_Player.Reset();
        m_TowerManager.Reset();
        m_EnemyManager.Reset();
        Time.timeScale = 1.0f;
    }

    void OnPlayerLost()
    {
        Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    void Update ()
    {
        m_Player.Update();
        m_TowerManager.Update();

    }
}
