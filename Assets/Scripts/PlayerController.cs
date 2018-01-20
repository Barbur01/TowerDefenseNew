using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController
{
    enum State
    {
        IDLE = 0,
        PLACING_TOWER,
        TOWER_SELECTED,

        INVALID
    };

    public delegate void PlacingTower();
    public static event PlacingTower OnPlacingTower;

    public delegate void TowerConstructed();
    public static event TowerConstructed OnTowerConstructed;

    public delegate void TowerSelected();
    public static event TowerSelected OnTowerSelected;

    State m_State = State.INVALID;
    TowerManager m_TowerManager = null;
    Player m_Player = null;

    Tower m_TowerToManipulate = null;

    public PlayerController(Player player)
    {
        m_Player = player;
        m_TowerManager = new TowerManager();

        m_State = State.IDLE;

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

    public void Reset()
    {
        m_TowerToManipulate = null;
        m_TowerManager.Reset();
        m_State = State.IDLE;
    }

    void OnCreateTowerButtonPressed(Tower.Type type)
    {
        if (m_State == State.IDLE)
        {
            int cost = m_TowerManager.GetCost(Tower.Type.BASIC);

            if (m_Player.CanAfford(cost))
            {
                Debug.Log("OnCreateTowerButtonPressed");
                m_TowerToManipulate = m_TowerManager.PrepareNewTower(Tower.Type.BASIC);

                if (m_TowerToManipulate != null)
                {
                    m_State = State.PLACING_TOWER;

                    if (OnPlacingTower != null)
                    {
                        OnPlacingTower();
                    }
                }
            }
        }
    }

    void OnCancelTowerButtonPressed()
    {
        if (m_State == State.PLACING_TOWER)
        {
            Debug.Log("OnCancelTowerButtonPressed");

            if (m_TowerToManipulate != null)
            {
                m_TowerManager.DestroyTower(m_TowerToManipulate);
                m_TowerToManipulate = null;
            }

            m_State = State.IDLE;
        }
    }

    void OnUpgradeTowerButtonPressed()
    {
        if (m_State == State.TOWER_SELECTED)
        {
            Debug.Log("OnUpgradeTowerButtonPressed");

            if (m_TowerToManipulate != null)
            {
                int cost = m_TowerToManipulate.GetUpgradeCost();
                m_Player.AddCoins(cost);

                m_TowerToManipulate.Upgrade();
                m_TowerToManipulate = null;

                m_State = State.IDLE;
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
        m_TowerManager.Update();

        UpdateStates();
        UpdateDebugInput();
    }

    void UpdateStates()
    {
        switch (m_State)
        {
            case State.IDLE:
                UpdateIdle();
                break;
            case State.PLACING_TOWER:
                UpdatePlacingTower();
                break;
            case State.TOWER_SELECTED:
                break;
            case State.INVALID:
                break;
            default:
                break;
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

                if (CanUpgradeTower(m_TowerToManipulate))
                {
                    m_State = State.TOWER_SELECTED;

                    if (OnTowerSelected != null)
                    {
                        OnTowerSelected();
                    }
                }
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

                    m_Player.AddCoins(-m_TowerToManipulate.GetCost());

                    m_TowerToManipulate = null;

                    if (OnTowerConstructed != null)
                    {
                        OnTowerConstructed();
                    }

                    m_State = State.IDLE;
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
