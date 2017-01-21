using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UIPlanning : MonoBehaviour
{
    #region Fields
    [SerializeField, Category( "Selection" )]
    private UIWaveInfo[] m_waveInfos = null;

    private Player m_player = null;
    private UIWaveInfo m_currentWaveInfo = null;
    private int m_currentIndex = 0;
    #endregion

    #region Methods
    public void Awake()
    {
        m_currentWaveInfo = m_waveInfos[ 0 ];
        m_currentWaveInfo.Increment();
    }

    public void Init( Player p_player )
    {
        m_player = p_player;
        foreach ( UIWaveInfo _wi in m_waveInfos )
            _wi.Init( m_player );
    }

    public void FixedUpdate()
    {
        //if ( InputUtility.GetButtonDown( InputButton.MenuRight, m_player.inputSource ) )
        //    IncreaseSelector();
        //if ( InputUtility.GetButtonDown( InputButton.MenuLeft, m_player.inputSource ) )
        //    DecreaseSelector();
        if ( Input.GetKeyDown( KeyCode.D ) )
            IncreaseSelector();
        if ( Input.GetKeyDown( KeyCode.A ) )
            DecreaseSelector();
    }

    private void IncreaseSelector()
    {
        if ( !m_currentWaveInfo.Increment() )
        {
            m_currentIndex = ( m_currentIndex + 1 ) % m_waveInfos.Length;
            m_currentWaveInfo = m_waveInfos[ m_currentIndex ];
            IncreaseSelector();
        }
    }

    private void DecreaseSelector()
    {
        if ( !m_currentWaveInfo.Decrement() )
        {
            m_currentIndex = ( ( m_currentIndex % m_waveInfos.Length ) + m_waveInfos.Length - 1 ) % m_waveInfos.Length;
            m_currentWaveInfo = m_waveInfos[ m_currentIndex ];
            DecreaseSelector();
        }
    }
    #endregion
}
