using System.Collections;
using System.Collections.Generic;

public class Player
{
    public enum State
    {
        IDLE = 0,
        PLACING_TOWER,
        TOWER_SELECTED,

        INVALID
    };

    int m_InitialHealth = 1;
    int m_InitialCoins = 5;

    int m_Health = 1;
    int m_CurrentCoins = 0;
    int m_Score = 0;
    State m_State = State.INVALID;

    PlayerController m_Controller;

    public delegate void CoinsChanged(int totalCoins);
    public static event CoinsChanged OnCoinsChanged;

    public delegate void PlayerLost();
    public static event PlayerLost OnPlayerLost;

    public delegate void ScoreChanged(int score);
    public static event ScoreChanged OnScoreChanged;


    public void Init(TowerManager towerManager)
    {
        m_Controller = new PlayerController();
        m_Controller.Init(this, towerManager);

        Reset();
    }

    public void Reset()
    {
        m_Controller.Reset();

        m_CurrentCoins = 0;
        AddCoins(m_InitialCoins);
        m_Score = 0;
        AddScore(0);
        m_Health = m_InitialHealth;
    }

    public void SetState(State state)
    {
        m_State = state;
    }

    public State GetState()
    {
        return m_State;
    }

    public void AddScore(int score)
    {
        m_Score += score;

        if (OnScoreChanged != null)
        {
            OnScoreChanged(m_Score);
        }
    }

    public void ApplyDamage(int damage)
    {
        m_Health -= damage;

        if (IsDead())
        {
            Die();
        }
    }

    void Die()
    {
        if (OnPlayerLost != null)
        {
            OnPlayerLost();
        }
    }

    public bool IsDead()
    {
        return m_Health <= 0;
    }

    public bool CanAfford(int coins)
    {
        return m_CurrentCoins >= coins;
    }

    public void AddCoins(int coins)
    {
        m_CurrentCoins += coins;

        if (OnCoinsChanged != null)
        {
            OnCoinsChanged(m_CurrentCoins);
        }
    }

    public void Update()
    {
        m_Controller.Update();
    }
}
