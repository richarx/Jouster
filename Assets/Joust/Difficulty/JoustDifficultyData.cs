using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "JoustDifficultyData", menuName = "Scriptable Objects/JoustDifficultyData")]
public class JoustDifficultyData : ScriptableObject
{
    [SerializeField] private JoustEnemyData enemyData;

    [Space]
    [SerializeField] private List<GameObject> earlyChunksPool;
    [SerializeField] private List<GameObject> lateChunksPool;

    [Space]
    [SerializeField] private List<GameObject> shortObstacles;
    [SerializeField] private List<GameObject> tallObstacles;
    [SerializeField] private List<GameObject> tall_X2_Obstacles;
    [SerializeField] private List<GameObject> short_X3_Obstacles;

    public JoustEnemyData EnemyData => enemyData;

    public ReadOnlyCollection<GameObject> earlyChunks => earlyChunksPool.AsReadOnly();
    public ReadOnlyCollection<GameObject> lateChunks => lateChunksPool.AsReadOnly();

    public ReadOnlyCollection<GameObject> ShortObstacles => shortObstacles.AsReadOnly();
    public ReadOnlyCollection<GameObject> TallObstacles => tallObstacles.AsReadOnly();
    public ReadOnlyCollection<GameObject> Tall_X2_Obstacles => tall_X2_Obstacles.AsReadOnly();
    public ReadOnlyCollection<GameObject> Short_X3_Obstacles => short_X3_Obstacles.AsReadOnly();

    [Space]
    public int laneCount;
    public float totalDistance;

    [Space]
    public float targetPower;
}
