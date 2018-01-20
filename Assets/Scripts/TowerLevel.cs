using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider))]
public class TowerLevel : MonoBehaviour
{
    public TowerData m_Data;

    Transform m_Transform = null;
    Transform m_CannonBase = null;
    Transform m_CannonTop = null;
    Transform m_CannonEnd = null;
    Transform m_RadiusSphere = null;

    void Awake()
    {
        m_Transform = transform;
        m_CannonBase = m_Transform.Find("Base");
        m_CannonTop = m_Transform.Find("Top");
        m_CannonEnd = m_CannonTop.Find("Cannon");
        m_RadiusSphere = m_Transform.Find("Radius");

        RefreshShootingRadiusSphere();
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
        return m_CannonEnd.position;
    }

    public void RotateCannon(float angle)
    {
        m_CannonTop.Rotate(Vector3.up, angle);
    }

    void SetAlpha(Renderer rend, float alpha)
    {
        Color c = rend.material.color;
        c.a = alpha;
        rend.material.color = c;
    }

    void SetTowerAlphaColor(float alpha)
    {
        SetAlpha(m_CannonBase.GetComponent<Renderer>(), alpha);
        SetAlpha(m_CannonTop.GetComponent<Renderer>(), alpha);
    }

    public void SetCanBuild(bool canbuild)
    {
        SetTowerAlphaColor(canbuild ? 1.0f : 0.5f);
        m_RadiusSphere.gameObject.SetActive(canbuild);
    }

    void RefreshShootingRadiusSphere()
    {
        Vector3 scale = m_RadiusSphere.localScale;
        scale *= m_Data.m_ShootingRadius;
        m_RadiusSphere.localScale = scale;
    }

    public void ShowShootingRadius(bool visible, float delay)
    {
        if (visible)
        {
            Invoke("ShowShootingRadius", delay);
        }
        else
        {
            Invoke("HideShootingRadius", delay);
        }
    }

    void ShowShootingRadius()
    {
        m_RadiusSphere.gameObject.SetActive(true);
    }

    void HideShootingRadius()
    {
        m_RadiusSphere.gameObject.SetActive(false);
    }
}
