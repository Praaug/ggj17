using UnityEngine;
using System.Collections;

public class UIMenu : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private GameObject m_optionsPopup = null;
    [SerializeField]
    private GameObject m_creditsPopup = null;
    #endregion

    #region Methods
    private void Awake()
    {
        m_optionsPopup.SetActive( false );
        m_creditsPopup.SetActive( false );
    }

    public void OnStartClick()
    {
        Dbg.Log( "On Start Clicked" );
        gameObject.SetActive( false );
        GameInfo.instance.StartGame();
    }

    public void OnOptionsClick()
    {
        m_optionsPopup.SetActive( true );
    }

    public void OnOptionsOKClick()
    {
        m_optionsPopup.SetActive( false );
    }

    public void OnCreditsClick()
    {
        m_creditsPopup.SetActive( true );
    }

    public void OnCredtirsOKClick()
    {
        m_creditsPopup.SetActive( false );
    }
    #endregion
}
