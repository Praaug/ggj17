using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPlane : MonoBehaviour
{
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
        PlayerConfig _config = m_owningPlayer.config;

        StartCoroutine( Coroutine_Spawn( _config.enemyPrefabs, _config.spawnAmount, _config.spawnRate, _config.randomBias ) );
    }

    private IEnumerator Coroutine_Spawn( GameObject[] p_spawnPrefabs, int p_spawnAmount, float p_baseTick, float p_randomBias )
    {
        for ( int i = 0; i < p_spawnAmount; i++ )
        {
            // Instantiate entity
            GameObject _prefab = p_spawnPrefabs[ Randomx.Range0( p_spawnPrefabs.Length ) ];
            Vector3 _spawnPoint = transform.position + Randomx.Box( m_width, m_height, m_depth );

            // Create enemy
            Enemy.CreateEnemy( _prefab, _spawnPoint, transform.rotation );

            Dbg.Log( "Instantiated enemy" );
            yield return new WaitForSeconds( Randomx.Bias( p_randomBias ) + p_baseTick );
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube( transform.position, new Vector3( m_width, m_height, m_depth ) );
    }
    #endregion
}
