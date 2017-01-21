using UnityEngine;
using System.Collections;

public class PlayerConfig : MonoBehaviour
{
    #region Properties
    public float spawnRate { get { return m_spawnRate; } }
    public float randomBias { get { return m_randomBias; } }
    #endregion

    #region Methods
    [SerializeField, Category( "Stats" )]
    private float m_spawnRate = 0;
    [SerializeField, Category( "Stats" )]
    private float m_randomBias = 0.0f;

    private Player m_player;

    private void Start()
    {
        m_player = Player.GetPlayer( gameObject );
    }
    #endregion
}
