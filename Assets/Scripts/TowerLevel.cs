using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider))]
public class TowerLevel : MonoBehaviour
{
    public TowerData m_Data;
    Transform m_Transform;

    void Awake()
    {
        m_Transform = transform;
    }

    public void SetPosition(Vector3 pos)
    {
        m_Transform.position = pos;
    }

    public int GetCost()
    {
        return m_Data.m_Cost;
    }

    public bool IsOwnCollider(Collider col)
    {
        return GetComponent<Collider>() == col;
    }

    public float GetConstructionRadius()
    {
        return GetComponent<CapsuleCollider>().radius;
    }

    public void SetTowerAlphaColor(float alpha)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Color c = renderer.material.color;
            c.a = alpha;
            renderer.material.color = c;
        }
    }
}
