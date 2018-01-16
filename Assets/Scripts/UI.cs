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

    public Text m_CoinsText;
    public Button m_CancelButton;
    public Button m_CreateButton;
    public Button m_UpgradeButton;

    private void OnEnable()
    {
        Player.OnCoinsChanged += OnCoinsChanged;
        PlayerController.OnPlacingTower += OnPlacingTower;
        PlayerController.OnTowerConstructed += OnTowerConstructed;
    }

    private void OnDisable()
    {
        Player.OnCoinsChanged -= OnCoinsChanged;
        PlayerController.OnPlacingTower -= OnPlacingTower;
        PlayerController.OnTowerConstructed -= OnTowerConstructed;
    }

    void OnTowerConstructed()
    {
        ShowCreateButton();
    }

    void OnPlacingTower()
    {
        ShowCancelButton();
    }

    void OnCoinsChanged(int coins)
    {
        m_CoinsText.text = coins.ToString();
    }

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

        ShowCreateButton();
    }

    public void UpgradeTower()
    {
        if (OnUpgradeTowerButtonPressed != null)
        {
            OnUpgradeTowerButtonPressed();
        }

        ShowCreateButton();
    }

    void ShowCancelButton()
    {
        m_CancelButton.gameObject.SetActive(true);
        m_UpgradeButton.gameObject.SetActive(false);
        m_CreateButton.gameObject.SetActive(false);
    }

    void ShowCreateButton()
    {
        m_CancelButton.gameObject.SetActive(false);
        m_UpgradeButton.gameObject.SetActive(false);
        m_CreateButton.gameObject.SetActive(true);
    }

    void ShowUpgradeButton()
    {
        m_CancelButton.gameObject.SetActive(false);
        m_UpgradeButton.gameObject.SetActive(true);
        m_CreateButton.gameObject.SetActive(false);
    }
}
