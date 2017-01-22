using UnityEngine;
using System.Collections;

public class RagdollController : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private GameObject m_base = null;
    [SerializeField]
    private CharacterJoint[] m_joints = null;
    #endregion

    #region Methods
    public void TriggerRagdoll()
    {
        Animator _animator = GetComponent<Animator>();
        if ( _animator != null )
            _animator.enabled = false;

        CharacterController _cc = GetComponent<CharacterController>();
        if ( _cc )
            _cc.enabled = false;

        Rigidbody _rb;

        for ( int i = 0; i < m_joints.Length; i++ )
        {
            m_joints[ i ].gameObject.layer = LayerMask.NameToLayer( "Ragdoll" );
            m_joints[ i ].GetComponent<Collider>().isTrigger = false;
            _rb = m_joints[ i ].GetComponent<Rigidbody>();
            _rb.isKinematic = false;
            _rb.useGravity = true;
        }

        m_base.layer = LayerMask.NameToLayer( "Ragdoll" );
        m_base.GetComponent<Collider>().isTrigger = false;
        _rb = m_base.GetComponent<Rigidbody>();
        _rb.isKinematic = false;
        _rb.useGravity = true;
    }
    #endregion
}
