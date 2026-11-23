using System;
using PrimeTween;
using UnityEngine;

public class OpponentCursor : MonoBehaviour
{
    [SerializeField] private Transform cursor;
    [SerializeField] private OpponentSelection opponentSelection;

    private Sequence moveSequence;

    private void Start()
    {
        opponentSelection.OnOpen?.AddListener(DisplayCursor);
        opponentSelection.OnClose?.AddListener(HideCursor);
        opponentSelection.OnSelectShield?.AddListener(MoveCursorToCurrentTarget);

        HideCursorInstant();
    }

    private void MoveCursorToCurrentTarget()
    {
        if (moveSequence.isAlive)
            moveSequence.Stop();

        moveSequence = Sequence.Create()
            .Chain(Tween.Position(cursor, opponentSelection.CurrentSelectionPosition, 0.15f, Ease.OutBack));
    }

    private void DisplayCursor()
    {
        cursor.gameObject.SetActive(true);
        MoveCursorToCurrentTarget();
    }

    private void HideCursor()
    {
        cursor.gameObject.SetActive(false);
    }

    private void HideCursorInstant()
    {
        cursor.gameObject.SetActive(false);
    }
}
