using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float m_Speed = 100.0f;

    Transform m_Target = null;
    Transform m_Transform;
    int m_Damage = 1;

	// Use this for initialization
	void Start ()
    {
        m_Transform = transform;
    }

    public void SetTarget(Transform target, int damage)
    {
        m_Target = target;
        m_Damage = damage;
    }

    void OnTriggerEnter(Collider other)
    {
        other.SendMessage("ApplyDamage", m_Damage);

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_Target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector3 dir = m_Target.position - m_Transform.position;
            dir = dir.normalized;

            m_Transform.position += dir * m_Speed * Time.deltaTime;
        }
	}
}
