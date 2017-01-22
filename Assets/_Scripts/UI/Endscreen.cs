using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Endscreen : MonoBehaviour
{
    [SerializeField, Category( "References" )]
    private Text m_winnerText = null;

    private void Start()
    {
        gameObject.SetActive( false );
        GameInfo.instance.OnCurrentGamePhaseChange += GameInfo_OnCurrentGamePhaseChange;
    }

    private void GameInfo_OnCurrentGamePhaseChange()
    {
        gameObject.SetActive( GameInfo.instance.currentGamePhase == GameInfo.GamePhase.PostGame );

        if ( GameInfo.instance.currentGamePhase == GameInfo.GamePhase.PostGame )
        {
            bool _player1Won = GameInfo.instance.winner == Player.allPlayer[ 0 ];
            m_winnerText.text = string.Format( "GAME OVER!\n Player {0} won the match!", _player1Won ? "1" : "2" );
        }

    }

    public void BackToMenu()
    {
        GameInfo.instance.InitPhase( GameInfo.GamePhase.PreGame );
    }
}
