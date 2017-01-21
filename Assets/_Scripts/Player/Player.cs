using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class Player
{
    #region Properties
    public PlayerConfig config { get; private set; }
    public GameObject gameObject { get; private set; }

    public int elePtsFire { get; private set; }
    public int elePtsWater { get; private set; }
    public int elePtsStorm { get; private set; }
    public int elePtsEarth { get; private set; }
    #endregion

    #region Constructor
    public Player( GameObject p_avatarInstance )
    {
        gameObject = p_avatarInstance;
        config = p_avatarInstance.GetComponent<PlayerConfig>();
    }

    public void ReduceLife( int p_amount )
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
            allPlayer.Add( new Player( _playerInstance ) );
        }

        return allPlayer;
    }
}
