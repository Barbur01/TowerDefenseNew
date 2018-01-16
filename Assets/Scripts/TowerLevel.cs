using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider))]
public class TowerLevel : MonoBehaviour
{
    public TowerData m_Data;
    Transform m_Transform = null;
    Transform m_CannonTop = null;

    void Awake()
    {
        m_Transform = transform;
    }

    private void Start()
    {
        m_CannonTop = m_Transform.Find("Top");
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

    public Vector3 GetCannonTopForward()
    {
        return m_CannonTop.forward;
    }

    public Vector3 GetCannonPosition()
    {
        return m_CannonTop.position;
    }

    public void RotateCannon(float angle)
    {
        m_CannonTop.Rotate(Vector3.up, angle);
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
