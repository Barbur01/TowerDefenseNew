using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    Player m_Player;

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
        m_Player = new Player();
        EnemyManager.Instance.Init();
	}

    void OnDestroy()
    {
        EnemyManager.Instance.Destroy();
    }

    void OnReplayGameRequested()
    {
        m_Player.Reset();
        EnemyManager.Instance.Reset();
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
	}
}
