using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    #region Properties
    [Debug]
    public bool isGrounded { get; private set; }

    private Vector3 CCSphereCenterLower { get { return transform.position + m_cc.center + Vector3.up * ( -m_cc.height * 0.5f + m_cc.radius ); } }
    private Vector3 CCSphereCenterUpper { get { return transform.position + m_cc.center + Vector3.up * ( m_cc.height * 0.5f + m_cc.radius ); } }
    #endregion

    #region Fields
    [SerializeField, Category( "References" )]
    private CharacterController m_cc = null;
    [SerializeField, Category( "Movement" )]
    private float m_movementSpeed = 0.0f;
    [SerializeField, Category( "Rotation" )]
    private float m_rotationSpeed = 0.0f;

    private float m_currentGravityForce = 0.0f;
    #endregion

    #region Methods
    void Start()
    {

    }

    private void FixedUpdate()
    {
        Vector3 _deltaTotal = Vector3.zero;

        UpdateRotation();

        _deltaTotal += UpdateMovement();

        _deltaTotal += UpdateGravitiy();

        ExecuteMove( _deltaTotal );
    }

    private void UpdateRotation()
    {
        float _hor = InputUtility.GetAxis( Axis.Horizontal );

        transform.rotation *= Quaternion.AngleAxis( _hor * m_rotationSpeed * Time.deltaTime, Vector3.up );
    }

    private Vector3 UpdateMovement()
    {
        float _ver = InputUtility.GetAxis( Axis.Vertical );

        Vector3 _deltaTotal = Vector3.zero;

        Vector3 _deltaMovement = Vector3.zero;
        _deltaMovement += _ver * m_movementSpeed * Time.deltaTime * transform.forward;

        _deltaMovement = Vector3.ClampMagnitude( _deltaMovement, m_movementSpeed * Time.deltaTime );

        _deltaTotal += _deltaMovement;

        // Apply delta movement
        return _deltaTotal;
    }

    private Vector3 UpdateGravitiy()
    {
        m_currentGravityForce += Time.deltaTime * Physics.gravity.y;

        return m_currentGravityForce * Time.deltaTime * Vector3.up;
    }

    private void ExecuteMove( Vector3 p_deltaTotal )
    {
        isGrounded = m_cc.Move( p_deltaTotal ) == CollisionFlags.Below;
        isGrounded |= Physics.CheckSphere( CCSphereCenterLower, m_cc.radius, -1, QueryTriggerInteraction.Ignore );

        isGrounded = m_cc.isGrounded;
    }
    #endregion
}
