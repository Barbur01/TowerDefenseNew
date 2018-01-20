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

    public delegate void ReplayGameRequested();
    public static event ReplayGameRequested OnReplayGameRequested;
    
    public Text m_CoinsText;
    public Text m_ScoreText;
    public Button m_CancelButton;
    public Button m_CreateButton;
    public Button m_UpgradeButton;
    public GameObject m_LostPanel;

    private void OnEnable()
    {
        Player.OnCoinsChanged += OnCoinsChanged;
        Player.OnPlayerLost += OnPlayerLost;
        Player.OnScoreChanged += OnScoreChanged;
        PlayerController.OnPlacingTower += OnPlacingTower;
        PlayerController.OnTowerConstructed += OnTowerConstructed;
        PlayerController.OnTowerSelected += OnTowerSelected;
    }

    private void OnDisable()
    {
        Player.OnCoinsChanged -= OnCoinsChanged;
        Player.OnPlayerLost -= OnPlayerLost;
        Player.OnScoreChanged -= OnScoreChanged;
        PlayerController.OnPlacingTower -= OnPlacingTower;
        PlayerController.OnTowerConstructed -= OnTowerConstructed;
        PlayerController.OnTowerSelected -= OnTowerSelected;
    }

    void Restart()
    {
        ShowCreateButton();
        m_LostPanel.SetActive(false);
    }

    void OnPlayerLost()
    {
        ShowLostPanel();
        DisableAllHud();
    }

    void OnScoreChanged(int score)
    {
        m_ScoreText.text = score.ToString();
    }

    void OnTowerSelected(bool selected)
    {
        if (selected)
        {
            ShowUpgradeButton();
        }
        else
        {
            ShowCreateButton();
        }
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

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void ReplayGame()
    {
        if (OnReplayGameRequested != null)
        {
            OnReplayGameRequested();
        }

        Restart();
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

    void DisableAllHud()
    {
        m_CancelButton.gameObject.SetActive(false);
        m_UpgradeButton.gameObject.SetActive(false);
        m_CreateButton.gameObject.SetActive(false);
    }

    void ShowLostPanel()
    {
        m_LostPanel.SetActive(true);
    }
}
