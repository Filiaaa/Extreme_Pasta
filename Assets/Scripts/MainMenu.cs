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
        if (PlayerPrefs.HasKey("end") || !PlayerPrefs.HasKey("checkPoint"))
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
        PlayerPrefs.DeleteKey("secondWay");
        PlayerPrefs.SetInt("checkPoint", 0);
        PlayerPrefs.DeleteKey("end");
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
            PlayerPrefs.DeleteKey("secondWay");
            PlayerPrefs.SetInt("checkPoint", 0);
            PlayerPrefs.DeleteKey("end");
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
