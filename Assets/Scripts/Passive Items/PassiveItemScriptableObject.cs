using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName ="ScriptableObject/Passive Item")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    float multipler;
    public float Multipler { get => multipler; private set => multipler = value;  }

    [SerializeField]
    int level; // Not meant to be modified in game [only in the editor]
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab; // The prefab of the next level i.e what object becomes when it levels up
                                // Not to be confused with the prefab to be spawned at the next level
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    new string name;
    public string Name { get => name; private set => name = value; }

    [SerializeField]
    new string description;  // What is the description of this weapon? [if this weapon is an upgrade, place the description of the upgrades]
    public string Description { get => description; private set => description = value; }

    [SerializeField]
    Sprite icon;  //Not mean to be modified in game [only in Editor]
    public Sprite Icon { get => icon; private set => icon = value; }

}
