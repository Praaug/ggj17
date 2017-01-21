using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI.Extensions;

public class UIWaveInfo : MonoBehaviour
{
    #region Consts
    private const int PART_01_INDEX = 1;
    private const int PART_02_INDEX = 3;
    private const int PART_03_INDEX = 5;
    private const float PART_01_X = 0.1666f;
    private const float PART_02_X = 0.4998f;
    private const float PART_03_X = 0.833f;

    #endregion

    #region Fields
    [SerializeField]
    private ElementType m_type = ElementType.Fire;
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
    public void Awake()
    {
        foreach ( GameObject _go in m_selector )
            _go.SetActive( false );

        m_wave.Points[ PART_01_INDEX ] = new Vector2( PART_01_X, 0.0f );
        m_wave.Points[ PART_02_INDEX ] = new Vector2( PART_02_X, 0.0f );
        m_wave.Points[ PART_03_INDEX ] = new Vector2( PART_03_X, 0.0f );
    }

    public void Init( Player p_player )
    {
        m_player = p_player;
        m_waveInfo = m_player.waveInfo;
        m_elePoints = m_player.elementPointsDict[ m_type ];
    }

    /// <summary>
    /// Increments the selector
    /// </summary>
    /// <returns>true if the increment was successfull</returns>
    public bool IncrementSelector()
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
    public bool DecrementSelector()
    {
        if ( m_currentIndex == -1 )
        {
            m_currentIndex = m_selector.Length - 1;
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

    public void IncrementStat()
    {
        int _pointsNeeded = m_waveInfo.PointsNeededForLevelUp( m_type, m_currentIndex );
        if ( m_elePoints > _pointsNeeded )
        {
            m_player.RemoveElementPoints( m_type, _pointsNeeded );
            m_waveInfo.IncrementElementCount( m_type, m_currentIndex );
        }
    }

    public void DecrementStat()
    {
        m_waveInfo.DecrementElementCount( m_type, m_currentIndex );
    }
    #endregion
}
