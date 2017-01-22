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
    private bool[] m_allEnemiesSpawned = new bool[ 3 ];
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

        for ( int i = 0; i < m_allEnemiesSpawned.Length; i++ )
            m_allEnemiesSpawned[ i ] = false;

        WaveInfo _waveInfo = m_owningPlayer.waveInfo;

        m_owningPlayer.RegisterSpendElementPoints( ElementType.Dirt, _waveInfo.GetTotalAmplitude( ElementType.Dirt ) );
        m_owningPlayer.RegisterSpendElementPoints( ElementType.Fire, _waveInfo.GetTotalAmplitude( ElementType.Fire ) );
        m_owningPlayer.RegisterSpendElementPoints( ElementType.Air, _waveInfo.GetTotalAmplitude( ElementType.Air ) );
        m_owningPlayer.RegisterSpendElementPoints( ElementType.Water, _waveInfo.GetTotalAmplitude( ElementType.Water ) );

        PlayerConfig _config = m_owningPlayer.config;

        var _calcSpawns = _waveInfo.CalculateSpawns();
        for ( int i = 0; i < _calcSpawns.Length; i++ )
        {
            List<EnemyInfo> _mashUp = new List<EnemyInfo>();

            foreach ( ElementType _elementType in EnumUtility.GetValues<ElementType>() )
                _mashUp.AddRange( _calcSpawns[ i ][ _elementType ] );

            _mashUp.Shuffle();

            StartCoroutine( Coroutine_Spawn( _mashUp, i, _config.spawnRate, _config.randomBias ) );
        }

        // Clear wave data
        _waveInfo.ClearElementCount();
    }

    private void OnDestroy()
    {
        for ( int i = 0; i < m_enemySpawnedList.Count; i++ )
            m_enemySpawnedList[ i ].OnKillThis -= Enemy_OnKillThis;
        m_enemySpawnedList.Clear();
    }

    private IEnumerator Coroutine_Spawn( List<EnemyInfo> p_spawnPrefabs, int p_frequencyIndex, float p_baseTick, float p_randomBias )
    {
        for ( int i = 0; i < p_spawnPrefabs.Count; i++ )
        {
            if ( i < p_spawnPrefabs.Count - 1 )
                yield return new WaitForSeconds( Randomx.Bias( p_randomBias ) + p_baseTick );

            // Instantiate entity
            GameObject _prefab = p_spawnPrefabs[ i ].gameObject;
            Vector3 _spawnPoint = transform.position + Randomx.Box( m_width, m_height, m_depth );

            // Create enemy
            Enemy _enemy = Enemy.CreateEnemy( _prefab, _spawnPoint, transform.rotation );

            // Add to spawn list
            m_enemySpawnedList.Add( _enemy );
            // Subscribe kill event
            _enemy.OnKillThis += Enemy_OnKillThis;
        }

        m_allEnemiesSpawned[ p_frequencyIndex ] = true;
    }

    private void Enemy_OnKillThis( Enemy p_enemy )
    {
        p_enemy.OnKillThis -= Enemy_OnKillThis;

        m_enemySpawnedList.Remove( p_enemy );

        bool _allFrequenciesReady = true;
        for ( int i = 0; i < m_allEnemiesSpawned.Length; i++ )
        {
            if ( !m_allEnemiesSpawned[ i ] )
            {
                _allFrequenciesReady = false;
                break;
            }
        }

        if ( _allFrequenciesReady && m_enemySpawnedList.Count == 0 )
        {
            if ( OnWaveDoneThis != null )
                OnWaveDoneThis( this );
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube( transform.position + Vector3.up * m_height * 0.5f, new Vector3( m_width, m_height, m_depth ) );
    }
    #endregion
}
