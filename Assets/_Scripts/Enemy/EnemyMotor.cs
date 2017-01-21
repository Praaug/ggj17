using UnityEngine;
using System.Collections;

public class EnemyMotor : MonoBehaviour
{
    #region Properties
    private Enemy enemy { get { return m_controller.enemy; } }
    [Debug]
    public bool isGrounded { get; private set; }

    private Vector3 CCSphereCenterLower { get { return transform.position + m_cc.center + Vector3.up * ( -m_cc.height * 0.5f + m_cc.radius ); } }
    private Vector3 CCSphereCenterUpper { get { return transform.position + m_cc.center + Vector3.up * ( m_cc.height * 0.5f + m_cc.radius ); } }
    #endregion

    #region Fields
    [SerializeField, Category( "References" )]
    private Animator m_animator = null;
    [SerializeField, Category( "References" )]
    private CharacterController m_cc = null;
    [SerializeField, Category( "References" )]
    private EnemyController m_controller = null;
    [SerializeField, Category( "Movement" )]
    private float m_movementSpeed = 0.0f;

    private float m_currentGravityForce = 0.0f;
    #endregion

    #region Methods
    private void Awake()
    {
        if ( m_cc == null )
            this.AssignComponent( out m_cc );
        if ( m_cc == null )
            Dbg.LogError( gameObject, "No character controller component found on {0}", gameObject.name );

        if ( m_controller == null )
            this.AssignComponent( out m_controller );
        if ( m_controller == null )
            Dbg.LogError( gameObject, "No EnemyController component found on {0}", gameObject.name );

    }

    private void FixedUpdate()
    {
        Vector3 _deltaMovement = Vector3.zero;

        if ( !enemy.isDead )
            _deltaMovement += UpdateMovement();

        _deltaMovement += UpdateGravitiy();

        ExecuteMove( _deltaMovement );

        UpdateAnimator();
    }

    private Vector3 UpdateMovement()
    {
        // Move forward
        return transform.forward * m_movementSpeed * Time.deltaTime;
    }

    private Vector3 UpdateGravitiy()
    {
        if ( isGrounded )
            m_currentGravityForce = 0.0f;

        m_currentGravityForce += Time.deltaTime * Physics.gravity.y;

        return m_currentGravityForce * Time.deltaTime * Vector3.up;
    }

    private void ExecuteMove( Vector3 p_deltaTotal )
    {
        isGrounded = m_cc.Move( p_deltaTotal ) == CollisionFlags.Below;
        isGrounded |= m_cc.isGrounded;
        isGrounded |= Physics.CheckSphere( CCSphereCenterLower, m_cc.radius, 1 << LayerMask.NameToLayer( "Ground" ), QueryTriggerInteraction.Ignore );
    }

    private void UpdateAnimator()
    {
        m_animator.SetFloat( "speed", m_cc.velocity.magnitude );
    }
    #endregion
}
