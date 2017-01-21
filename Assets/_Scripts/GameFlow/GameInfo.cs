﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameInfo : MonoBehaviour
{
    #region Events
    /// <summary>
    /// Event that is fired when the game phase has changed
    /// </summary>
    public event System.Action OnCurrentGamePhaseChange;

    /// <summary>
    /// Event that is fired when the game has started
    /// </summary>
    public event System.Action OnStartGame;
    #endregion

    #region Properties
    public Dictionary<ElementType, EnemyInfo[]> elementPrefabDict { get; private set; }
    public float damageIncPerStep { get { return m_damageIncPerStep; } }
    public int killsPerStep { get { return m_killsPerStep; } }
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
    #endregion

    #region Fields
    [SerializeField, Category( "References" )]
    private EnemySpawnPlane[] m_spawnPlanes = new EnemySpawnPlane[ 0 ];
    [SerializeField, Category( "References" )]
    private EnemyKillPlane[] m_killPlanes = new EnemyKillPlane[ 0 ];
    [SerializeField, Category( "References" )]
    private GameObject m_player1Prefab = null;
    [SerializeField, Category( "References" )]
    private GameObject m_player2Prefab = null;

    [SerializeField, Category( "Enemy" )]
    private EnemyInfo[] m_enemyPrefabFire = new EnemyInfo[ 0 ];
    [SerializeField, Category( "Enemy" )]
    private EnemyInfo[] m_enemyPrefabAir = new EnemyInfo[ 0 ];
    [SerializeField, Category( "Enemy" )]
    private EnemyInfo[] m_enemyPrefabWater = new EnemyInfo[ 0 ];
    [SerializeField, Category( "Enemy" )]
    private EnemyInfo[] m_enemyPrefabDirt = new EnemyInfo[ 0 ];

    [SerializeField, Category( "Stats" )]
    private float m_damageIncPerStep = 0.0f;
    [SerializeField, Category( "Stats" )]
    private int m_killsPerStep = 0;
    [SerializeField, Category( "Stats" )]
    private float m_waveBuildTimer = 0.0f;

    private bool m_player1WaveFinished = false;
    private bool m_player2WaveFinished = false;

    private bool m_player1Ready;
    private bool m_player2Ready;

    private GamePhase m_currentGamePhase;

    private Coroutine m_coroutineTimerWaveBuilding;
    #endregion

    #region Methods
    private void Awake()
    {
        if ( s_instance != null && s_instance != this )
        {
            Destroy( this );
            return;
        }

        for ( int i = 0; i < m_spawnPlanes.Length; i++ )
            m_spawnPlanes[ i ].OnWaveDoneThis += SpawnPlane_OnWaveDone;

        s_instance = this;

        currentGamePhase = GamePhase.PreGame;



        elementPrefabDict = new Dictionary<ElementType, EnemyInfo[]>();
        elementPrefabDict.Add( ElementType.Dirt, m_enemyPrefabDirt );
        elementPrefabDict.Add( ElementType.Fire, m_enemyPrefabFire );
        elementPrefabDict.Add( ElementType.Water, m_enemyPrefabWater );
        elementPrefabDict.Add( ElementType.Air, m_enemyPrefabAir );
    }

    private void SpawnPlane_OnWaveDone( EnemySpawnPlane p_spawnPlane )
    {
        if ( p_spawnPlane == m_spawnPlanes[ 0 ] )
            m_player1WaveFinished = true;
        if ( p_spawnPlane == m_spawnPlanes[ 1 ] )
            m_player2WaveFinished = true;

        if ( m_player1WaveFinished && m_player2WaveFinished )
            currentGamePhase = GamePhase.WaveBuilding;
    }

    private void OnDestroy()
    {
        if ( s_instance == this )
            s_instance = null;
    }

    private void Internal_CurrentGamePhaseChange()
    {
        Dbg.Log( "GAME PHASE CHANGED TO {0}", currentGamePhase );
        switch ( currentGamePhase )
        {
            case GamePhase.PreGame:
                break;
            case GamePhase.WaveBuilding:
                InitPhase_WaveBuilding();
                break;
            case GamePhase.Fight:
                InitPhase_Fight();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    [Button( "Start Game" )]
    public void StartGame()
    {
        // Create the player avatars and objects
        Player.CreatePlayer();

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
        InitPhase( GamePhase.Fight );
    }

    public void InitPhase( GamePhase p_phase )
    {
        currentGamePhase = p_phase;
    }

    private void InitPhase_Fight()
    {
        if ( m_coroutineTimerWaveBuilding != null )
        {
            StopCoroutine( m_coroutineTimerWaveBuilding );
            m_coroutineTimerWaveBuilding = null;
        }

        m_player1Ready = false;
        m_player2Ready = false;

        Player.CreateAvatars( m_player1Prefab, m_player2Prefab );

        m_player1WaveFinished = false;
        m_player2WaveFinished = false;

        for ( int i = 0; i < m_spawnPlanes.Length; i++ )
            m_spawnPlanes[ i ].SpawnMobs();
    }

    private void InitPhase_WaveBuilding()
    {
        if ( m_coroutineTimerWaveBuilding != null )
            StopCoroutine( m_coroutineTimerWaveBuilding );
        m_coroutineTimerWaveBuilding = StartCoroutine( Coroutine_TimerWaveBuilding() );

        m_player1Ready = false;
        m_player2Ready = false;

        Player.DestroyAvatars();

        // Add income
        foreach ( Player _player in Player.allPlayer )
        {
            _player.AddElementPoints( ElementType.Air, CalculcateIncome( _player.elementPointSpendDict[ ElementType.Air ] ) );
            _player.AddElementPoints( ElementType.Dirt, CalculcateIncome( _player.elementPointSpendDict[ ElementType.Dirt ] ) );
            _player.AddElementPoints( ElementType.Fire, CalculcateIncome( _player.elementPointSpendDict[ ElementType.Fire ] ) );
            _player.AddElementPoints( ElementType.Water, CalculcateIncome( _player.elementPointSpendDict[ ElementType.Water ] ) );
        }
    }

    private int CalculcateIncome( int p_pointsSpend )
    {
        return Mathf.FloorToInt( Mathf.Sqrt( p_pointsSpend * 3.0f ) );
    }

    private void OnGUI()
    {
        if ( GUILayout.Button( "Start Game" ) )
            StartGame();

        if ( Player.allPlayer != null && Player.allPlayer.Count > 0 )
        {
            GUILayout.Label( "Player1" );
            Player _player1 = Player.allPlayer[ 0 ];
            GUILayout.Label( string.Format( "Fire: {0}, spend: {1}, wave: {2}", _player1.elementPointsDict[ ElementType.Fire ], _player1.elementPointSpendDict[ ElementType.Fire ], _player1.waveInfo.fireCount ) );
            GUILayout.Label( string.Format( "Water: {0}, spend: {1}, wave: {2}", _player1.elementPointsDict[ ElementType.Water ], _player1.elementPointSpendDict[ ElementType.Water ], _player1.waveInfo.waterCount ) );
            GUILayout.Label( string.Format( "Air: {0}, spend: {1}, wave: {2}", _player1.elementPointsDict[ ElementType.Air ], _player1.elementPointSpendDict[ ElementType.Air ], _player1.waveInfo.windCount ) );
            GUILayout.Label( string.Format( "Dirt: {0}, spend: {1}, wave: {2}", _player1.elementPointsDict[ ElementType.Dirt ], _player1.elementPointSpendDict[ ElementType.Dirt ], _player1.waveInfo.dirtCount ) );

            GUILayout.Label( "" );
            GUILayout.Label( "Player2" );
            Player _player2 = Player.allPlayer[ 1 ];
            GUILayout.Label( string.Format( "Fire: {0}, spend: {1}, wave: {2}", _player2.elementPointsDict[ ElementType.Fire ], _player2.elementPointSpendDict[ ElementType.Fire ], _player1.waveInfo.fireCount ) );
            GUILayout.Label( string.Format( "Water: {0}, spend: {1}, wave: {2}", _player2.elementPointsDict[ ElementType.Water ], _player2.elementPointSpendDict[ ElementType.Water ], _player1.waveInfo.waterCount ) );
            GUILayout.Label( string.Format( "Air: {0}, spend: {1}, wave: {2}", _player2.elementPointsDict[ ElementType.Air ], _player2.elementPointSpendDict[ ElementType.Air ], _player1.waveInfo.windCount ) );
            GUILayout.Label( string.Format( "Dirt: {0}, spend: {1}, wave: {2}", _player2.elementPointsDict[ ElementType.Dirt ], _player2.elementPointSpendDict[ ElementType.Dirt ], _player1.waveInfo.dirtCount ) );
        }


        if ( currentGamePhase == GamePhase.WaveBuilding )
        {
            if ( !m_player1Ready && GUILayout.Button( "Player 1 ready" ) )
            {
                m_player1Ready = true;
            }

            if ( !m_player2Ready && GUILayout.Button( "Player 2 ready" ) )
            {
                m_player2Ready = true;
            }

            if ( m_player1Ready && m_player2Ready )
                InitPhase( GamePhase.Fight );
        }
    }

    private IEnumerator Coroutine_TimerWaveBuilding()
    {
        yield return new WaitForSeconds( 20.0f );

        InitPhase( GamePhase.Fight );
    }
    #endregion
}

public partial class GameInfo : MonoBehaviour
{
    public static GameInfo instance
    {
        get
        {
            if ( s_instance == null )
            {
                s_instance = new GameObject( "GameInfo" ).AddComponent<GameInfo>();
                DontDestroyOnLoad( s_instance );
            }

            return s_instance;
        }
    }
    private static GameInfo s_instance;

    public enum GamePhase
    {
        PreGame,
        WaveBuilding,
        Fight
    }
}
