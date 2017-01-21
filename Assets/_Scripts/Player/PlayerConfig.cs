using UnityEngine;
using System.Collections;

public class PlayerConfig : MonoBehaviour
{
    #region Properties
    public WaveInfo waveInfo { get { return m_waveInfo; } }
    public float spawnRate { get { return m_spawnRate; } }
    public float randomBias { get { return m_randomBias; } }
    #endregion

    #region Methods
    [SerializeField, Category( "Wave" )]
    private WaveInfo m_waveInfo;

    [SerializeField, Category( "Stats" )]
    private float m_spawnRate = 0;
    [SerializeField, Category( "Stats" )]
    private float m_randomBias = 0.0f;

    private Player m_player;

    private void Start()
    {
        m_player = Player.GetPlayer( gameObject );

        m_waveInfo = new WaveInfo( m_player, m_waveInfo );
    }
    #endregion
}
