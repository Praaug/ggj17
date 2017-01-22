using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIDefending : MonoBehaviour
{
    #region Fields
    [SerializeField, Category( "References" )]
    private Player m_player = null;
    [SerializeField, Category( "References" )]
    private GameObject[] m_healthglobs = null;
    [SerializeField, Category( "References" )]
    private Text m_fireCount = null;
    [SerializeField, Category( "References" )]
    private Text m_waterCount = null;
    [SerializeField, Category( "References" )]
    private Text m_windCount = null;
    [SerializeField, Category( "References" )]
    private Text m_dirtCount = null;

    [SerializeField, Category( "Selection" )]
    private bool m_isPlayer1 = false;

    private int[] m_enemiesKilled = new int[ 4 ] { 0, 0, 0, 0 };
    #endregion

    #region Methods
    private void Start()
    {
        GameInfo.instance.OnStartGame += GameInfo_OnStartGame;
        gameObject.SetActive( false );
        GameInfo.instance.OnCurrentGamePhaseChange += Instance_OnCurrentGamePhaseChange;
    }

    private void Instance_OnCurrentGamePhaseChange()
    {
        gameObject.SetActive( GameInfo.instance.currentGamePhase == GameInfo.GamePhase.Fight );
    }

    private void GameInfo_OnStartGame()
    {
        Init( m_isPlayer1 ? Player.allPlayer[ 0 ] : Player.allPlayer[ 1 ] );
    }

    private void Init( Player p_player )
    {
        m_player = p_player;
        Reset();
        m_player.OnLifeChanged += OnHealthLost;
        m_player.OnEnemyKilled += OnEnemyKilled;
    }

    private void OnEnemyKilled( ElementType p_type )
    {
        m_enemiesKilled[ (int)p_type ]++;

        m_fireCount.text = m_enemiesKilled[ (int)ElementType.Fire ].ToString000();
        m_fireCount.text = m_enemiesKilled[ (int)ElementType.Water ].ToString000();
        m_fireCount.text = m_enemiesKilled[ (int)ElementType.Air ].ToString000();
        m_fireCount.text = m_enemiesKilled[ (int)ElementType.Dirt ].ToString000();
    }

    private void OnHealthLost()
    {
        for ( int i = 0; i < m_healthglobs.Length; i++ )
            m_healthglobs[ i ].SetActive( i <= m_player.lifes );
    }

    private void Reset()
    {
        for ( int i = 0; i < m_healthglobs.Length; i++ )
            m_healthglobs[ i ].SetActive( true );

        m_enemiesKilled = new int[ 4 ] { 0, 0, 0, 0 };
    }
    #endregion
}
