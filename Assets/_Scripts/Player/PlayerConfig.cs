using UnityEngine;
using System.Collections;

public class PlayerConfig : MonoBehaviour
{
    #region Properties
    public GameObject[] enemyPrefabs { get { return m_enemyPrefabs; } }
    public int spawnAmount { get { return m_spawnAmount; } }
    public float spawnRate { get { return m_spawnRate; } }
    public float randomBias { get { return m_randomBias; } }
    #endregion

    #region Methods
    [SerializeField, Category( "Stats" )]
    private GameObject[] m_enemyPrefabs = new GameObject[ 0 ];
    [SerializeField, Category( "Stats" )]
    private int m_spawnAmount = 0;
    [SerializeField, Category( "Stats" )]
    private float m_spawnRate = 0;
    [SerializeField, Category( "Stats" )]
    private float m_randomBias = 0.0f;
    #endregion
}
