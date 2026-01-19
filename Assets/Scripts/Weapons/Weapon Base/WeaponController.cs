using UnityEngine;

/// <summary>
/// Base script for all weapons controllers

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;
    

    protected PlayerMovement pm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        pm = FindFirstObjectByType<PlayerMovement>();
        currentCooldown = weaponData.Cooldownduration; //at the start set the current cooldown to be the cooldown duration
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime; //reduce the current cooldown by the time passed since last frame
        if (currentCooldown <= 0f)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = weaponData.Cooldownduration; //reset the cooldown
    }
}
