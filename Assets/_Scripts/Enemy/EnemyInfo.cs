using UnityEngine;
using System.Collections;

public class EnemyInfo : MonoBehaviour
{
    #region Properties
    public float maxHealth { get { return m_health; } }
    public ElementType elementType { get { return m_type; } }
    public int elementPointValue { get { return m_elementPointValue; } }
    #endregion

    #region Fields
    [SerializeField, Category( "Stats" )]
    private float m_health = 0.0f;
    [SerializeField, Category( "Stats" )]
    private ElementType m_type = ElementType.Dirt;
    [SerializeField, Category( "Stats" )]
    private int m_elementPointValue = 0;
    #endregion

    #region Methods

    #endregion
}
