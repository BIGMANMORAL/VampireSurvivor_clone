using UnityEngine;

public class KnifeController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedKnife = Instantiate(weaponData.Prefab);
        spawnedKnife.transform.position = transform.position; // Assign the position to be the same as this object which is parented to the player
        spawnedKnife.GetComponent<KnifeBehavior>().DirectionChecker(pm.lastMoveVector); // Set the direction of the knife to be the same as the player's facing direction
    }  
}
