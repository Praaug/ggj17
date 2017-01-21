using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameInfo : MonoBehaviour
{
    #region Events
    /// <summary>
    /// Event that is fired when the game phase has changed
    /// </summary>
    public event System.Action OnCurrentGamePhaseChange;
    #endregion

    #region Properties
    [Debug]
    public GamePhase currentGamePhase
    {
        get { return m_currentGamePhase; }
        private set
        {
            GamePhase _oldPhase = m_currentGamePhase;
            m_currentGamePhase = value;

            Internal_CurrentGamePhaseChange();

            if ( value != _oldPhase && OnCurrentGamePhaseChange != null )
                OnCurrentGamePhaseChange();
        }
    }
    private GamePhase m_currentGamePhase;
    #endregion

    #region Fields
    [SerializeField, Category( "References" )]
    private EnemySpawnPlane[] m_spawnPlanes = new EnemySpawnPlane[ 0 ];
    [SerializeField, Category( "References" )]
    private EnemyKillPlane[] m_killPlanes = new EnemyKillPlane[ 0 ];
    [SerializeField, Category( "References" )]
    private GameObject m_playerPrefab = null;
    #endregion

    #region Methods
    private void Awake()
    {
        if ( s_instance != null && s_instance != this )
        {
            Destroy( this );
            return;
        }

        s_instance = this;

        currentGamePhase = GamePhase.PreGame;
    }

    private void OnDestroy()
    {
        if ( s_instance == this )
            s_instance = null;
    }

    private void Internal_CurrentGamePhaseChange()
    {
        switch ( currentGamePhase )
        {
            case GamePhase.PreGame:
                break;
            case GamePhase.WaveBuilding:
                break;
            case GamePhase.Fight:
                InitPhase_Fight();
                break;
            default:
                break;
        }
    }

    private void OnGUI()
    {

    }

    /// <summary>
    /// Starts the game
    /// </summary>
    [Button( "Start Game" )]
    public void StartGame()
    {
        // Create the player avatars and objects
        Player.CreatePlayer( m_playerPrefab );

        if ( Player.allPlayer.Count != m_spawnPlanes.Length || Player.allPlayer.Count != m_killPlanes.Length )
        {
            Dbg.LogError( "The amount of player does not match the amount of spawn or kill planes!" );
            return;
        }

        // Assign player to spawn planes
        for ( int i = 0; i < m_spawnPlanes.Length; i++ )
        {
            Player _owningPlayer = Player.allPlayer[ i ];
            Player _damagedPlayer = Player.allPlayer[ ( i + 1 ) % Player.allPlayer.Count ];

            m_spawnPlanes[ i ].AssignPlayer( _owningPlayer, _damagedPlayer );
            m_killPlanes[ i ].AssignPlayer( _owningPlayer, _damagedPlayer );
        }

        // Init the game phase
        currentGamePhase = GamePhase.Fight;
    }

    private void InitPhase_Fight()
    {
        for ( int i = 0; i < m_spawnPlanes.Length; i++ )
            m_spawnPlanes[ i ].SpawnMobs();
    }
    #endregion
}

public partial class GameInfo : MonoBehaviour
{
    private static GameInfo s_instance;

    public enum GamePhase
    {
        PreGame,
        WaveBuilding,
        Fight
    }
}
