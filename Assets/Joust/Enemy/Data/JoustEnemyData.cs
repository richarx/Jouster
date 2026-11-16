using UnityEngine;

[CreateAssetMenu(fileName = "JoustEnemyData", menuName = "Scriptable Objects/JoustEnemyData")]
public class JoustEnemyData : ScriptableObject
{
    public enum WeightClass
    {
        Light,
        Medium,
        Heavy
    }

    public enum ReactionTime
    {
        Quick,
        Medium,
        Slow
    }

    public WeightClass weightClass;
    public ReactionTime reactionTime;
    public Strategy strategy;

    [Space]
    public GameObject knightPrefab;
}
