using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController
{
    public delegate void PlacingTower();
    public static event PlacingTower OnPlacingTower;

    public delegate void TowerConstructed();
    public static event TowerConstructed OnTowerConstructed;

    public delegate void TowerSelected(bool selected);
    public static event TowerSelected OnTowerSelected;

    public delegate void TowerCanUpgrade();
    public static event TowerCanUpgrade OnTowerCanUpgrade;

    TowerManager m_TowerManager = null;
    Player m_Player = null;

    Tower m_TowerToManipulate = null;

    public PlayerController()
    {
        PlayerBase.OnPlayerBaseTouched += OnPlayerBaseTouched;
        Creep.OnCreepDied += OnCreepDied;
        UI.OnCreateTowerButtonPressed += OnCreateTowerButtonPressed;
        UI.OnCancelTowerButtonPressed += OnCancelTowerButtonPressed;
        UI.OnUpgradeTowerButtonPressed += OnUpgradeTowerButtonPressed;
    }

    ~PlayerController()
    {
        PlayerBase.OnPlayerBaseTouched -= OnPlayerBaseTouched;
        Creep.OnCreepDied -= OnCreepDied;
        UI.OnCreateTowerButtonPressed -= OnCreateTowerButtonPressed;
        UI.OnCancelTowerButtonPressed -= OnCancelTowerButtonPressed;
        UI.OnUpgradeTowerButtonPressed -= OnUpgradeTowerButtonPressed;
    }

    public void Init(Player player, TowerManager towerManager)
    {
        m_Player = player;
        m_Player.SetState(Player.State.IDLE);
        m_TowerManager = towerManager;
    }

    public void Reset()
    {
        m_TowerToManipulate = null;
        m_Player.SetState(Player.State.IDLE);
    }
    
    void OnCreateTowerButtonPressed(Tower.Type type)
    {
        int cost = m_TowerManager.GetCost(Tower.Type.BASIC);

        if (m_Player.CanAfford(cost))
        {
            Debug.Log("OnCreateTowerButtonPressed");
            m_TowerToManipulate = m_TowerManager.PrepareNewTower(Tower.Type.BASIC);

            if (m_TowerToManipulate != null)
            {
                m_Player.SetState(Player.State.PLACING_TOWER);

                if (OnPlacingTower != null)
                {
                    OnPlacingTower();
                }
            }
        }
    }

    void OnCancelTowerButtonPressed()
    {
        if (m_Player.GetState() == Player.State.PLACING_TOWER)
        {
            Debug.Log("OnCancelTowerButtonPressed");

            if (m_TowerToManipulate != null)
            {
                m_TowerManager.DestroyTower(m_TowerToManipulate);
                m_TowerToManipulate = null;
            }

            m_Player.SetState(Player.State.IDLE);
        }
    }

    void OnUpgradeTowerButtonPressed()
    {
        if (m_Player.GetState() == Player.State.TOWER_SELECTED)
        {
            Debug.Log("OnUpgradeTowerButtonPressed");

            if (m_TowerToManipulate != null)
            {
                int cost = m_TowerToManipulate.GetUpgradeCost();
                m_Player.AddCoins(cost);

                m_TowerToManipulate.Upgrade();
                m_TowerToManipulate.Unselect(2.0f);

                m_TowerToManipulate = null;

                m_Player.SetState(Player.State.IDLE);
            }
        }
    }

    void OnCreepDied(Creep creep)
    {
        m_Player.AddCoins(creep.GetCoins());
        m_Player.AddScore(1);
    }

    void OnPlayerBaseTouched(int damage)
    {
        m_Player.ApplyDamage(damage);
    }

    public void Update()
    {
        UpdateStates();
        UpdateDebugInput();
    }

    void UpdateStates()
    {
        switch (m_Player.GetState())
        {
            case Player.State.IDLE:
                UpdateIdle();
                break;
            case Player.State.PLACING_TOWER:
                UpdatePlacingTower();
                break;
            case Player.State.TOWER_SELECTED:
                UpdateTowerSelected();
                break;
            case Player.State.INVALID:
                break;
            default:
                break;
        }
    }

    void UpdateTowerSelected()
    {
        if (CanUpgradeTower(m_TowerToManipulate))
        {
            if (OnTowerCanUpgrade != null)
            {
                OnTowerCanUpgrade();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mpos = Input.mousePosition;
            Ray r = Camera.main.ScreenPointToRay(mpos);
            RaycastHit hit;

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                m_TowerToManipulate.Unselect(0.0f);
                m_TowerToManipulate = null;
                m_Player.SetState(Player.State.IDLE);

                if (Physics.Raycast(r, out hit, 1000, LayerMask.GetMask("tower")))
                {
                    Tower towerToManipulate = m_TowerManager.GetTower(hit.collider.gameObject);
                    if (towerToManipulate != m_TowerToManipulate)
                    {
                        if (m_TowerToManipulate != null)
                        {
                            m_TowerToManipulate.Unselect(0.0f);
                        }

                        m_TowerToManipulate = towerToManipulate;
                        m_TowerToManipulate.Select();

                        if (CanUpgradeTower(m_TowerToManipulate))
                        {
                            m_Player.SetState(Player.State.TOWER_SELECTED);
                        }
                    }
                }

                if (m_Player.GetState() == Player.State.IDLE)
                {
                    if (OnTowerSelected != null)
                    {
                        OnTowerSelected(false);
                    }
                }
            }
        }
    }

    void UpdateIdle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mpos = Input.mousePosition;
            Ray r = Camera.main.ScreenPointToRay(mpos);
            RaycastHit hit;

            if (Physics.Raycast(r, out hit, 1000, LayerMask.GetMask("tower")))
            {
                m_TowerToManipulate = m_TowerManager.GetTower(hit.collider.gameObject);
                m_TowerToManipulate.Select();

                m_Player.SetState(Player.State.TOWER_SELECTED);
            }
        }
    }

    void UpdatePlacingTower()
    {
        Vector3 mpos = Input.mousePosition;
        Ray r = Camera.main.ScreenPointToRay(mpos);
        RaycastHit hit;

        if (Physics.Raycast(r, out hit, 1000, LayerMask.GetMask("terrain")))
        {
            m_TowerToManipulate.SetPosition(hit.point);

            bool canBuild = CanBuildTower(hit.point, m_TowerToManipulate);
            m_TowerToManipulate.SetBuildColor(canBuild);

            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject() && canBuild)
                {
                    m_TowerManager.AddTower(m_TowerToManipulate);
                    m_TowerToManipulate.Construct();
                    m_TowerToManipulate.Unselect(0.0f);

                    m_Player.AddCoins(-m_TowerToManipulate.GetCost());
                    m_TowerToManipulate = null;

                    if (OnTowerConstructed != null)
                    {
                        OnTowerConstructed();
                    }

                    m_Player.SetState(Player.State.IDLE);
                }
            }
        }
    }

    bool CanUpgradeTower(Tower tower)
    {
        if (tower.CanUpgrade() && m_Player.CanAfford(tower.GetUpgradeCost()))
        {
            return true;
        }

        return false;
    }

    bool CanBuildTower(Vector3 pos, Tower tower)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, tower.GetConstructionRadius());

        foreach (var col in colliders)
        {
            if (!tower.IsOwnCollider(col) &&
                col.gameObject.layer != LayerMask.NameToLayer("terrain") &&
                col.gameObject.layer != LayerMask.NameToLayer("creep"))
            {
                return false;
            }
        }

        return true;
    }

    void UpdateDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            ++Time.timeScale;
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            --Time.timeScale;
        }
    }
}
