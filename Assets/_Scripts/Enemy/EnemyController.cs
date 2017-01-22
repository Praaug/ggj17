using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    #region Properties
    public Enemy enemy { get; private set; }
    public Transform healthbarTransform { get { return m_healthbarTransform; } }
    #endregion

    #region Fields
    [SerializeField, Category( "References" )]
    private RagdollController m_ragdollController = null;
    [SerializeField, Category( "References" )]
    private Transform m_healthbarTransform = null;
    private Healthbar m_healthbar;

    #endregion

    #region Methods
    // Use this for initialization
    void Start()
    {
        enemy = Enemy.GetEnemy( gameObject );

        if ( enemy == null )
        {
            Dbg.LogError( gameObject, "Enemy not found for an EnemyController" );
            return;
        }

        enemy.OnKill += Enemy_OnKill;
        enemy.OnInflictDamage += Enemy_OnInflictDamage;

        m_healthbar = GameInfo.instance.CreateHealthbar( enemy );
    }

    private void Enemy_OnInflictDamage()
    {
        m_healthbar.SetSlider( enemy.health / enemy.info.maxHealth );
    }

    private void OnDestroy()
    {
        if ( enemy != null )
        {
            enemy.OnKill -= Enemy_OnKill;
            enemy.OnInflictDamage -= Enemy_OnInflictDamage;
        }

        if ( m_healthbar != null )
            Destroy( m_healthbar.gameObject );
        m_healthbar = null;
    }

    private void Enemy_OnKill()
    {
        // Trigger Ragdoll
        if ( m_ragdollController != null )
            m_ragdollController.TriggerRagdoll();

        if ( m_healthbar != null )
            Destroy( m_healthbar.gameObject );
        m_healthbar = null;
    }

    // Update is called once per frame
    void Update()
    {
        if ( enemy.isDead )
            return;
    }
    #endregion
}
