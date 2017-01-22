using UnityEngine;
using System.Collections;

public static class CombatManager
{
    public static void PerformAttack( Player p_attackingPlayer, Enemy p_targetEnemy, CombatHit p_hitInfo )
    {
        float _eleBonusDamage = p_attackingPlayer.elementBuffDict[ p_targetEnemy.info.elementType ];
        p_targetEnemy.InflictDamage( p_attackingPlayer, p_hitInfo.damage + _eleBonusDamage );
    }
}

public class CombatHit
{
    public float damage;
}
