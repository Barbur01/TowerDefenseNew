using System.Collections;
using System.Collections.Generic;

public class Player
{
    public int m_Health = 1;
    public int m_InitialCoins = 5;

    int m_CurrentCoins = 0;
    int m_Score = 0;

    public delegate void CoinsChanged(int totalCoins);
    public static event CoinsChanged OnCoinsChanged;

    PlayerController m_Controller;

    public Player()
    {
        m_Controller = new PlayerController(this);

        AddCoins(m_InitialCoins);
        m_Score = 0;
    }

    ~Player()
    {
    }

    public void ApplyDamage(int damage)
    {
        m_Health -= damage;
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
