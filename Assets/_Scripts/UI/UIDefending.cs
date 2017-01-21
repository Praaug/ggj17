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
    #endregion

    #region Methods
    private void InitUI( Player p_player )
    {
        m_player = p_player;
    }

    private void InitWave()
    {
        Reset();
        OnIncomeChange();
    }

    private void OnIncomeChange()
    {
        m_fireCount.text = ( (int)( m_player.elementBuffDict[ ElementType.Fire ] ) ).ToString000();
        m_waterCount.text = ( (int)( m_player.elementBuffDict[ ElementType.Water ] ) ).ToString000();
        m_windCount.text = ( (int)( m_player.elementBuffDict[ ElementType.Air ] ) ).ToString000();
        m_dirtCount.text = ( (int)( m_player.elementBuffDict[ ElementType.Dirt ] ) ).ToString000();
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

        for ( int i = 0; i < m_healthglobs.Length; i++ )
            m_healthglobs[ i ].SetActive( true );
    }
    #endregion
}
