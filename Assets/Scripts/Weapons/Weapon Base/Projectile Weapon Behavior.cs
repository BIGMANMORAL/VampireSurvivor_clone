using UnityEngine;

/// <summary>
/// Base script for all projectile behaviour [To be placed in a profb of a weaponthat is a projectile]

public class ProjectileWeaponBehavior : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    protected Vector3 direction;
    public float destroyAfterseconds;

    //current stats that can be modified by buffs/debuffs
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected float currentPierce;


    private void Awake()
    {
        //initialize current stats from weapon data
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.Cooldownduration;
        currentPierce = weaponData.Pierce;
    }


    public float GetCurrentDamage()
    {
        return currentDamage *= FindAnyObjectByType<PlayerStats>().CurrentMight;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterseconds);

    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;

        float dirx = direction.x;
        float diry = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if (dir.x < 0 && dir.y == 0) // left facing
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        else if (dir.x == 0 && dir.y < 0)// down
        {
            scale.y = scale.y * -1;
        }
        else if (dir.x == 0 && dir.y > 0) //up
        {
            scale.x = scale.x * -1;
        }
        else if (dir.x > 0 && dir.y > 0) // right up
        {
            rotation.z = 0f;
        } 
        else if (dir.x > 0 && dir.y < 0) //right down
        {
            rotation.z = -90f;
        }
        else if (dir.x < 0 && dir.y > 0) // left up
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = -90f;
        }
        else if (dir.x < 0 && dir.y < 0) // left down
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = 0f;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation); // can't simply set the vector because cannot convert
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        //reffernce the script from the collided collider and deal damage using TakeDamage()
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage()); //Make sure to use currentDamage instead of weaponData.damage in case any damage multipliers in the future
            ReducePierce();
        }
        else if (col.CompareTag("Props"))
        {
            if (col.gameObject.TryGetComponent( out BreakableProps breakable))
            {
                breakable.TakeDamage(GetCurrentDamage());
                ReducePierce();
            }
        }
    }

    void ReducePierce()
    {
        currentPierce--;
        if(currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
