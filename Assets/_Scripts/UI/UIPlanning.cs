using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UIPlanning : MonoBehaviour
{
    #region Fields
    [SerializeField, Category( "Points" )]
    private Text m_firePoints = null;
    [SerializeField, Category( "Points" )]
    private Text m_waterPoints = null;
    [SerializeField, Category( "Points" )]
    private Text m_windPoints = null;
    [SerializeField, Category( "Points" )]
    private Text m_dirtPoints = null;
    [SerializeField, Category( "Waves" )]
    private UILineRenderer m_fireWave = null;
    [SerializeField, Category( "Waves" )]
    private UILineRenderer m_waterWave = null;
    [SerializeField, Category( "Waves" )]
    private UILineRenderer m_windWave = null;
    [SerializeField, Category( "Waves" )]
    private UILineRenderer m_dirtWave = null;
    [SerializeField, Category( "other Player" )]
    private Text m_otherPlayerFireStrengh = null;
    [SerializeField, Category( "other Player" )]
    private Text m_otherPlayerWaterStrengh = null;
    [SerializeField, Category( "other Player" )]
    private Text m_otherPlayerWindStrengh = null;
    [SerializeField, Category( "other Player" )]
    private Text m_otherPlayerDirtStrengh = null;
    [SerializeField, Category( "Selection" )]
    private GameObject[] m_selectors = null;

    private Player m_player = null;
    #endregion

    #region Methods
    public void Init( Player p_player )
    {
        m_player = p_player;
    }

    private void IncreaseStat( EnemyType p_type )
    {

    }

    private void DecreaseStat( EnemyType p_type )
    {

    }
    #endregion
}
