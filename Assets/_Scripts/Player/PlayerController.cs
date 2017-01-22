using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    #region Events
    public event System.Action OnFrontSwingStart;
    public event System.Action OnFrontSwingFinish;
    #endregion

    #region Properties
    [Debug]
    public bool isAttacking { get; private set; }
    public Player player { get; private set; }
    #endregion

    #region Fields
    [SerializeField, Category( "References" )]
    private Animator m_animator = null;
    [SerializeField, Category( "Stats" )]
    private float m_attacksPerSec = 0.0f;

    private float m_lastAttackTimestamp = 0.0f;
    #endregion

    #region Methods
    private void Awake()
    {
        m_lastAttackTimestamp = float.MinValue;

        if ( m_animator == null )
            this.AssignComponent( out m_animator );
        if ( m_animator == null )
            Dbg.LogError( gameObject, "There is not animator component on the gameobject {0}", gameObject.name );
    }

    private void Start()
    {
        player = Player.GetPlayer( gameObject );
    }

    private void FixedUpdate()
    {
        if ( InputUtility.GetFixedButtonDown( InputButton.Attack, player.inputSource ) )
        {
            if ( !GGJMath.TimeCheck( m_lastAttackTimestamp, Time.time, m_attacksPerSec ) )
            {
                // Init Attack anim
                isAttacking = false;
                m_animator.SetTrigger( "attack01" );

                // Track timestamp
                m_lastAttackTimestamp = Time.time;
            }
        }
    }

    private void AnimEvnt_OnFrontSwingStart()
    {
        isAttacking = true;

        if ( OnFrontSwingStart != null )
            OnFrontSwingStart();
    }

    private void AnimEvnt_OnFrontSwingFinish()
    {
        isAttacking = false;

        if ( OnFrontSwingFinish != null )
            OnFrontSwingFinish();
    }

    #endregion
}
