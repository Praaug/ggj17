using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPlane : MonoBehaviour
{
    #region Events
    /// <summary>
    /// Event that is fired when a wave is completed
    /// </summary>
    public event System.Action<EnemySpawnPlane> OnWaveDoneThis;
    #endregion

    #region Fields
    [SerializeField, Category( "Stats" )]
    private float m_width = 0.0f;
    [SerializeField, Category( "Stats" )]
    private float m_height = 0.0f;
    [SerializeField, Category( "Stats" )]
    private float m_depth = 0.0f;

    /// <summary>
    /// Player that determines the spawns of this enemy spawn plane
    /// </summary>
    private Player m_owningPlayer;
    private Player m_damagedPlayer;

    private List<Enemy> m_enemySpawnedList = new List<Enemy>();
    private bool m_allEnemiesSpawned = false;
    #endregion

    #region Methods
    public void AssignPlayer( Player p_owningPlayer, Player p_damagedPlayer )
    {
        m_owningPlayer = p_owningPlayer;
        m_damagedPlayer = p_damagedPlayer;
    }

    /// <summary>
    /// Starts to spawn a player's mobs
    /// </summary>
    public void SpawnMobs()
    {
        // Clear spawn list
        m_enemySpawnedList.Clear();
        m_allEnemiesSpawned = false;

        PlayerConfig _config = m_owningPlayer.config;

        StartCoroutine( Coroutine_Spawn( m_owningPlayer.config.waveInfo.CalculatePrefabs(), _config.spawnRate, _config.randomBias ) );
    }

    private void OnDestroy()
    {
        for ( int i = 0; i < m_enemySpawnedList.Count; i++ )
            m_enemySpawnedList[ i ].OnKillThis -= Enemy_OnKillThis;
        m_enemySpawnedList.Clear();
    }

    private IEnumerator Coroutine_Spawn( List<EnemyInfo> p_spawnPrefabs, float p_baseTick, float p_randomBias )
    {
        for ( int i = 0; i < p_spawnPrefabs.Count; i++ )
        {
            // Instantiate entity
            GameObject _prefab = p_spawnPrefabs[ i ].gameObject;
            Vector3 _spawnPoint = transform.position + Randomx.Box( m_width, m_height, m_depth );

            // Create enemy
            Enemy _enemy = Enemy.CreateEnemy( _prefab, _spawnPoint, transform.rotation );

            // Add to spawn list
            m_enemySpawnedList.Add( _enemy );
            // Subscribe kill event
            _enemy.OnKillThis += Enemy_OnKillThis;

            yield return new WaitForSeconds( Randomx.Bias( p_randomBias ) + p_baseTick );
        }

        m_allEnemiesSpawned = true;
    }

    private void Enemy_OnKillThis( Enemy p_enemy )
    {
        p_enemy.OnKillThis -= Enemy_OnKillThis;

        m_enemySpawnedList.Remove( p_enemy );

        if ( m_allEnemiesSpawned && m_enemySpawnedList.Count == 0 )
        {
            Dbg.Log( "ON WAVE DONE CALLED" );
            if ( OnWaveDoneThis != null )
                OnWaveDoneThis( this );

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube( transform.position, new Vector3( m_width, m_height, m_depth ) );
    }
    #endregion
}
