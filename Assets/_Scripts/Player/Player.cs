﻿using UnityEngine;
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

    public int health { get; private set; }

    public Dictionary<EnemyType, int> enemyCountDict { get { return m_enemyCountDict; } }
    #endregion

    #region Fields
    private Dictionary<EnemyType, int> m_enemyCountDict;
    private Dictionary<EnemyType, int> m_enemyStrenghDict;
    #endregion

    #region Constructor
    public Player( GameObject p_avatarInstance )
    {
        gameObject = p_avatarInstance;
        config = p_avatarInstance.GetComponent<PlayerConfig>();

        m_enemyCountDict = new Dictionary<EnemyType, int>();
        m_enemyCountDict.Add( EnemyType.Fire, 0 );
        m_enemyCountDict.Add( EnemyType.Water, 0 );
        m_enemyCountDict.Add( EnemyType.Wind, 0 );
        m_enemyCountDict.Add( EnemyType.Dirt, 0 );

        m_enemyStrenghDict = new Dictionary<EnemyType, int>();
        m_enemyStrenghDict.Add( EnemyType.Fire, MIN_DAMAGE );
        m_enemyStrenghDict.Add( EnemyType.Water, MIN_DAMAGE );
        m_enemyStrenghDict.Add( EnemyType.Wind, MIN_DAMAGE );
        m_enemyStrenghDict.Add( EnemyType.Dirt, MIN_DAMAGE );
    }

    public void ReduceLife( int p_amount )
    {

    }
    #endregion
}

public partial class Player
{
    public const int PLAYER_COUNT = 2;
    public const int MAX_HEALTH = 10;
    public const int MIN_DAMAGE = 100;

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
