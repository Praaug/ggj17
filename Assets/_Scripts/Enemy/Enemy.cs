using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class Enemy
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
    #endregion

    #region Constructor
    public Enemy( GameObject p_enemyAvatar )
    {
        gameObject = p_enemyAvatar;
    }
    #endregion

    public void Kill()
    {
        // Fire kill event


        // Remove from enemy list
        allEnemies.Remove( this );
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
}

public enum EnemyType
{
    Fire,
    Water,
    Wind,
    Dirt
}