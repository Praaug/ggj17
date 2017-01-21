using UnityEngine;
using System.Collections;

public class EnemyInfo : MonoBehaviour
{
    #region Properties
    public float maxHealth { get { return m_health; } }
    #endregion

    #region Fields
    [SerializeField, Category( "Stats" )]
    private float m_health = 0.0f;
    #endregion

    #region Methods

    #endregion
}
