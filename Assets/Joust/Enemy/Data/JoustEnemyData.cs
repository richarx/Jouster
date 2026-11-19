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

    public enum HealthStatus
    {
        GoodHealth,
        Injured,
        BarelyHanging
    }

    public WeightClass weightClass;
    public ReactionTime reactionTime;
    public Strategy strategy;

    [Space]
    public GameObject knightPrefab;

    [Space]
    public string knightName;
    public string knightTitle;
    public string horseName;
    public int skillLevel;
    public HealthStatus healthStatus;

    public Color knightColor;
}
