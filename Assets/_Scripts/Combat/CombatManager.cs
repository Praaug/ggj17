using UnityEngine;
using System.Collections;

public static class CombatManager
{
    public static void PerformAttack( ICombatEntity p_attackingEntity, ICombatEntity p_targetEntity, CombatHit p_hitInfo )
    {
        p_targetEntity.InflictDamage( p_hitInfo.damage );
    }
}

public class CombatHit
{
    public float damage;
}
