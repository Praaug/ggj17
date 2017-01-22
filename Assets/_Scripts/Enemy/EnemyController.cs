using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    #region Properties
    public Enemy enemy { get; private set; }
    #endregion

    #region Fields
    [SerializeField, Category( "References" )]
    private RagdollController m_ragdollController = null;

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
    }

    private void OnDestroy()
    {
        if ( enemy != null )
            enemy.OnKill -= Enemy_OnKill;
    }

    private void Enemy_OnKill()
    {
        // Trigger Ragdoll
        if ( m_ragdollController != null )
            m_ragdollController.TriggerRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        if ( enemy.isDead )
            return;
    }
    #endregion
}
