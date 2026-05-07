using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject tomato;
    public GameObject transitions;
    public Image[] btnImages;
    public Sprite[] usedBtns;
    public Sprite usedTomato;
    public float timeForTransitions;

    public void SetTomatoPos(Transform btnTransform)
    {
        tomato.transform.position = new Vector3(tomato.transform.position.x, btnTransform.position.y, tomato.transform.position.z);
    }

    public void Quit()
    {
        btnImages[1].sprite = usedBtns[1];
        tomato.GetComponent<Image>().sprite = usedTomato;
        transitions.SetActive(true);
        StartCoroutine(TransitionWaiting());
    }
    public void Continue()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);

    }
    IEnumerator TransitionWaiting()
    {
        yield return new WaitForSecondsRealtime(timeForTransitions);
        SceneManager.LoadScene(0);
    }

}
