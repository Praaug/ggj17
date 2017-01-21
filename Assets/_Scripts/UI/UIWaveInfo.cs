using UnityEngine;
using System.Collections;
using UnityEngine.UI.Extensions;

public class UIWaveInfo : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private UILineRenderer m_wave = null;
    [SerializeField]
    private int m_elePoints = 0;
    [SerializeField]
    private int m_otherPlayerStrengh = 0;
    [SerializeField]
    private WaveInfo m_waveInfo = null;
    [SerializeField]
    private GameObject[] m_selector = null;

    private Player m_player = null;
    private int m_currentIndex = -1;
    #endregion


    #region Methods
    public void Init( Player p_player )
    {
        m_player = p_player;

        foreach ( GameObject _go in m_selector )
            _go.SetActive( false );
    }

    /// <summary>
    /// Increments the selector
    /// </summary>
    /// <returns>true if the increment was successfull</returns>
    public bool Increment()
    {
        if ( m_currentIndex == -1 )
        {
            m_currentIndex = 0;
            m_selector[ m_currentIndex ].SetActive( true );
            return true;
        }

        if ( m_currentIndex >= 0 && m_currentIndex < m_selector.Length - 1 )
        {
            m_selector[ m_currentIndex ].SetActive( false );
            m_currentIndex++;
            m_selector[ m_currentIndex ].SetActive( true );
            return true;
        }

        m_selector[ m_currentIndex ].SetActive( false );
        m_currentIndex = -1;
        return false;
    }

    /// <summary>
    /// Increments the selector
    /// </summary>
    /// <returns>true if the decrement was successfull</returns>
    public bool Decrement()
    {
        if ( m_currentIndex == -1 )
        {
            m_currentIndex = m_selector.Length;
            m_selector[ m_currentIndex ].SetActive( true );
            return true;
        }

        if ( m_currentIndex > 0 && m_currentIndex < m_selector.Length )
        {
            m_selector[ m_currentIndex ].SetActive( false );
            m_currentIndex--;
            m_selector[ m_currentIndex ].SetActive( true );
            return true;
        }

        m_selector[ m_currentIndex ].SetActive( false );
        m_currentIndex = -1;
        return false;
    }
    #endregion
}
