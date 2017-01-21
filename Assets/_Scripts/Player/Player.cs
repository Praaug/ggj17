using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class Player
{
    #region Events
    /// <summary>
    /// Event that is fired when the player life amount has changed
    /// </summary>
    public event System.Action OnLifeChanged;

    /// <summary>
    /// Event that is fired when the amount of points of a certain element type has changed
    /// </summary>
    public event System.Action<ElementType> OnElementPointsChange;

    /// <summary>
    /// Event that if fired when the player has lost all lifes
    /// </summary>
    public event System.Action OnKill;
    #endregion

    #region Properties
    public PlayerConfig config { get; private set; }
    public GameObject gameObject { get; private set; }
    public InputSource inputSource { get; private set; }

    public int lifes { get; private set; }
    public WaveInfo waveInfo { get; private set; }


    /// <summary>
    /// Dictionary that holds the income of the player per element
    /// </summary>
    public Dictionary<ElementType, int> elementPointSpendDict { get; private set; }
    /// <summary>
    /// Dictionary that holds the points per element for the player
    /// </summary>
    public Dictionary<ElementType, int> elementPointsDict { get; private set; }
    /// <summary>
    /// Dictionary that contains the amount of bonus damage per element for the player
    /// </summary>
    public Dictionary<ElementType, float> elementBuffDict { get; private set; }

    public Dictionary<ElementType, int> elementKillDictTemp { get; private set; }
    #endregion

    #region Fields
    private Dictionary<ElementType, int> m_enemyStrenghDict;
    #endregion

    #region Constructor
    public Player( InputSource p_inputSource )
    {
        inputSource = p_inputSource;
        lifes = 10;

        elementPointSpendDict = new Dictionary<ElementType, int>();
        elementPointSpendDict.Add( ElementType.Fire, 0 );
        elementPointSpendDict.Add( ElementType.Water, 0 );
        elementPointSpendDict.Add( ElementType.Air, 0 );
        elementPointSpendDict.Add( ElementType.Dirt, 0 );

        elementPointsDict = new Dictionary<ElementType, int>();
        elementPointsDict.Add( ElementType.Fire, 0 );
        elementPointsDict.Add( ElementType.Water, 0 );
        elementPointsDict.Add( ElementType.Air, 0 );
        elementPointsDict.Add( ElementType.Dirt, 0 );

        elementKillDictTemp = new Dictionary<ElementType, int>();
        elementKillDictTemp.Add( ElementType.Fire, 0 );
        elementKillDictTemp.Add( ElementType.Water, 0 );
        elementKillDictTemp.Add( ElementType.Air, 0 );
        elementKillDictTemp.Add( ElementType.Dirt, 0 );

        elementBuffDict = new Dictionary<ElementType, float>();
        elementBuffDict.Add( ElementType.Fire, 0 );
        elementBuffDict.Add( ElementType.Water, 0 );
        elementBuffDict.Add( ElementType.Air, 0 );
        elementBuffDict.Add( ElementType.Dirt, 0 );

        m_enemyStrenghDict = new Dictionary<ElementType, int>();
        m_enemyStrenghDict.Add( ElementType.Fire, MIN_DAMAGE );
        m_enemyStrenghDict.Add( ElementType.Water, MIN_DAMAGE );
        m_enemyStrenghDict.Add( ElementType.Air, MIN_DAMAGE );
        m_enemyStrenghDict.Add( ElementType.Dirt, MIN_DAMAGE );

        waveInfo = new WaveInfo( this );
        waveInfo.fireCount++;
    }
    #endregion

    #region Methods
    public void AssignAvatar( GameObject p_gameObject )
    {
        gameObject = p_gameObject;
        config = p_gameObject.GetComponent<PlayerConfig>();
    }

    public void DestroyAvatar()
    {
        Object.Destroy( gameObject );
    }

    public void ReduceLife( int p_amount )
    {
        // Reduce life
        lifes = Mathf.Max( 0, lifes - p_amount );

        if ( OnLifeChanged != null )
            OnLifeChanged();

        if ( lifes == 0 )
            Kill();
    }

    /// <summary>
    /// Registeres a kill of the enemy from this player
    /// </summary>
    /// <param name="p_enemy">The enemy that the player has killed</param>
    public void RegisterEnemyKill( Enemy p_enemy )
    {
        ElementType _elementType = p_enemy.info.elementType;

        // Add element points
        AddElementPoints( _elementType, p_enemy.info.elementPointValue );

        // Increase tmp counter
        elementKillDictTemp[ _elementType ]++;

        if ( elementKillDictTemp[ _elementType ] >= GameInfo.instance.killsPerStep )
        {
            // Decrease tmp count
            elementKillDictTemp[ _elementType ] -= GameInfo.instance.killsPerStep;

            // Increase bonus damage for this elemental
            elementBuffDict[ _elementType ] += GameInfo.instance.damageIncPerStep;
        }

        // Increase counter for kill of this enemy type
        elementBuffDict[ p_enemy.info.elementType ]++;
    }

    public void AddElementPoints( ElementType p_type, int p_amount )
    {
        // Increase points
        elementPointsDict[ p_type ] += p_amount;

        // Call event
        if ( OnElementPointsChange != null )
            OnElementPointsChange( p_type );
    }

    public void RemoveElementPoints( ElementType p_type, int p_amount )
    {
        // Increase points
        elementPointsDict[ p_type ] = Mathf.Max( 0, elementPointsDict[ p_type ] - p_amount );

        // Call event
        if ( OnElementPointsChange != null )
            OnElementPointsChange( p_type );
    }

    public void RegisterSpendElementPoints( ElementType p_type, int p_amount )
    {
        elementPointSpendDict[ p_type ] += p_amount;
    }

    public void Kill()
    {
        if ( OnKill != null )
            OnKill();

        Dbg.LogError( "Player with input source {0} DIED!", inputSource );
    }
    #endregion
}

public partial class Player
{
    public const int PLAYER_COUNT = 2;
    public const int MAX_HEALTH = 10;
    public const int MIN_DAMAGE = 100;

    public static List<Player> allPlayer { get; private set; }

    public static List<Player> CreatePlayer()
    {
        allPlayer = new List<Player>() { new Player( InputSource.Player1 ), new Player( InputSource.Player2 ) };

        return allPlayer;
    }

    public static void CreateAvatars( GameObject p_player1Prefab, GameObject p_player2Prefab )
    {
        // Create player avatar
        GameObject _player1Instance = Object.Instantiate( p_player1Prefab );
        allPlayer[ 0 ].AssignAvatar( _player1Instance );

        // Create player avatar
        GameObject _player2Instance = Object.Instantiate( p_player2Prefab );
        allPlayer[ 1 ].AssignAvatar( _player2Instance );
    }

    public static void DestroyAvatars()
    {
        foreach ( Player _player in allPlayer )
            _player.DestroyAvatar();
    }

    public static Player GetPlayer( GameObject p_gameObject )
    {
        return allPlayer.FirstOrDefault( p => p.gameObject == p_gameObject );
    }

    public static Player OtherPlayer( Player p_player )
    {
        return allPlayer[ 0 ] == p_player ? allPlayer[ 1 ] : allPlayer[ 0 ];
    }
}
