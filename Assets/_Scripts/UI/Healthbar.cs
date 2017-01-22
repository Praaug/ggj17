using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    #region Fields
    [SerializeField, Category( "References" )]
    private Slider m_slider = null;
    [SerializeField, Category( "References" )]
    private Image m_fillImage = null;

    [SerializeField, Category( "Color" )]
    private Color m_fireColor;
    [SerializeField, Category( "Color" )]
    private Color m_waterColor;
    [SerializeField, Category( "Color" )]
    private Color m_airColor;
    [SerializeField, Category( "Color" )]
    private Color m_dirtColor;
    private Enemy m_enemy;
    #endregion

    #region Methods
    public void SetSlider( float p_value )
    {
        m_slider.value = Mathf.Clamp01( p_value );
    }

    public void Init( Enemy p_enemy )
    {
        m_enemy = p_enemy;

        switch ( p_enemy.info.elementType )
        {
            case ElementType.Fire:
                m_fillImage.color = m_fireColor;
                break;
            case ElementType.Water:
                m_fillImage.color = m_waterColor;
                break;
            case ElementType.Air:
                m_fillImage.color = m_airColor;
                break;
            case ElementType.Dirt:
                m_fillImage.color = m_dirtColor;
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if ( m_enemy == null )
            return;

        transform.position = Camera.main.WorldToScreenPoint( m_enemy.controller.healthbarTransform.position );
    }
    #endregion
}
