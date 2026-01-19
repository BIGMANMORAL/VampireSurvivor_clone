using UnityEngine;

public class GarlicController : WeaponController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnGarlic = Instantiate(weaponData.Prefab);
        spawnGarlic.transform.position = transform.position; // Assign the position to be the same as this object which is parented to the player
        spawnGarlic.transform.parent = transform; // so that is spawns below this object
    }
}
