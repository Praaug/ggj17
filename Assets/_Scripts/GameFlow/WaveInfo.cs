using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WaveInfo
{
    public int fireCount;
    public int waterCount;
    public int windCount;
    public int dirtCount;

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
}
