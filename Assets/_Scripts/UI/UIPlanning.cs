using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UIPlanning : MonoBehaviour
{
    #region Fields
    [SerializeField, Category( "Selection" )]
    private UIWaveInfo[] m_waveInfos = null;
    [SerializeField, Category( "Selection" )]
    private bool m_isPlayer1 = false;

    private Player m_player = null;
    private UIWaveInfo m_currentWaveInfo = null;
    private int m_currentIndex = 0;
    #endregion

    #region Methods
    public void Awake()
    {
        m_currentWaveInfo = m_waveInfos[ 0 ];
        m_currentWaveInfo.IncrementSelector();
    }

    private void Start()
    {
        GameInfo.instance.OnStartGame += GameInfo_OnStartGame;
        GameInfo.instance.OnCurrentGamePhaseChange += GameInfo_OnCurrentGamePhaseChange;
        gameObject.SetActive( false );
    }

    private void GameInfo_OnCurrentGamePhaseChange()
    {
        gameObject.SetActive( GameInfo.instance.currentGamePhase == GameInfo.GamePhase.WaveBuilding );
    }

    private void GameInfo_OnStartGame()
    {
        Init( m_isPlayer1 ? Player.allPlayer[ 0 ] : Player.allPlayer[ 1 ] );
    }

    public void Init( Player p_player )
    {
        m_player = p_player;
        foreach ( UIWaveInfo _wi in m_waveInfos )
            _wi.Init( m_player );
    }

    public void FixedUpdate()
    {
        if ( GameInfo.instance.currentGamePhase != GameInfo.GamePhase.WaveBuilding )
            return;

        if ( InputUtility.GetButtonDown( InputButton.MenuRight, m_player.inputSource ) )
            IncreaseSelector();
        if ( InputUtility.GetButtonDown( InputButton.MenuLeft, m_player.inputSource ) )
            DecreaseSelector();
        if ( InputUtility.GetButtonDown( InputButton.MenuUp, m_player.inputSource ) )
            IncreaseStat();
        if ( InputUtility.GetButtonDown( InputButton.MenuDown, m_player.inputSource ) )
            DecreaseStat();
        //if ( Input.GetKeyDown( KeyCode.D ) )
        //    IncreaseSelector();
        //if ( Input.GetKeyDown( KeyCode.A ) )
        //    DecreaseSelector();
    }

    private void IncreaseSelector()
    {
        if ( !m_currentWaveInfo.IncrementSelector() )
        {
            m_currentIndex = ( m_currentIndex + 1 ) % m_waveInfos.Length;
            m_currentWaveInfo = m_waveInfos[ m_currentIndex ];
            IncreaseSelector();
        }
    }

    private void DecreaseSelector()
    {
        if ( !m_currentWaveInfo.DecrementSelector() )
        {
            m_currentIndex = ( ( m_currentIndex % m_waveInfos.Length ) + m_waveInfos.Length - 1 ) % m_waveInfos.Length;
            m_currentWaveInfo = m_waveInfos[ m_currentIndex ];
            DecreaseSelector();
        }
    }

    private void IncreaseStat()
    {
        m_currentWaveInfo.IncrementStat();
    }
    private void DecreaseStat()
    {
        m_currentWaveInfo.DecrementStat();
    }
    #endregion
}
