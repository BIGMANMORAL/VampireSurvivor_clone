using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollector;
    public float pullspeed;

    void Start()
    {
        player = FindAnyObjectByType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }

    void update()
    {
        //Update the radius of the collector based on the player's collector radius stat
        playerCollector.radius = player.CurrentMagnet;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //check if the other game object has the Icollectible interface
        if (col.gameObject.TryGetComponent(out ICollectible collectible))
        {
            //Pulling animation
            //Gets the rigidbody2D component on the item
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            //Vector2 pointion from the item to the player
            Vector2 forcedirection = (transform.position - col.transform.position).normalized;
            //Applies force tothe item in the forceDirection with the pullspeed
            rb.AddForce(forcedirection * pullspeed);

            //If it does, call the collect method
            collectible.Collect();
        }
    }
}
