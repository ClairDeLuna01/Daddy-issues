using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    public Vector3 position;
    public Quaternion rotation;
    public int health;
    public bool aggro;
    public Enemy.EnemyType enemyType;

    public EnemyState(Vector3 position, Quaternion rotation, int health, Enemy.EnemyType enemyType, bool aggro)
    {
        this.position = position;
        this.rotation = rotation;
        this.health = health;
        this.enemyType = enemyType;
        this.aggro = aggro;
    }
}

[System.Serializable]
public class GameState
{
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public bool armor;
    public bool godMode;
    public bool infiniteEnergy;
    public int energy;

    public EnemyState[] AliveEnemies;
    public bool[] completedArenas;

    public GameState(Vector3 playerPosition, Quaternion playerRotation, bool armor, bool godMode, bool infiniteEnergy, int energy, EnemyState[] AliveEnemies, bool[] completedArenas)
    {
        this.playerPosition = playerPosition;
        this.playerRotation = playerRotation;
        this.armor = armor;
        this.godMode = godMode;
        this.infiniteEnergy = infiniteEnergy;
        this.energy = energy;
        this.AliveEnemies = AliveEnemies;
        this.completedArenas = completedArenas;
    }
}
