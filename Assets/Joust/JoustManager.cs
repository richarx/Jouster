using System;
using UnityEngine;

public class JoustManager : MonoBehaviour
{
    public enum JoustResult
    {
        None,
        Win,
        Lose,
        Draw
    }

    [Space]
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private ObstacleScroller obstacleScroller;
    [SerializeField] private JoustPlayerStateMachine joustPlayer;
    [SerializeField] private JoustPlayerPower joustPlayerPower;
    [SerializeField] private GroundManager groundManager;

    private void Start()
    {
        JoustDifficultyData data = JoustPreparation.instance.CurrentOpponent;

        obstacleSpawner.Setup(data);
        joustPlayer.Setup(data);
        groundManager.Setup(data);
        joustPlayerPower.Setup(data);

        obstacleScroller.OnFinishScrollingKnight.AddListener(EndJoust);
    }

    private void EndJoust(bool hasPlayBeenHit)
    {
        JoustPreparation.instance.ComeBackFromJoust(JoustResult.Win);
    }
}
