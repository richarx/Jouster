using PrimeTween;
using TMPro;
using UnityEngine;

public class DisplayOpponent : MonoBehaviour
{
    [SerializeField] private RectTransform opponentPanel;
    [SerializeField] private TextMeshProUGUI knightName;
    [SerializeField] private TextMeshProUGUI knightTitle;
    [SerializeField] private TextMeshProUGUI horseName;
    [SerializeField] private TextMeshProUGUI healthStatus;
    [SerializeField] private Transform skillIconHolder;

    [Space]
    [SerializeField] private GameObject skillIconPrefab;

    [Space]
    [SerializeField] private OpponentSelection opponentSelection;

    private float panelWidth;
    private float panelHeight;
    private float panelHalfHeight => panelHeight / 2.0f;

    private Sequence displaySequence;
    private CanvasGroup panelCanvasGroup;

    private void Start()
    {
        panelHeight = opponentPanel.sizeDelta.y;
        panelWidth = opponentPanel.sizeDelta.x;
        panelCanvasGroup = opponentPanel.GetComponent<CanvasGroup>();
        HidePanelInstant();

        opponentSelection.OnOpen.AddListener(() => DisplayPanel(opponentSelection.CurrentSelection));
        opponentSelection.OnClose.AddListener(HidePanel);
        opponentSelection.OnSelectShield.AddListener(() => DisplayPanel(opponentSelection.CurrentSelection));
    }

    public void DisplayPanel(JoustDifficultyData data)
    {
        if (displaySequence.isAlive)
            displaySequence.Stop();

        DeleteSkillPoints();
        knightName.text = "";
        knightTitle.text = "";
        horseName.text = ComputeHorseName("");
        healthStatus.text = "Health Status : ";

        displaySequence = Sequence.Create()
            .Group(Tween.Alpha(panelCanvasGroup, 1.0f, 0.3f))
            .Group(Tween.UISizeDelta(opponentPanel, new Vector2(panelWidth, panelHeight), 0.3f, Ease.OutCirc))
            .ChainCallback(() => knightName.text = data.EnemyData.knightName)
            .ChainDelay(0.05f)
            .ChainCallback(() => knightTitle.text = data.EnemyData.knightTitle)
            .ChainDelay(0.05f)
            .ChainCallback(() => horseName.text = ComputeHorseName(data.EnemyData.horseName))
            .ChainDelay(0.05f)
            .ChainCallback(() => healthStatus.text = ComputeHealthStatus(data.EnemyData.healthStatus))
            .ChainDelay(0.05f)
            .ChainCallback(() => CreateSkillPoints(data.EnemyData.skillLevel));
    }

    private void CreateSkillPoints(int skillLevel)
    {
        for (int i = 0; i < skillLevel; i++)
        {
            Instantiate(skillIconPrefab, skillIconHolder);
        }
    }

    private void DeleteSkillPoints()
    {
        for (int i = skillIconHolder.childCount - 1; i >= 0; i--)
        {
            Destroy(skillIconHolder.GetChild(i).gameObject);
        }
    }

    private string ComputeHorseName(string horseName)
    {
        return $"Riding on : {horseName}";
    }

    private string ComputeHealthStatus(JoustEnemyData.HealthStatus healthStatus)
    {
        string status = "Health Status : ";

        switch (healthStatus)
        {
            case JoustEnemyData.HealthStatus.GoodHealth:
                status += $"Good";
                break;
            case JoustEnemyData.HealthStatus.Injured:
                status += $"Injured";
                break;
            case JoustEnemyData.HealthStatus.BarelyHanging:
                status += $"Barely Hanging";
                break;
        }

        return status;
    }

    public void HidePanel()
    {
        if (displaySequence.isAlive)
            displaySequence.Stop();

        DeleteSkillPoints();
        displaySequence = Sequence.Create()
            .Group(Tween.Alpha(panelCanvasGroup, 0.0f, 0.3f))
            .Group(Tween.UISizeDelta(opponentPanel, new Vector2(panelWidth, panelHalfHeight), 0.3f, Ease.OutCirc));
    }

    public void HidePanelInstant()
    {
        panelCanvasGroup.alpha = 0.0f;
    }
}
