using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private int m_livesDownThreshhold = 4;
    [SerializeField]
    private AudioSource[] m_killSources = null;
    [SerializeField]
    private AudioSource m_lowHealthSource = null;
    [SerializeField]
    private AudioSource m_defendingSource = null;
    [SerializeField]
    private AudioSource m_planningSource = null;


    private int m_killIndex = 0;
    private Player m_player01;
    private Player m_player02;

    private void Awake()
    {
        Player.OnPlayerCreation += Player_OnPlayerCreation;
        GameInfo.instance.OnCurrentGamePhaseChange += Instance_OnCurrentGamePhaseChange;
    }

    private void Instance_OnCurrentGamePhaseChange()
    {
        if ( GameInfo.instance.currentGamePhase == GameInfo.GamePhase.WaveBuilding )
        {
            m_defendingSource.Stop();
            m_planningSource.Play();
        }
        if ( GameInfo.instance.currentGamePhase == GameInfo.GamePhase.Fight )
        {
            m_planningSource.Stop();
            m_defendingSource.Play();
        }

        m_lowHealthSource.Stop();
    }

    private void Player_OnPlayerCreation()
    {
        m_player01 = Player.allPlayer[ 0 ];
        m_player02 = Player.allPlayer[ 1 ];

        m_player01.OnEnemyKilled += OnEnemyKilled;
        m_player01.OnLifeChanged += OnLifeChanged;
    }

    private void OnLifeChanged()
    {
        if ( m_player01.lifes < m_livesDownThreshhold || m_player02.lifes < m_livesDownThreshhold )
            m_lowHealthSource.Play();
    }

    private void OnEnemyKilled( Enemy p_enemy )
    {
        m_killSources[ m_killIndex ].Play();
        m_killIndex = ( m_killIndex + 1 ) % m_killSources.Length;
    }
}
