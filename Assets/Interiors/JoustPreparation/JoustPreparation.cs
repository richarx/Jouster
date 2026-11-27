using System.Collections.Generic;
using System.Collections.ObjectModel;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class JoustPreparation : MonoBehaviour
{
    [SerializeField] private List<JoustDifficultyData> joustDifficultyDatas;
    [SerializeField] private Transform playerMoveTarget;
    [SerializeField] private Image blackScreen;

    public UnityEvent OnRegisterPlayer = new UnityEvent();
    public UnityEvent OnSelectOpponent = new UnityEvent();
    public UnityEvent OnGoToStartJoust = new UnityEvent();

    public static JoustPreparation instance;

    public ReadOnlyCollection<JoustDifficultyData> JoustDifficultyDatas => joustDifficultyDatas.AsReadOnly();

    private bool isRegistered;
    public bool IsRegistered => isRegistered;

    private JoustDifficultyData currentOpponent;
    public bool IsOpponentSelected => currentOpponent != null;

    private Sequence startJoustSequence;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

    }

    public void RegisterPlayer()
    {
        if (isRegistered)
            return;

        isRegistered = true;
        OnRegisterPlayer?.Invoke();
    }

    public void ValidateOpponent(JoustDifficultyData opponent)
    {
        currentOpponent = opponent;
        OnSelectOpponent?.Invoke();
    }

    public void StartJoust()
    {
        if (startJoustSequence.isAlive)
            return;

        PlayerMovement.instance.SetLockState(true, playerMoveTarget.position, Vector2.up, 9.0f);

        startJoustSequence = Sequence.Create()
            .Chain(Tween.Alpha(blackScreen, 1.0f, 3.0f, Ease.OutCirc))
            .ChainCallback(() => OnGoToStartJoust?.Invoke())
            .ChainCallback(() => SceneSwapper.instance.LoadScene(Map.Cities.Red, Map.Target.Joust));
    }
}
