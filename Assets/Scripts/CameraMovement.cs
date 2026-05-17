using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDuration = 1.5f;
    public Ease moveEase = Ease.Linear;

    [Header("End Sequence Settings")]
    public float moveDownDuration = 2f;
    public float timeBeforeTransition = 1f;
    public float timeBeforeEnd = 2f;

    [Header("References")]
    public Transform[] points;
    public int pointNumb = -1;
    public GameObject[] levelScripts;
    public PlayerMover playerMover;
    public Transform downEndPoint;
    public GameObject closingTransition;

    [SerializeField] private int nextSceneIndex;
    [SerializeField] private CheckpointSystem checkpointSystem;

    private Tween currentCameraTween;

    private void OnDestroy()
    {
        currentCameraTween?.Kill();
    }

    public void MoveToNextPoint()
    {
        if (pointNumb < 0 || pointNumb >= points.Length) return;
        MoveCameraToTarget(points[pointNumb], moveDuration, moveEase, OnCameraMoveComplete);
    }

    public void StartEnd()
    {
        MoveCameraToTarget(downEndPoint, moveDownDuration, Ease.InOutQuad, OnEndMoveComplete);
    }

    private void MoveCameraToTarget(Transform target, float duration, Ease ease, System.Action onComplete = null)
    {
        currentCameraTween?.Kill();

        currentCameraTween = transform.DOMove(target.position, duration)
            .SetEase(ease)
            .SetUpdate(UpdateType.Fixed)
            .OnComplete(() => {
                onComplete?.Invoke();
                currentCameraTween = null;
            });
    }

    private void OnCameraMoveComplete()
    {
        if (pointNumb >= 0 && pointNumb < levelScripts.Length)
        {
            if (pointNumb <= 3)
            {
                levelScripts[pointNumb].SetActive(true);
            }
            else
            {
                if (playerMover != null)
                    playerMover.enabled = true;
            }
        } else
        {
            if (playerMover != null)
                playerMover.enabled = true;
        }
    }

    private void OnEndMoveComplete()
    {
        checkpointSystem.ResetPlayerPrefs();
        DOVirtual.DelayedCall(timeBeforeTransition, () => {
            if (closingTransition != null)
                closingTransition.SetActive(true);

            DOVirtual.DelayedCall(timeBeforeEnd, () => {
                SceneManager.LoadScene(nextSceneIndex);
            }, false);
        }, false);
    }

    public void MoveToCheckPOint(Transform checkPoint)
    {
        if (checkPoint == null) return;
        MoveCameraToTarget(checkPoint, moveDuration, moveEase, null);
    }

    public void StopCamera()
    {
        currentCameraTween?.Pause();
    }

    public void ResumeCamera()
    {
        currentCameraTween?.Play();
    }

    public void InstantMoveTo(Transform target)
    {
        if (target == null) return;
        currentCameraTween?.Kill();
        transform.position = target.position;
        currentCameraTween = null;
    }
}