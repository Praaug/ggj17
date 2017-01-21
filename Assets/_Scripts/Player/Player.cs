using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class Player : ICombatEntity
{
    #region Properties
    public PlayerConfig config { get; private set; }
    public GameObject gameObject { get; private set; }
    public InputSource inputSource { get; private set; }

    public int lives { get; private set; }

    public int elePtsFire { get; private set; }
    public int elePtsWater { get; private set; }
    public int elePtsStorm { get; private set; }
    public int elePtsEarth { get; private set; }
    #endregion

    #region Constructor
    public Player( GameObject p_avatarInstance, InputSource p_inputSource )
    {
        gameObject = p_avatarInstance;
        config = p_avatarInstance.GetComponent<PlayerConfig>();
        inputSource = p_inputSource;
    }

    public void ReduceLife( int p_amount )
    {

    }

    public void InflictDamage( float p_amount )
    {

    }
    #endregion
}

public partial class Player
{
    public const int PLAYER_COUNT = 2;

    public static List<Player> allPlayer { get; private set; }

    public static List<Player> CreatePlayer( GameObject p_playerPrefab )
    {
        allPlayer = new List<Player>();

        for ( int i = 0; i < PLAYER_COUNT; i++ )
        {
            // Create player avatar
            GameObject _playerInstance = Object.Instantiate( p_playerPrefab );

            // Create player object with reference to avatar
            allPlayer.Add( new Player( _playerInstance, i == 0 ? InputSource.Player1 : InputSource.Player2 ) );
        }

        return allPlayer;
    }

    public static Player GetPlayer( GameObject p_gameObject )
    {
        return allPlayer.FirstOrDefault( p => p.gameObject == p_gameObject );
    }
}
