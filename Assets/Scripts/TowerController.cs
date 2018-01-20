using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController
{
    Tower m_Tower = null;
    float m_LastShotTime = 0.0f;

    public void Init(Tower tower)
    {
        m_Tower = tower;
        m_Tower.SetState(Tower.State.IDLE);
    }

    bool IsInsideShootingRadius(Transform target)
    {
        Vector3 dir = m_Tower.GetPosition() - target.position;
        dir.y = 0;

        TowerData data = m_Tower.GetCurrentLevelData();
        float sqShootingRadius = data.m_ShootingRadius * data.m_ShootingRadius;
        return dir.sqrMagnitude <= sqShootingRadius;
    }

    Transform GetNearestTarget()
    {
        Creep[] enemies = GameObject.FindObjectsOfType<Creep>();

        Transform nearestEnemy = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < enemies.Length; ++i)
        {
            Creep creep = enemies[i].GetComponent<Creep>();
            if (!creep.IsDead() && !creep.IsTargeted())
            {
                Vector2 pos = m_Tower.GetPosition() - enemies[i].transform.position;
                float sqDistance = Vector2.SqrMagnitude(pos);

                if (IsInsideShootingRadius(enemies[i].transform) && minDistance > sqDistance)
                {
                    minDistance = sqDistance;
                    nearestEnemy = enemies[i].transform;
                }
            }
        }

        return nearestEnemy;
    }

    public void Update ()
    {
        m_LastShotTime -= Time.deltaTime;

        switch (m_Tower.GetState())
        {
            case Tower.State.IDLE:
                UpdateIdle();
                break;
            case Tower.State.ATTACK:
                UpdateAttack();
                break;
            case Tower.State.INVALID:
                break;
            default:
                break;
        }
    }

    void UpdateIdle()
    {
        Transform target = null;

        if (!m_Tower.HasEnemyTarget())
        {
            target = GetNearestTarget();
            m_Tower.SetEnemyTarget(target);
        }

        if (m_Tower.HasEnemyTarget())
        {
            target.GetComponent<Creep>().SetTargeted(true);
            m_Tower.SetState(Tower.State.ATTACK);
        }
    }

    bool IsTargetDead(Transform target)
    {
        if (target != null)
        {
            Creep creep = target.GetComponent<Creep>();
            return creep.IsDead();
        }

        return true;
    }

    float FaceTarget(Transform target)
    {
        Vector3 dir = target.position - m_Tower.GetPosition(); 
        dir.y = 0;
        dir = dir.normalized;

        float targetAngle = Vector3.SignedAngle(m_Tower.GetCannonForward(), dir, Vector3.up);
        float sign = Mathf.Sign(targetAngle);
        float delta = Mathf.Min(Mathf.Abs(targetAngle), m_Tower.GetCurrentLevelData().m_RotationSpeed * Time.deltaTime);

        m_Tower.RotateCannon(sign * delta);

        return targetAngle;
    }

    void Shoot(Transform enemy)
    {
        if (m_LastShotTime <= 0.0f)
        {
            TowerData data = m_Tower.GetCurrentLevelData();

            GameObject projectileObj = GameObject.Instantiate(data.m_Projectile, m_Tower.GetCannonPosition(), Quaternion.identity);
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            projectile.SetTarget(enemy, data.m_Damage);
            m_LastShotTime = data.m_ShootSpeed;
        }
    }

    void UpdateAttack()
    {
        Transform target = m_Tower.GetEnemyTarget();

        if (IsTargetDead(target) || !IsInsideShootingRadius(target))
        {
            if (target != null)
            {
                target.GetComponent<Creep>().SetTargeted(false);
            }

            m_Tower.SetState(Tower.State.IDLE);
            m_Tower.SetEnemyTarget(null);
        }
        else
        {
            float targetAngle = FaceTarget(target);

            if (Mathf.Abs(targetAngle) < 10.0f)
            {
                Shoot(target);
            }
        }
    }
}
