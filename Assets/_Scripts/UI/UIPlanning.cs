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
    [SerializeField, Category( "References" )]
    private GameObject[] m_objectsToActivate = null;
    [SerializeField, Category( "References" )]
    private Image m_ready = null;
    [SerializeField, Category( "Color" )]
    private Color m_NotReadyColor = Color.white;
    [SerializeField, Category( "Color" )]
    private Color m_readyColor = Color.white;

    private Player m_player = null;
    private UIWaveInfo m_currentUIWaveInfo = null;
    private int m_currentIndex = 0;
    #endregion

    #region Methods
    public void Awake()
    {
        m_currentUIWaveInfo = m_waveInfos[ 0 ];
        m_currentUIWaveInfo.IncrementSelector();
    }

    private void Start()
    {
        GameInfo.instance.OnStartGame += GameInfo_OnStartGame;
        //GameInfo.instance.OnEndGame += GameInfo_OnEndGame;
        GameInfo.instance.OnCurrentGamePhaseChange += GameInfo_OnCurrentGamePhaseChange;
        foreach ( var _go in m_objectsToActivate )
        {
            _go.SetActive( false );
        }
        gameObject.SetActive( false );
    }

    private void OnEnable()
    {
        m_ready.color = m_NotReadyColor;
    }

    private void GameInfo_OnCurrentGamePhaseChange()
    {
        gameObject.SetActive( GameInfo.instance.currentGamePhase == GameInfo.GamePhase.WaveBuilding );
        foreach ( var _go in m_objectsToActivate )
        {
            _go.SetActive( GameInfo.instance.currentGamePhase == GameInfo.GamePhase.WaveBuilding );
        }
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

        if ( InputUtility.GetFixedButtonDown( InputButton.MenuRight, m_player.inputSource ) )
            IncreaseSelector();
        if ( InputUtility.GetFixedButtonDown( InputButton.MenuLeft, m_player.inputSource ) )
            DecreaseSelector();
        if ( InputUtility.GetFixedButtonDown( InputButton.MenuUp, m_player.inputSource ) )
            IncreaseStat();
        if ( InputUtility.GetFixedButtonDown( InputButton.MenuDown, m_player.inputSource ) )
            DecreaseStat();
        if ( InputUtility.GetFixedButtonDown( InputButton.Attack, m_player.inputSource ) )
            SetReady();
        //if ( Input.GetKeyDown( KeyCode.D ) )
        //    IncreaseSelector();
        //if ( Input.GetKeyDown( KeyCode.A ) )
        //    DecreaseSelector();
    }

    private void IncreaseSelector()
    {
        if ( !m_currentUIWaveInfo.IncrementSelector() )
        {
            m_currentIndex = ( m_currentIndex + 1 ) % m_waveInfos.Length;
            m_currentUIWaveInfo = m_waveInfos[ m_currentIndex ];
            IncreaseSelector();
        }
    }

    private void DecreaseSelector()
    {
        if ( !m_currentUIWaveInfo.DecrementSelector() )
        {
            m_currentIndex = ( ( m_currentIndex % m_waveInfos.Length ) + m_waveInfos.Length - 1 ) % m_waveInfos.Length;
            m_currentUIWaveInfo = m_waveInfos[ m_currentIndex ];
            DecreaseSelector();
        }
    }

    private void IncreaseStat()
    {
        m_currentUIWaveInfo.IncrementStat();
    }
    private void DecreaseStat()
    {
        m_currentUIWaveInfo.DecrementStat();
    }
    private void SetReady()
    {
        if ( m_isPlayer1 )
        {
            GameInfo.instance.player1Ready = true;
            m_ready.color = m_readyColor;
        }
        else
        {
            GameInfo.instance.player2Ready = true;
            m_ready.color = m_readyColor;
        }
    }
    #endregion
}
