using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;

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
    private Text m_elePointsAvailableText = null;
    [SerializeField]
    private Text m_eleBonusDamageText = null;
    [SerializeField]
    private ElementType m_type = ElementType.Fire;
    [SerializeField]
    private UILineRenderer m_wave = null;
    [SerializeField]
    private int m_elePoints = 0;
    [SerializeField]
    private GameObject[] m_selector = null;

    private Player m_player = null;
    private int m_currentIndex = -1;
    private float m_maxAmplitudeHeight = 0.0f;
    #endregion


    #region Methods
    public void Awake()
    {
        foreach ( GameObject _go in m_selector )
            _go.SetActive( false );

        Vector2[] _points = m_wave.Points;
        _points[ PART_01_INDEX ] = new Vector2( PART_01_X, 0.0f );
        _points[ PART_02_INDEX ] = new Vector2( PART_02_X, 0.0f );
        _points[ PART_03_INDEX ] = new Vector2( PART_03_X, 0.0f );
        m_wave.Points = _points;
        m_wave.SetAllDirty();
    }

    private void OnEnable()
    {
        if ( m_player == null )
            return;

        Vector2[] _points = m_wave.Points;
        for ( int i = 0; i < _points.Length; i++ )
            _points[ i ] = new Vector2( _points[ i ].x, 0.0f );
        m_wave.Points = _points;

        m_eleBonusDamageText.text = ( (int)m_player.elementBuffDict[ m_type ] ).ToString0000();

        m_maxAmplitudeHeight = Mathf.Max( m_player.waveInfo.MaximumPossibleAmplitude(), m_player.otherPlayer.waveInfo.MaximumPossibleAmplitude() );
    }

    public void Init( Player p_player )
    {
        m_player = p_player;
        m_player.OnElementPointsChange += Player_OnElementPointsChange;
        OnEnable();
    }

    private void Player_OnElementPointsChange( ElementType p_type )
    {
        if ( m_type != p_type )
            return;

        m_elePoints = m_player.elementPointsDict[ m_type ];
        m_elePointsAvailableText.text = m_elePoints.ToString000();
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
        int _pointsNeeded = m_player.waveInfo.PointsNeededForLevelUp( m_type, m_currentIndex );
        if ( m_elePoints >= _pointsNeeded )
        {
            m_player.waveInfo.IncrementElementCount( m_type, m_currentIndex );

            Vector2[] _points = m_wave.Points;
            _points[ GetIndex() ] = new Vector2( GetXValue(), GetAmplitudeHeight() );
            m_wave.Points = _points;
        }
    }

    public void DecrementStat()
    {
        if ( m_player.waveInfo.DecrementElementCount( m_type, m_currentIndex ) )
        {
            Vector2[] _points = m_wave.Points;
            _points[ GetIndex() ] = new Vector2( GetXValue(), GetAmplitudeHeight() );
            m_wave.Points = _points;
        }
    }

    private int GetIndex()
    {
        return m_currentIndex == 0 ? PART_01_INDEX : m_currentIndex == 1 ? PART_02_INDEX : PART_03_INDEX;
    }
    private float GetXValue()
    {
        return m_currentIndex == 0 ? PART_01_X : m_currentIndex == 1 ? PART_02_X : PART_03_X;
    }
    private float GetAmplitudeHeight()
    {
        return m_player.waveInfo.GetAmplitudeCount( m_type, m_currentIndex ) / m_maxAmplitudeHeight;
    }
    #endregion
}
