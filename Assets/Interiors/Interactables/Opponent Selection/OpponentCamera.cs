using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;

public class OpponentCamera : MonoBehaviour
{
    [SerializeField] private OpponentSelection opponentSelection;
    [SerializeField] private CinemachineCamera camera;

    [Space]
    [SerializeField] private float horizontalOffset;

    private Sequence moveSequence;

    private void Start()
    {
        opponentSelection.OnOpen.AddListener(() => SetCameraState(true));
        opponentSelection.OnClose.AddListener(() => SetCameraState(false));
        opponentSelection.OnSelectShield.AddListener(() => MoveCamera());
    }

    private void MoveCamera()
    {
        if (moveSequence.isAlive)
            moveSequence.Stop();

        moveSequence = Sequence.Create()
            .Chain(Tween.PositionX(camera.transform, opponentSelection.CurrentSelectionPosition.x + horizontalOffset, 0.5f, Ease.InOutCirc));
    }

    private void SetCameraState(bool state)
    {
        camera.gameObject.SetActive(state);
    }
}
