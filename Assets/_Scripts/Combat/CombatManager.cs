using UnityEngine;
using System.Collections;

public static class CombatManager
{
    public static void PerformAttack( Player p_attackingPlayer, ICombatEntity p_targetEntity, CombatHit p_hitInfo )
    {
        p_targetEntity.InflictDamage( p_hitInfo.damage );
    }
}

public class CombatHit
{
    public float damage;
}
