using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThirdPerson : MonoBehaviour
{
    public enum UpdateMode
    {
        Update,
        LateUpdate,
        FixedUpdate
    }

    #region Fields
    [SerializeField]
    private UpdateMode m_updateMode = UpdateMode.FixedUpdate;
    [SerializeField, Category( "Target" )]
    private Transform m_target = null;
    [SerializeField, Category( "Target" )]
    private float m_yOffset = 0.0f;
    [SerializeField, Category( "Movement" )]
    private float m_movementSpeed = 0.0f;
    [SerializeField, Category( "Movement" )]
    private float m_scrollSpeed = 0.0f;

    [SerializeField, Category( "References" )]
    private Camera m_camera = null;
    [SerializeField, Category( "Rotation" )]
    private float m_rotationSpeedX = 0.0f;
    [SerializeField, Category( "Rotation" )]
    private float m_rotationSpeedY = 0.0f;
    [SerializeField, Category( "Rotation" )]
    private bool m_invertY = false;

    [SerializeField]
    private bool m_controlTargetRotation = true;
    #endregion

    #region Methods
    private void Update()
    {
        if ( m_updateMode != UpdateMode.Update )
            return;

        TickUpdate();
    }

    private void LateUpdate()
    {
        if ( m_updateMode != UpdateMode.LateUpdate )
            return;

        TickUpdate();
    }

    private void FixedUpdate()
    {
        if ( m_updateMode != UpdateMode.FixedUpdate )
            return;

        TickUpdate();
    }

    private void TickUpdate()
    {
        // Apply rotation to camera
        UpdateRotation();

        // Move camera
        UpdateMovement();

        // UPdate the zoom of the camera
        UpdateZoom();
    }

    private void UpdateRotation()
    {
        float _hor = InputUtility.GetAxis( Axis.MouseX );
        float _ver = InputUtility.GetAxis( Axis.MouseY );

        transform.rotation *= Quaternion.AngleAxis( ( m_invertY ? 1 : -1 ) * _ver * m_rotationSpeedY * Time.deltaTime, Vector3.right );

        if ( !m_controlTargetRotation )
            transform.rotation = Quaternion.AngleAxis( _hor * m_rotationSpeedX * Time.deltaTime, Vector3.up ) * transform.rotation;
        else
        {
            transform.rotation = Quaternion.AngleAxis( _hor * m_rotationSpeedX * Time.deltaTime, Vector3.up ) * transform.rotation;
            Vector3 _eulerAngles = new Vector3( 0.0f, transform.rotation.eulerAngles.y, 0.0f );
            m_target.transform.eulerAngles = _eulerAngles;
        }
    }

    private void UpdateMovement()
    {
        transform.position = Vector3.MoveTowards( transform.position, m_target.position + Vector3.up * m_yOffset, m_movementSpeed * Time.deltaTime );
    }

    private void UpdateZoom()
    {
        float _zoomDelta = InputUtility.GetAxis( Axis.MouseScrollwheel );

        m_camera.transform.position += transform.forward * _zoomDelta * m_scrollSpeed * Time.deltaTime;
    }
    #endregion
}
