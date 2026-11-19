using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

public class JoustPreparation : MonoBehaviour
{
    [SerializeField] private List<JoustDifficultyData> joustDifficultyDatas;

    public UnityEvent OnRegisterPlayer = new UnityEvent();
    public UnityEvent OnSelectOpponent = new UnityEvent();

    public static JoustPreparation instance;

    public ReadOnlyCollection<JoustDifficultyData> JoustDifficultyDatas => joustDifficultyDatas.AsReadOnly();

    private bool isRegistered;
    public bool IsRegistered => isRegistered;

    private JoustDifficultyData currentOpponent;
    public bool IsOpponentSelected => currentOpponent != null;

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
}
