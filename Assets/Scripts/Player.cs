using System.Collections;
using System.Collections.Generic;

public class Player
{
    public int m_Health = 1;
    public int m_InitialCoins = 5;

    int m_CurrentCoins = 0;
    int m_Score = 0;

    PlayerController m_Controller;

    public Player()
    {
        m_Controller = new PlayerController(this);

        m_CurrentCoins = m_InitialCoins;
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

    public void AddCoins(int coins)
    {
        m_CurrentCoins += coins;
    }

    public void Update()
    {
        m_Controller.Update();
    }
}
