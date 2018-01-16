using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public delegate void CreateTowerButtonPressed(Tower.Type type);
    public static event CreateTowerButtonPressed OnCreateTowerButtonPressed;

    public delegate void CancelTowerButtonPressed();
    public static event CancelTowerButtonPressed OnCancelTowerButtonPressed;

    public delegate void UpgradeTowerButtonPressed();
    public static event UpgradeTowerButtonPressed OnUpgradeTowerButtonPressed;

    public void CreateBasicTower()
    {
        if (OnCreateTowerButtonPressed != null)
        {
            OnCreateTowerButtonPressed(Tower.Type.BASIC);
        }
    }

    public void CancelTower()
    {
        if (OnCancelTowerButtonPressed != null)
        {
            OnCancelTowerButtonPressed();
        }
    }

    public void UpgradeTower()
    {
        if (OnUpgradeTowerButtonPressed != null)
        {
            OnUpgradeTowerButtonPressed();
        }
    }
}
