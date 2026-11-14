using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "JoustDifficultyData", menuName = "Scriptable Objects/JoustDifficultyData")]
public class JoustDifficultyData : ScriptableObject
{
    [SerializeField] private List<GameObject> obstaclesPrefab;

    public ReadOnlyCollection<GameObject> ObstaclesPrefab => obstaclesPrefab.AsReadOnly();

    public float minTimeBetweenSpawn;
    public int laneCount;
    public float totalDistance;
}
