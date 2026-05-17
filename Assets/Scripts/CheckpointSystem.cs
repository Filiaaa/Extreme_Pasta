using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointSystem : MonoBehaviour
{
    public const string KeyCheckpoint = "checkPoint";
    public const string KeySecondWay = "secondWay";

    const string LegacyKeyCheckpointIndex = "CP_Index";
    const string LegacyKeyPathPrefix = "CP_Path_";

    [Header("References")]
    public StarSys starSys;
    public CameraMovement cameraMovement;
    public GameObject player;
    public GameObject learningObject;

    [Header("Points And Level Objects")]
    public Transform[] playerPoints;
    public Transform[] playerSecondWayPoints;
    public Transform[] cameraPoints;
    public GameObject[] colsBeforeLevel;
    public GameObject[] checkPointCols;

    [SerializeField]
    float restoreGraceSeconds = 0.75f;

    int currentCheckpointIndex;
    bool resumeOnSecondWay;
    bool isRestoringFromLoad;

    void Awake()
    {
        isRestoringFromLoad = true;
        LoadCheckpointState();
    }

    void Start()
    {
        InitializeLevelState();
        RestoreCheckpointState();
        StartCoroutine(EndRestoreGrace());
    }

    IEnumerator EndRestoreGrace()
    {
        yield return new WaitForSeconds(restoreGraceSeconds);
        isRestoringFromLoad = false;
    }

    void LoadCheckpointState()
    {
        MigrateLegacyPrefsIfNeeded();

        int maxIndex = GetMaxCheckpointIndex();
        currentCheckpointIndex = PlayerPrefs.GetInt(KeyCheckpoint, 0);
        currentCheckpointIndex = Mathf.Clamp(currentCheckpointIndex, 0, maxIndex);
        resumeOnSecondWay = PlayerPrefs.GetInt(KeySecondWay, 0) == 1;
    }

    void MigrateLegacyPrefsIfNeeded()
    {
        if (PlayerPrefs.HasKey(KeyCheckpoint))
            return;

        if (!PlayerPrefs.HasKey(LegacyKeyCheckpointIndex))
            return;

        int legacyIndex = PlayerPrefs.GetInt(LegacyKeyCheckpointIndex, 0);
        PlayerPrefs.SetInt(KeyCheckpoint, legacyIndex);

        if (PlayerPrefs.HasKey(LegacyKeyPathPrefix + legacyIndex))
            PlayerPrefs.SetInt(KeySecondWay, PlayerPrefs.GetInt(LegacyKeyPathPrefix + legacyIndex, 0));

        PlayerPrefs.Save();
    }

    void SaveCheckpointState()
    {
        PlayerPrefs.SetInt(KeyCheckpoint, currentCheckpointIndex);
        PlayerPrefs.SetInt(KeySecondWay, resumeOnSecondWay ? 1 : 0);
        PlayerPrefs.Save();
    }

    int GetMaxCheckpointIndex()
    {
        int max = 0;
        if (playerPoints != null)
            max = Mathf.Max(max, playerPoints.Length - 1);
        if (playerSecondWayPoints != null)
            max = Mathf.Max(max, playerSecondWayPoints.Length - 1);
        if (cameraPoints != null)
            max = Mathf.Max(max, cameraPoints.Length - 1);
        if (checkPointCols != null)
            max = Mathf.Max(max, checkPointCols.Length - 1);
        return Mathf.Max(max, 0);
    }

    void InitializeLevelState()
    {
        if (learningObject != null)
            learningObject.SetActive(currentCheckpointIndex == 0);

        UpdateCheckpointColliders();
    }

    void UpdateCheckpointColliders()
    {
        if (checkPointCols == null)
            return;

        for (int i = 0; i < checkPointCols.Length; i++)
        {
            if (checkPointCols[i] != null)
                checkPointCols[i].SetActive(i >= currentCheckpointIndex);
        }
    }

    void RestoreCheckpointState()
    {
        SetPlayerPosition();
        SetCameraPosition();
        ActivateCheckpointSegments();
        ActivateStartLevelCanvas();
        ResetPlayerStats();
    }

    void SetPlayerPosition()
    {
        if (player == null)
            return;

        Transform[] targetPoints = resumeOnSecondWay ? playerSecondWayPoints : playerPoints;
        if (targetPoints == null || currentCheckpointIndex >= targetPoints.Length)
            return;

        if (targetPoints[currentCheckpointIndex] == null)
            return;

        player.transform.position = targetPoints[currentCheckpointIndex].position;
    }

    void SetCameraPosition()
    {
        if (cameraMovement == null || cameraPoints == null)
            return;

        int camIndex = Mathf.Clamp(currentCheckpointIndex, 0, cameraPoints.Length - 1);
        if (cameraPoints[camIndex] == null)
            return;

        cameraMovement.transform.position = cameraPoints[camIndex].position;
        cameraMovement.pointNumb = camIndex;
    }

    void ActivateCheckpointSegments()
    {
        if (colsBeforeLevel == null)
            return;

        for (int i = 0; i < colsBeforeLevel.Length; i++)
        {
            if (colsBeforeLevel[i] != null)
                colsBeforeLevel[i].SetActive(i <= currentCheckpointIndex);
        }
    }

    void ActivateStartLevelCanvas()
    {
        GameObject[] levelScripts = cameraMovement != null ? cameraMovement.levelScripts : null;
        if (levelScripts == null || levelScripts.Length == 0)
        {
            SetPlayerMovementEnabled(true);
            return;
        }

        for (int i = 0; i < levelScripts.Length; i++)
        {
            if (levelScripts[i] != null)
                levelScripts[i].SetActive(false);
        }

        if (currentCheckpointIndex < levelScripts.Length && levelScripts[currentCheckpointIndex] != null)
        {
            levelScripts[currentCheckpointIndex].SetActive(true);
            SetPlayerMovementEnabled(false);
            return;
        }

        SetPlayerMovementEnabled(true);
    }

    void SetPlayerMovementEnabled(bool enabled)
    {
        if (player == null)
            return;

        PlayerMover mover = player.GetComponent<PlayerMover>();
        if (mover != null)
            mover.enabled = enabled;
    }

    void ResetPlayerStats()
    {
        if (starSys != null)
            starSys.SetToDefault();
    }

    int GetCheckpointColliderIndex(BoxCollider2D col)
    {
        if (checkPointCols == null || col == null)
            return -1;

        for (int i = 0; i < checkPointCols.Length; i++)
        {
            if (checkPointCols[i] == null)
                continue;

            if (col.gameObject == checkPointCols[i])
                return i;

            if (col.transform.IsChildOf(checkPointCols[i].transform))
                return i;
        }

        return -1;
    }

    /// <summary>
    /// Called when the player enters a checkpoint trigger.
    /// Returns false if the checkpoint should be ignored (restore grace, wrong order).
    /// </summary>
    public bool TryAdvanceCheckpoint(BoxCollider2D col, bool isSecondWay)
    {
        if (col == null || isRestoringFromLoad)
            return false;

        int colIndex = GetCheckpointColliderIndex(col);

        if (colIndex >= 0)
        {
            if (colIndex < currentCheckpointIndex)
                return false;

            if (colIndex > currentCheckpointIndex)
                return false;
        }

        AdvanceCheckpoint(isSecondWay);
        col.enabled = false;
        return true;
    }

    void AdvanceCheckpoint(bool isSecondWay)
    {
        if (learningObject != null)
            learningObject.SetActive(false);

        int maxIndex = GetMaxCheckpointIndex();
        currentCheckpointIndex = Mathf.Min(currentCheckpointIndex + 1, maxIndex);
        resumeOnSecondWay = isSecondWay;

        SaveCheckpointState();

        if (starSys != null)
            starSys.SetToDefault();

        UpdateCheckpointColliders();
        ActivateCheckpointSegments();
    }

    public void RespawnPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteKey(KeyCheckpoint);
        PlayerPrefs.DeleteKey(KeySecondWay);
        PlayerPrefs.DeleteKey(LegacyKeyCheckpointIndex);
        PlayerPrefs.Save();
    }

    public void ResetSession()
    {
        PlayerPrefs.DeleteKey(KeyCheckpoint);
        PlayerPrefs.DeleteKey(KeySecondWay);
        PlayerPrefs.DeleteKey(LegacyKeyCheckpointIndex);

        for (int i = 0; i <= GetMaxCheckpointIndex(); i++)
            PlayerPrefs.DeleteKey(LegacyKeyPathPrefix + i);

        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetCheckpointIndex() => currentCheckpointIndex;

    public bool IsOnSecondWay() => resumeOnSecondWay;
}
