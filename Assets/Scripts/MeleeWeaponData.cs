using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Weapon Data", menuName = "Data/Melee Weapon Data")]
public class MeleeWeaponData : ScriptableObject
{
    [Header("Основные параметры")]
    public float damage = 25f; 
    public float attackRange = 2f; // 
    public float attackCooldown = 1f; 

    [Header("Звук")]
    public AudioClip attackSound; 

    [Header("Модель")]
    public GameObject weaponModel; 
}