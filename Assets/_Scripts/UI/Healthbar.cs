using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Healthbar : MonoBehaviour
{
    #region Fields
    [SerializeField, Category( "References" )]
    private Slider m_slider = null;
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
    }

    private void Update()
    {
        if ( m_enemy == null )
            return;

        transform.position = Camera.main.WorldToScreenPoint( m_enemy.controller.healthbarTransform.position );
    }
    #endregion
}
