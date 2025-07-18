using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float health;
    public float damage;
    public float walkingSpeed;
    public float runningSpeed;
    public float fieldOfView;
    public float attackRange;
    public float attackDelay;
}