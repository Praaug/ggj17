using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

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

    private Dictionary<ElementType, int> m_elePointsGainedPerRound = new Dictionary<ElementType, int>();

    [SerializeField]
    private GameObject[] m_objectsToActivate = null;
    #endregion

    #region Methods
    private void Awake()
    {
        foreach ( ElementType _type in EnumUtility.GetValues<ElementType>() )
            m_elePointsGainedPerRound.Add( _type, 0 );
    }

    private void Start()
    {
        foreach ( var _go in m_objectsToActivate )
        {
            _go.SetActive( false );
        }
        Player.OnPlayerCreation += Player_OnPlayerCreation;
        gameObject.SetActive( false );
        GameInfo.instance.OnCurrentGamePhaseChange += Instance_OnCurrentGamePhaseChange;
    }

    private void Instance_OnCurrentGamePhaseChange()
    {
        gameObject.SetActive( GameInfo.instance.currentGamePhase == GameInfo.GamePhase.Fight );
        foreach ( var _go in m_objectsToActivate )
            _go.SetActive( GameInfo.instance.currentGamePhase == GameInfo.GamePhase.Fight );
    }

    private void Player_OnPlayerCreation()
    {
        Init( m_isPlayer1 ? Player.allPlayer[ 0 ] : Player.allPlayer[ 1 ] );
    }

    private void Init( Player p_player )
    {
        m_player = p_player;
        Reset();
        m_player.OnLifeChanged += OnHealthLost;
        m_player.OnEnemyKilled += OnEnemyKilled;

        GameInfo.instance.OnCurrentGamePhaseChange += GameInfo_OnCurrentGamePhaseChange1;
    }

    private void GameInfo_OnCurrentGamePhaseChange1()
    {
        if ( GameInfo.instance.currentGamePhase == GameInfo.GamePhase.Fight )
            Reset();
    }

    private void OnEnemyKilled( Enemy p_enemy )
    {
        m_elePointsGainedPerRound[ p_enemy.info.elementType ] += p_enemy.info.elementPointValue;

        UpdateText();
    }

    private void UpdateText()
    {
        m_fireCount.text = m_elePointsGainedPerRound[ ElementType.Fire ].ToString000();
        m_waterCount.text = m_elePointsGainedPerRound[ ElementType.Water ].ToString000();
        m_windCount.text = m_elePointsGainedPerRound[ ElementType.Air ].ToString000();
        m_dirtCount.text = m_elePointsGainedPerRound[ ElementType.Dirt ].ToString000();
    }

    private void OnHealthLost()
    {
        UpdateHealthGlobs();
    }

    private void UpdateHealthGlobs()
    {
        for ( int i = 0; i < m_healthglobs.Length; i++ )
            m_healthglobs[ i ].SetActive( i <= m_player.lifes );
    }

    private void Reset()
    {
        UpdateHealthGlobs();

        foreach ( ElementType _type in EnumUtility.GetValues<ElementType>() )
            m_elePointsGainedPerRound[ _type ] = 0;

        UpdateText();
    }
    #endregion
}
