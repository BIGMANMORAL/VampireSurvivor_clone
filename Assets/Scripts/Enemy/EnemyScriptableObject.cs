using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject" , menuName = "ScriptableObject/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    //base stats for all enemies
    [SerializeField]
    float movespeed;
    public float Movespeed { get => movespeed; private set => movespeed = value; }

    [SerializeField]
    float maxHealth;
    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }

    [SerializeField]
    float damage;
    public float Damage { get => damage; private set => damage = value; }

}
