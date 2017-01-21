using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    #region Fields
    [SerializeField, Category( "References" )]
    private Transform m_fightTransform = null;
    [SerializeField, Category( "References" )]
    private Transform m_waveSelectTransform = null;

    [SerializeField, Category( "Stats" ), Range( 0.0f, 1.0f )]
    private float m_lerpFactor = 0.0f;

    private Coroutine m_transitCoroutine;
    #endregion

    #region Methods
    private void Start()
    {
        GameInfo.instance.OnCurrentGamePhaseChange += Instance_OnCurrentGamePhaseChange;
    }

    private void Instance_OnCurrentGamePhaseChange()
    {
        switch ( GameInfo.instance.currentGamePhase )
        {
            case GameInfo.GamePhase.PreGame:
                break;
            case GameInfo.GamePhase.WaveBuilding:
                TransitToTransform( m_waveSelectTransform );
                break;
            case GameInfo.GamePhase.Fight:
                TransitToTransform( m_fightTransform );
                break;
            default:
                break;
        }
    }

    private void TransitToTransform( Transform p_targetTransform )
    {
        if ( m_transitCoroutine != null )
            StopCoroutine( m_transitCoroutine );

        m_transitCoroutine = StartCoroutine( Coroutine_TransitToTransform( p_targetTransform ) );
    }

    private IEnumerator Coroutine_TransitToTransform( Transform p_targetTransform )
    {

        while ( !GGJMath.DistanceCheck( transform.position, p_targetTransform.position, 0.1f * Time.deltaTime ) )
        {
            transform.position = Vector3.Lerp( transform.position, p_targetTransform.position, m_lerpFactor );
            transform.rotation = Quaternion.Lerp( transform.rotation, p_targetTransform.rotation, m_lerpFactor );

            yield return null;
        }

        transform.position = p_targetTransform.position;
        transform.rotation = p_targetTransform.rotation;
    }
    #endregion
}
