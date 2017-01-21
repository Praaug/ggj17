using UnityEngine;
using System.Collections;

public class RagdollController : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private CharacterJoint[] m_joints = null;
    #endregion

    #region Methods
    public void TriggerRagdoll()
    {
        for ( int i = 0; i < m_joints.Length; i++ )
        {
            m_joints[ i ].GetComponent<Collider>().isTrigger = false;
            Rigidbody _rb = m_joints[ i ].GetComponent<Rigidbody>();
            _rb.isKinematic = false;
            _rb.useGravity = true;
        }

    }
    #endregion
}
