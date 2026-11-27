using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

public class JoustPreparation : MonoBehaviour
{
    [SerializeField] private Map.Cities city;
    [SerializeField] private List<JoustDifficultyData> joustDifficultyDatas;
    [SerializeField] private Transform playerMoveTarget;

    public UnityEvent OnRegisterPlayer = new UnityEvent();
    public UnityEvent OnSelectOpponent = new UnityEvent();
    public UnityEvent OnGoToStartJoust = new UnityEvent();
    public UnityEvent OnGoBackInside = new UnityEvent();

    public static JoustPreparation instance;

    public Dictionary<JoustDifficultyData, JoustManager.JoustResult> opponentsData { get; private set; }

    private bool isRegistered;
    public bool IsRegistered => isRegistered;

    private JoustDifficultyData currentOpponent;
    public JoustDifficultyData CurrentOpponent => currentOpponent;
    public bool IsOpponentSelected => currentOpponent != null;
    public Map.Cities targetCity => city;

    private Sequence startJoustSequence;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        opponentsData = new Dictionary<JoustDifficultyData, JoustManager.JoustResult>();
        foreach (JoustDifficultyData data in joustDifficultyDatas)
        {
            opponentsData.Add(data, JoustManager.JoustResult.None);
        }
    }

    private void Start()
    {
        SceneSwapper.instance.OnSwapScene.AddListener((city, target) =>
        {
            if (target == Map.Target.Exterior)
            {
                Destroy(gameObject);
            }
        });
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
            .Chain(Tween.Alpha(BlackScreen.instance.Image, 1.0f, 3.0f, Ease.OutCirc))
            .ChainCallback(() => OnGoToStartJoust?.Invoke())
            .ChainCallback(() => SceneSwapper.instance.LoadScene(city, Map.Target.Joust))
            .Chain(Tween.Alpha(BlackScreen.instance.Image, 0.0f, 0.3f, Ease.OutCirc));
    }

    public void ComeBackFromJoust(JoustManager.JoustResult result)
    {
        if (startJoustSequence.isAlive)
            return;

        opponentsData[currentOpponent] = result;
        currentOpponent = null;

        startJoustSequence = Sequence.Create()
            .Chain(Tween.Alpha(BlackScreen.instance.Image, 1.0f, 3.0f, Ease.OutCirc))
            .ChainCallback(() => OnGoBackInside?.Invoke())
            .ChainCallback(() => SceneSwapper.instance.LoadScene(city, Map.Target.Interior))
            .Chain(Tween.Alpha(BlackScreen.instance.Image, 0.0f, 0.3f, Ease.OutCirc));
    }
}
