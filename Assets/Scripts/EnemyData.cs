using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemy/Create New Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float health;
    public float moveSpeed;
    public float damage;
}