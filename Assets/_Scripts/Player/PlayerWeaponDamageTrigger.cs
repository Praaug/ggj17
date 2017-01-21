using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerWeaponDamageTrigger : MonoBehaviour
{
    #region Fields
    [SerializeField, Category( "References" )]
    private PlayerController m_controller = null;
    [SerializeField, Category( "Stats" )]
    private float m_damagePerHit = 0;

    private List<Enemy> m_enemyHitList = new List<Enemy>();
    #endregion

    #region Methods
    private void Awake()
    {
        if ( m_controller == null )
            this.AssignComponentInParent( out m_controller );
        if ( m_controller == null )
        {
            Dbg.LogError( gameObject, "No PlayerController controller found in parent of {0}", gameObject.name );
            return;
        }

        m_controller.OnFrontSwingStart += Controller_OnFrontSwingStart;
        m_controller.OnFrontSwingFinish += Controller_OnFrontSwingFinish;
    }

    private void OnDestroy()
    {
        if ( m_controller != null )
        {
            m_controller.OnFrontSwingStart -= Controller_OnFrontSwingStart;
            m_controller.OnFrontSwingFinish -= Controller_OnFrontSwingFinish;
        }
    }

    private void Controller_OnFrontSwingStart()
    {
        m_enemyHitList.Clear();
    }

    private void Controller_OnFrontSwingFinish()
    {
        m_enemyHitList.Clear();
    }

    private void OnTriggerStay( Collider p_collider )
    {
        Enemy _enemy;
        if ( !m_controller.isAttacking || Enemy.IsEnemy( p_collider, out _enemy ) )
            return;

        CombatHit _hitInfo = new CombatHit();
        _hitInfo.damage = m_damagePerHit;

        CombatManager.PerformAttack( m_controller.player, _enemy, _hitInfo );
    }
    #endregion
}
