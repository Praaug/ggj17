using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WaveInfo
{
    #region Fields
    public int fireCount;
    public int waterCount;
    public int windCount;
    public int dirtCount;

    private Player m_player;
    #endregion

    #region Constructor
    public WaveInfo( Player p_player )
    {
        m_player = p_player;
    }
    public WaveInfo( Player p_player, WaveInfo p_waveInfo )
    {
        m_player = p_player;

        fireCount = p_waveInfo.fireCount;
        waterCount = p_waveInfo.waterCount;
        windCount = p_waveInfo.windCount;
        dirtCount = p_waveInfo.dirtCount;
    }
    #endregion

    #region Methods
    public List<EnemyInfo> CalculatePrefabs()
    {
        List<EnemyInfo> _resultList = new List<EnemyInfo>();

        _resultList.AddRange( CalculateElement( fireCount, GameInfo.instance.elementPrefabDict[ ElementType.Fire ] ) );
        _resultList.AddRange( CalculateElement( waterCount, GameInfo.instance.elementPrefabDict[ ElementType.Water ] ) );
        _resultList.AddRange( CalculateElement( windCount, GameInfo.instance.elementPrefabDict[ ElementType.Air ] ) );
        _resultList.AddRange( CalculateElement( dirtCount, GameInfo.instance.elementPrefabDict[ ElementType.Dirt ] ) );

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

    public int PointsNeededForLevelUp( ElementType p_elementType )
    {
        switch ( p_elementType )
        {
            case ElementType.Fire:
                return PointsNeededForLevelUp( fireCount );
            case ElementType.Water:
                return PointsNeededForLevelUp( waterCount );
            case ElementType.Air:
                return PointsNeededForLevelUp( windCount );
            case ElementType.Dirt:
                return PointsNeededForLevelUp( dirtCount );
            default:
                throw new System.ArgumentException();
        }
    }

    private int PointsNeededForLevelUp( int p_level )
    {
        return Mathf.FloorToInt( Mathf.Pow( 1.4f, p_level ) );
    }

    public void IncrementElementCount( ElementType p_type )
    {
        switch ( p_type )
        {
            case ElementType.Fire:
                m_player.RemoveElementPoints( ElementType.Fire, PointsNeededForLevelUp( ElementType.Fire ) );
                fireCount++;
                break;
            case ElementType.Water:
                m_player.RemoveElementPoints( ElementType.Water, PointsNeededForLevelUp( ElementType.Water ) );
                waterCount++;
                break;
            case ElementType.Air:
                m_player.RemoveElementPoints( ElementType.Air, PointsNeededForLevelUp( ElementType.Air ) );
                windCount++;
                break;
            case ElementType.Dirt:
                m_player.RemoveElementPoints( ElementType.Dirt, PointsNeededForLevelUp( ElementType.Dirt ) );
                dirtCount++;
                break;
            default:
                throw new System.ArgumentException();
        }
    }
    #endregion
}
