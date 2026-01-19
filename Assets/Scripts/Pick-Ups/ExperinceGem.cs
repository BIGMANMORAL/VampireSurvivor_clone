// To fix CS0101, rename this class to avoid duplicate definition.
// Example: Change 'NewMonoBehaviourScript' to 'ExperienceGemPickUp'.

public class ExperienceGem : PickUp, ICollectible
{
    public int experienceGranted;
    public void Collect()
    {
        PlayerStats player = FindAnyObjectByType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
    }
}
