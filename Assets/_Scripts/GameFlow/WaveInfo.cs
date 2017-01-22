using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WaveInfo
{
    #region Fields
    public Dictionary<ElementType, int>[] m_amplitudes;
    private Player m_player;
    #endregion

    #region Constructor
    public WaveInfo( Player p_player )
    {
        m_player = p_player;
        m_amplitudes = new Dictionary<ElementType, int>[ 3 ];
        for ( int i = 0; i < m_amplitudes.Length; i++ )
        {
            m_amplitudes[ i ] = new Dictionary<ElementType, int>();
            m_amplitudes[ i ].Add( ElementType.Fire, 0 );
            m_amplitudes[ i ].Add( ElementType.Air, 0 );
            m_amplitudes[ i ].Add( ElementType.Dirt, 0 );
            m_amplitudes[ i ].Add( ElementType.Water, 0 );
        }
    }
    public WaveInfo( Player p_player, WaveInfo p_waveInfo ) : this( p_player )
    {
        m_player = p_player;

        for ( int i = 0; i < m_amplitudes.Length; i++ )
        {
            m_amplitudes[ i ][ ElementType.Fire ] = p_waveInfo.m_amplitudes[ i ][ ElementType.Fire ];
            m_amplitudes[ i ][ ElementType.Air ] = p_waveInfo.m_amplitudes[ i ][ ElementType.Air ];
            m_amplitudes[ i ][ ElementType.Dirt ] = p_waveInfo.m_amplitudes[ i ][ ElementType.Dirt ];
            m_amplitudes[ i ][ ElementType.Water ] = p_waveInfo.m_amplitudes[ i ][ ElementType.Water ];
        }
    }
    #endregion

    #region Methods
    public int GetAmplitudeCount( ElementType p_type, int p_frequencyIndex )
    {
        return m_amplitudes[ p_frequencyIndex ][ p_type ];
    }

    public int GetTotalAmplitude( ElementType p_type )
    {
        int _totalAmp = 0;

        for ( int i = 0; i < m_amplitudes.Length; i++ )
            _totalAmp += m_amplitudes[ i ][ p_type ];

        return _totalAmp;
    }

    public List<EnemyInfo> CalculatePrefabs()
    {
        List<EnemyInfo> _resultList = new List<EnemyInfo>();

        for ( int i = 0; i < m_amplitudes.Length; i++ )
        {
            _resultList.AddRange( CalculateElement( m_amplitudes[ i ][ ElementType.Fire ], GameInfo.instance.elementPrefabDict[ ElementType.Fire ] ) );
            _resultList.AddRange( CalculateElement( m_amplitudes[ i ][ ElementType.Water ], GameInfo.instance.elementPrefabDict[ ElementType.Water ] ) );
            _resultList.AddRange( CalculateElement( m_amplitudes[ i ][ ElementType.Air ], GameInfo.instance.elementPrefabDict[ ElementType.Air ] ) );
            _resultList.AddRange( CalculateElement( m_amplitudes[ i ][ ElementType.Dirt ], GameInfo.instance.elementPrefabDict[ ElementType.Dirt ] ) );
        }

        _resultList.Shuffle();

        return _resultList;
    }

    private List<EnemyInfo> CalculateElement( int p_count, EnemyInfo[] p_prefabs )
    {
        int _restCount = p_count;
        List<EnemyInfo> _resultList = new List<EnemyInfo>();

        while ( _restCount > 0 )
        {
            EnemyInfo _newItem = p_prefabs.RandomItem( info => info.elementPointValue <= _restCount );
            if ( _newItem == null )
                break;

            _resultList.Add( _newItem );
            _restCount -= _newItem.elementPointValue;
        }

        return _resultList;
    }

    public void ClearElementCount()
    {
        for ( int i = 0; i < m_amplitudes.Length; i++ )
        {
            m_amplitudes[ i ][ ElementType.Fire ] = 0;
            m_amplitudes[ i ][ ElementType.Water ] = 0;
            m_amplitudes[ i ][ ElementType.Air ] = 0;
            m_amplitudes[ i ][ ElementType.Dirt ] = 0;
        }
    }

    public int PointsNeededForLevelUp( ElementType p_elementType, int p_frequencyIndex )
    {
        return PointsNeededForLevelUp( m_amplitudes[ p_frequencyIndex ][ p_elementType ] );
    }

    public int MaximumPossibleAmplitude()
    {
        int _tmpMax = 0;

        foreach ( ElementType _elementType in EnumUtility.GetValues<ElementType>() )
        {
            int _tmpAmplitude = 0; // Amplitude inside 
            int _tmpPointsUsed = 0;

            while ( PointsNeededForLevelUp( _tmpAmplitude ) <= m_player.elementPointsDict[ _elementType ] - _tmpPointsUsed )
            {
                _tmpPointsUsed += PointsNeededForLevelUp( _tmpAmplitude );
                _tmpAmplitude++;
            }

            if ( _tmpAmplitude > _tmpMax )
                _tmpMax = _tmpAmplitude;
        }

        return _tmpMax;
    }

    private int PointsNeededForLevelUp( int p_level )
    {
        return Mathf.FloorToInt( Mathf.Pow( 1.4f, p_level ) );
    }

    public void IncrementElementCount( ElementType p_type, int p_frequencyIndex )
    {
        m_player.RemoveElementPoints( ElementType.Fire, PointsNeededForLevelUp( m_amplitudes[ p_frequencyIndex ][ p_type ] ) );
        m_amplitudes[ p_frequencyIndex ][ p_type ]++;
    }

    public void DecrementElementCount( ElementType p_type, int p_frequencyIndex )
    {
        m_amplitudes[ p_frequencyIndex ][ p_type ]--;
        m_player.AddElementPoints( p_type, PointsNeededForLevelUp( m_amplitudes[ p_frequencyIndex ][ p_type ] ) );
    }
    #endregion
}
