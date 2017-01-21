using UnityEngine;
using System.Collections;

public class UIMenu : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private GameObject m_OptionsPopup = null;
    #endregion

    #region Methods
    private void Awake()
    {
        m_OptionsPopup.SetActive( false );
    }

    public void OnStartClick()
    {
        gameObject.SetActive( false );
        GameInfo.Instance.StartGame();
    }

    public void OnOptionsClick()
    {
        m_OptionsPopup.SetActive( true );
    }

    public void OnOptionsOKClick()
    {
        m_OptionsPopup.SetActive( false );
    }
    #endregion
}
