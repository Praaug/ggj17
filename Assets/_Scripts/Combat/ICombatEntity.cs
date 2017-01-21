using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatEntity
{
    GameObject gameObject { get; }

    void InflictDamage( float p_damage );
}
