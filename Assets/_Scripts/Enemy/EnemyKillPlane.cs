using UnityEngine;
using System.Collections;

public class EnemyKillPlane : MonoBehaviour
{
    #region Fields
    /// <summary>
    /// The player that owns this lane
    /// </summary>
    private Player m_owningPlayer;
    /// <summary>
    /// Player that is punished when the mobs passes the kill plane
    /// </summary>
    private Player m_damagedPlayer;
    #endregion

    #region Methods
    public void AssignPlayer( Player p_owningPlayer, Player p_damagedPlayer )
    {
        m_owningPlayer = p_owningPlayer;
        m_damagedPlayer = p_damagedPlayer;
    }

    private void OnTriggerEnter( Collider p_collider )
    {
        Enemy _enemy;
        if ( !Enemy.IsEnemy( p_collider, out _enemy ) )
            return;

        // Kill the enemy
        _enemy.Kill( null );

        // Reduce life of damaged player
        m_damagedPlayer.ReduceLife( 1 );

        // Increase points / Reward owning player [NYI]
    }
    #endregion
}
