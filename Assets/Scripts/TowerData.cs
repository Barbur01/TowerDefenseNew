using UnityEngine;

[System.Serializable]
public class TowerData
{
    public float m_ShootSpeed = 1.0f;
    public float m_RotationSpeed = 50.0f;
    public float m_ShootingRadius = 5;

    public int m_Cost = 5;
    public int m_Damage = 1;
    public GameObject m_Projectile;
}
