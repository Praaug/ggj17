using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class Enemy : ICombatEntity
{
    #region Events
    /// <summary>
    /// Event that is fired when the enemy is killed
    /// </summary>
    public event System.Action OnKill;
    /// <summary>
    /// Event that is fired when the enemy is killed, with detailed Infos
    /// </summary>
    public static event System.Action<int, EnemyType, int> OnKillDetailed;
    #endregion

    #region Properties
    public GameObject gameObject { get; private set; }

    public float health { get; private set; }

    public bool isDead { get { return health <= 0; } }

    public EnemyController controller { get; private set; }
    public EnemyInfo info { get; private set; }
    #endregion

    #region Constructor
    public Enemy( GameObject p_enemyAvatar )
    {
        gameObject = p_enemyAvatar;

        controller = gameObject.GetComponent<EnemyController>();
        info = gameObject.GetComponent<EnemyInfo>();

        health = info.maxHealth;
    }
    #endregion

    public void Kill()
    {
        // Adjust health
        health = 0.0f;

        // Fire kill event
        if ( OnKill != null )
            OnKill();

        // Remove from enemy list
        allEnemies.Remove( this );

        // Destroy the gameObject
        Object.Destroy( gameObject, 5.0f );
    }

    public void InflictDamage( float p_amount )
    {
        health = Mathf.Max( 0, health - p_amount );

        if ( health <= 0 )
            Kill();
    }
}

public partial class Enemy
{
    static Enemy()
    {
        allEnemies = new List<Enemy>();
    }

    public static List<Enemy> allEnemies { get; private set; }

    public static Enemy CreateEnemy( GameObject p_prefab, Vector3 p_position, Quaternion p_rotation )
    {
        // Instantiate avatar
        GameObject _enemyAvatarInstance = Object.Instantiate( p_prefab, p_position, p_rotation );

        // Create enemy object
        Enemy _enemy = new Enemy( _enemyAvatarInstance );

        // Add enemy to list
        allEnemies.Add( _enemy );

        // Return enemy instance
        return _enemy;
    }

    public static bool IsEnemy( Component p_component, out Enemy p_enemy )
    {
        p_enemy = allEnemies.FirstOrDefault( enemy => enemy.gameObject == p_component.gameObject );
        return p_enemy != null;
    }

    public static Enemy GetEnemy( GameObject gameObject )
    {
        return allEnemies.FirstOrDefault( e => e.gameObject == gameObject );
    }
}

public enum EnemyType
{
    Fire,
    Water,
    Wind,
    Dirt
}