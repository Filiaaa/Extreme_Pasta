using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject tomato;
    public GameObject transitions;
    public Image[] btnImages;
    public Sprite[] usedBtns;
    public Sprite usedTomato;
    public float timeForTransitions;
    public Button continueBtn;

    private void Start()
    {
        Time.timeScale = 1;
/*        PlayerPrefs.DeleteKey("end");
        PlayerPrefs.DeleteKey("checkPoint");
        PlayerPrefs.DeleteKey("secondWay");*/
        if (PlayerPrefs.HasKey("end") || !PlayerPrefs.HasKey(CheckpointSystem.KeyCheckpoint))
        {
            continueBtn.GetComponent<Image>().raycastTarget = false;
            continueBtn.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            continueBtn.GetComponent<Image>().color = Color.white;
        }
    }
    public void SetTomatoPos(Transform btnTransform)
    {
        tomato.transform.position = new Vector3(tomato.transform.position.x,btnTransform.position.y, tomato.transform.position.z);
    }
    public void Play()
    {
        btnImages[0].sprite = usedBtns[0];
        tomato.GetComponent<Image>().sprite = usedTomato;
        transitions.SetActive(true);
        PlayerPrefs.DeleteKey(CheckpointSystem.KeySecondWay);
        PlayerPrefs.SetInt(CheckpointSystem.KeyCheckpoint, 0);
        PlayerPrefs.DeleteKey("end");
        PlayerPrefs.Save();
        StartCoroutine(TransitionWaiting());
    }
    IEnumerator TransitionWaiting()
    {
        yield return new WaitForSeconds(timeForTransitions);
        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        if (PlayerPrefs.HasKey("end"))
        {
            PlayerPrefs.DeleteKey(CheckpointSystem.KeySecondWay);
            PlayerPrefs.SetInt(CheckpointSystem.KeyCheckpoint, 0);
            PlayerPrefs.DeleteKey("end");
            PlayerPrefs.Save();
        }
        btnImages[1].sprite = usedBtns[1];
        tomato.GetComponent<Image>().sprite = usedTomato;
        transitions.SetActive(true);
        StartCoroutine(TransitionWaiting());
    }

    public void Quit()
    {
        btnImages[2].sprite = usedBtns[2];
        tomato.GetComponent<Image>().sprite = usedTomato;
        Application.Quit();
    }
}
