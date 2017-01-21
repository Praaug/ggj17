using UnityEngine;
using System.Collections;

public static class CombatManager
{
    public static void PerformAttack( Player p_attackingPlayer, Enemy p_targetEnemy, CombatHit p_hitInfo )
    {
        p_targetEnemy.InflictDamage( p_attackingPlayer, p_hitInfo.damage );
    }
}

public class CombatHit
{
    public float damage;
}
