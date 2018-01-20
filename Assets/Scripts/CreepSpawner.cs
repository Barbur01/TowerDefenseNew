using UnityEngine;
using System.Collections;

public class CreepSpawner : MonoBehaviour
{
    public Vector2 m_TimeBetweenOrders;
    public int m_MaxEnemiesOrder;
    public int MAX_FIBONAZZI;

    int m_LastFibonacciIndex;
    int m_NumEnemies;
    float m_TimeUntilNextOrder;
    float m_TimeUntilNextEnemy;

    int m_WavesCounter = 0;

    Vector3 m_NextTargetPosition;
    Transform m_Transform;
    EnemyManager m_EnemyManager;

    private enum SpawnerState
    {
        WAITING,
        STARTING_SPAWN,
        SPAWNING,
        SPAWNING_END
    }

    private SpawnerState m_State;

    // Use this for initialization
    void Start()
    {
        m_Transform = transform;
        m_LastFibonacciIndex = 1;
        m_TimeUntilNextOrder = GenerateTimeForNextOrder();
        MAX_FIBONAZZI = 7;
        m_NextTargetPosition = GetNextTargetPoint();
        m_State = SpawnerState.WAITING;
        m_NumEnemies = 0;
        m_TimeUntilNextEnemy = 0.0f;
        m_WavesCounter = 0;
    }

    public void Reset()
    {
        m_WavesCounter = 0;
        m_TimeUntilNextEnemy = 0.0f;
        m_NumEnemies = 0;
        m_LastFibonacciIndex = 1;
        m_State = SpawnerState.WAITING;
    }

    public void SetEnemyManager(EnemyManager enemyManager)
    {
        m_EnemyManager = enemyManager;
    }

    float GenerateTimeForNextOrder()
    {
        return Random.Range(m_TimeBetweenOrders.x, m_TimeBetweenOrders.y);
    }

    // Update is called once per frame
    void Update()
    {
        m_TimeUntilNextOrder -= Time.deltaTime;

        switch (m_State)
        {
            case SpawnerState.WAITING:
                if (m_TimeUntilNextOrder <= 0)
                {
                    m_State = SpawnerState.STARTING_SPAWN;
                }
                break;
            case SpawnerState.STARTING_SPAWN:
                m_NumEnemies = Fibonacci(m_LastFibonacciIndex);
                m_NumEnemies = Mathf.Min(m_NumEnemies, m_MaxEnemiesOrder);
                if (m_NumEnemies != -1)
                {
                    m_State = SpawnerState.SPAWNING;
                }
                break;
            case SpawnerState.SPAWNING:
                m_TimeUntilNextEnemy -= Time.deltaTime;
                if (m_TimeUntilNextEnemy <= 0.0f)
                {
                    CreateEnemy();
                    m_NumEnemies--;
                    if (m_NumEnemies <= 0)
                    {
                        m_State = SpawnerState.SPAWNING_END;
                    }
                    m_TimeUntilNextEnemy = 1.0f;
                }
                break;
            case SpawnerState.SPAWNING_END:
                m_WavesCounter++;

                if (m_LastFibonacciIndex < MAX_FIBONAZZI)
                {
                    m_LastFibonacciIndex++;
                }
                ResetTimeUntilNextOrder();
                m_NextTargetPosition = GetNextTargetPoint();
                m_State = SpawnerState.WAITING;
                break;
            default:
                break;
        }
    }

    int GetDifficulty()
    {
        if (m_WavesCounter < 7)
            return 0;
        else if (m_WavesCounter < 14)
            return 1;

        return 2;
    }

    void CreateEnemy()
    {
        m_EnemyManager.CreateRandomEnemy(m_NextTargetPosition, m_Transform, GetDifficulty());
        m_NextTargetPosition = GetNextTargetPoint();
    }

    Vector3 GetNextTargetPoint()
    {
        return m_Transform.position;
    }

    void ResetTimeUntilNextOrder()
    {
        m_TimeUntilNextOrder = GenerateTimeForNextOrder();
    }

    int Fibonacci(int n)
    {
        if (n == 0)
        {
            return 0;
        }
        else if (n == 1)
        {
            return 1;
        }
        else if (n >= 2)
        {
            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }
        else
        {
            Debug.Log("Bad fibonacci index");
            Debug.Assert(false);
            return -1;
        }
    }
}
