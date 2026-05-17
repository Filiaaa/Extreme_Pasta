using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRollingCut1stStage : ItemInScene
{
    static readonly Dictionary<int, RopeRollingCut1stStage> ActiveByCase = new Dictionary<int, RopeRollingCut1stStage>();

    public GameObject rope;
    [Tooltip("Объект rolling (или другой), вращается вокруг локального pivot")]
    public Transform pivot;
    [Tooltip("Целевой угол по локальной оси Z (Euler)")]
    public float targetLocalEulerZ;
    public float rotationDuration = 1f;

    bool _done;

    void OnEnable()
    {
        ActiveByCase[caseNumb] = this;
        if (pivot == null)
            pivot = transform;
    }

    void OnDisable()
    {
        if (ActiveByCase.TryGetValue(caseNumb, out var current) && current == this)
            ActiveByCase.Remove(caseNumb);
    }

    public static void OnKnifeFinished(int caseNum)
    {
        if (ActiveByCase.TryGetValue(caseNum, out var handler))
            handler.ExecuteCutSequence();
    }

    void ExecuteCutSequence()
    {
        if (_done)
            return;
        _done = true;

        if (rope != null)
            Destroy(rope);

        StartCoroutine(RotatePivot());
    }

    IEnumerator RotatePivot()
    {
        float startZ = pivot.localEulerAngles.z;
        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration);
            float z = Mathf.LerpAngle(startZ, targetLocalEulerZ, t);
            Vector3 euler = pivot.localEulerAngles;
            pivot.localEulerAngles = new Vector3(euler.x, euler.y, z);
            yield return null;
        }

        Vector3 finalEuler = pivot.localEulerAngles;
        pivot.localEulerAngles = new Vector3(finalEuler.x, finalEuler.y, targetLocalEulerZ);
        enabled = false;
    }
}
