using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointSystem : MonoBehaviour
{
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

    private int currentCheckpointIndex;
    private bool[] checkpointPathChoices;

    private const string KEY_CHECKPOINT_INDEX = "CP_Index";
    private const string KEY_PATH_CHOICE_PREFIX = "CP_Path_";

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        int checkpointCount = playerPoints != null ? playerPoints.Length : 0;
        checkpointPathChoices = new bool[checkpointCount];
        LoadCheckpointState();
    }

    private void Start()
    {
        InitializeLevelState();
        RestoreCheckpointState();
    }

    private void LoadCheckpointState()
    {
        currentCheckpointIndex = PlayerPrefs.GetInt(KEY_CHECKPOINT_INDEX, 0);
        currentCheckpointIndex = Mathf.Clamp(currentCheckpointIndex, 0, checkpointPathChoices.Length - 1);

        for (int i = 0; i < checkpointPathChoices.Length; i++)
        {
            checkpointPathChoices[i] = PlayerPrefs.GetInt(KEY_PATH_CHOICE_PREFIX + i, 0) == 1;
        }
    }

    private void SaveCheckpointState()
    {
        PlayerPrefs.SetInt(KEY_CHECKPOINT_INDEX, currentCheckpointIndex);
        for (int i = 0; i < checkpointPathChoices.Length; i++)
        {
            PlayerPrefs.SetInt(KEY_PATH_CHOICE_PREFIX + i, checkpointPathChoices[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    private void InitializeLevelState()
    {
        if (learningObject != null)
            learningObject.SetActive(currentCheckpointIndex == 0);

        UpdateCheckpointColliders();
    }

    private void UpdateCheckpointColliders()
    {
        if (checkPointCols == null) return;

        for (int i = 0; i < checkPointCols.Length; i++)
        {
            checkPointCols[i].SetActive(i >= currentCheckpointIndex);
        }
    }

    private void RestoreCheckpointState()
    {
        SetPlayerPosition();
        SetCameraPosition();
        ActivateCheckpointSegments();
        ResetPlayerStats();

        cameraMovement.levelScripts[currentCheckpointIndex].SetActive(true);

        //if (cameraMovement != null && cameraMovement.levelScripts != null && cameraMovement.levelScripts.Length > 0)
        //{
        //    cameraMovement.levelScripts[0].SetActive(true);
        //}
    }

    private void SetPlayerPosition()
    {
        if (player == null) return;

        bool isSecondWay = checkpointPathChoices[currentCheckpointIndex];
        Transform[] targetPoints = isSecondWay ? playerSecondWayPoints : playerPoints;

        if (targetPoints != null && currentCheckpointIndex < targetPoints.Length)
        {
            player.transform.position = targetPoints[currentCheckpointIndex].position;
        }
    }

    private void SetCameraPosition()
    {
        if (cameraMovement == null || cameraPoints == null) return;

        int camIndex = Mathf.Max(0, currentCheckpointIndex);
        if (camIndex < cameraPoints.Length)
        {
            cameraMovement.transform.position = cameraPoints[camIndex].position;
            cameraMovement.pointNumb = camIndex;
        }
    }

    private void ActivateCheckpointSegments()
    {
        if (colsBeforeLevel != null && currentCheckpointIndex < colsBeforeLevel.Length)
            colsBeforeLevel[currentCheckpointIndex].SetActive(true);
    }

    private void ResetPlayerStats()
    {
        if (starSys != null)
            starSys.SetToDefault();
    }

    public void SetCheckpoint(BoxCollider2D col, bool isSecondWay)
    {
        if (col == null) return;

        learningObject.SetActive(false);

        currentCheckpointIndex++;
        currentCheckpointIndex = Mathf.Clamp(currentCheckpointIndex, 0, checkpointPathChoices.Length - 1);
        checkpointPathChoices[currentCheckpointIndex] = isSecondWay;

        SaveCheckpointState();

        col.enabled = false;
        starSys.SetToDefault();
        UpdateCheckpointColliders();
        ActivateCheckpointSegments();
    }

    public void RespawnPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetSession()
    {
        PlayerPrefs.DeleteKey(KEY_CHECKPOINT_INDEX);
        for (int i = 0; i < checkpointPathChoices.Length; i++)
        {
            PlayerPrefs.DeleteKey(KEY_PATH_CHOICE_PREFIX + i);
        }
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
