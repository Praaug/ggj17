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
    private Text m_fireIncome = null;
    [SerializeField, Category( "References" )]
    private Text m_waterIncome = null;
    [SerializeField, Category( "References" )]
    private Text m_windIncome = null;
    [SerializeField, Category( "References" )]
    private Text m_dirtIncome = null;
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
        m_fireIncome.text = ( (int)( m_player.elementBuffDict[ ElementType.Fire ] ) ).ToString000();
        m_waterIncome.text = ( (int)( m_player.elementBuffDict[ ElementType.Water ] ) ).ToString000();
        m_windIncome.text = ( (int)( m_player.elementBuffDict[ ElementType.Air ] ) ).ToString000();
        m_dirtIncome.text = ( (int)( m_player.elementBuffDict[ ElementType.Dirt ] ) ).ToString000();
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
