using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    Player m_Player;

    private void OnEnable()
    {
        Player.OnPlayerLost += OnPlayerLost;
    }

    private void OnDisable()
    {
        Player.OnPlayerLost -= OnPlayerLost;
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

    void OnPlayerLost()
    {
        m_Player.
    }

    // Update is called once per frame
    void Update ()
    {
        m_Player.Update();
	}
}
