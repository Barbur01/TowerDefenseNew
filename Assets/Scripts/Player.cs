using System.Collections;
using System.Collections.Generic;

public class Player
{
    public int m_InitialHealth = 1;
    public int m_InitialCoins = 500;

    int m_Health = 1;
    int m_CurrentCoins = 0;
    int m_Score = 0;

    public delegate void CoinsChanged(int totalCoins);
    public static event CoinsChanged OnCoinsChanged;

    public delegate void PlayerLost();
    public static event PlayerLost OnPlayerLost;

    public delegate void ScoreChanged(int score);
    public static event ScoreChanged OnScoreChanged;

    PlayerController m_Controller;

    public Player()
    {
        m_Controller = new PlayerController(this);

        AddCoins(m_InitialCoins);
        m_Score = 0;
        m_Health = m_InitialHealth;
    }

    ~Player()
    {
    }

    public void Reset()
    {
        m_Controller.Reset();

        AddCoins(m_InitialCoins);
        m_Score = 0;
        m_Health = m_InitialHealth;
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
