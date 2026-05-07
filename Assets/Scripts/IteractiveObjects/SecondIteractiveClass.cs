using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SecondIteractiveClass : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public AudioSource interfaceSound;
    public bool haveTimer = false;
    public GameObject[] timers;
    public bool timerSet = true;
    public int number;
    public StartLevelScript startLevelScript;
    Vector2 offset;
    public bool inCol = false;
    Vector2 windowPos;
    Vector2 startPos;
    public int settedTime = 0;

    private void Start()
    {
        startPos = transform.position;
        if (haveTimer)
        {
            timerSet = false;
        }
    }

    public void SetStartPos()
    {
        transform.position = startPos;
        GetComponent<Image>().raycastTarget = true;
        startLevelScript.secondObjectsUsed[number] = 0;
        startLevelScript.secondWindowNumb--;
        settedTime = 0;
        foreach (var timer in timers)
        {
            timer.SetActive(false);
        }
        timerSet = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "sdIteractionObjectWindow")
        {
            inCol = true;
            windowPos = collision.transform.position;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "sdIteractionObjectWindow")
        {
            inCol = false;
        }
    }


    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        offset = -transform.position + Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 diff = currentMousePosition - offset;
        transform.position = new Vector3(diff.x, diff.y, 0);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
    }

    public void SetTimer(int time, GameObject timerGo)
    {
        startLevelScript.SetMain();
        startLevelScript.secondWindowNumb++;
        startLevelScript.UpdateSecondWindows();

        settedTime = time;
        timerSet = true;
        foreach (var timer in timers)
        {
            if (timer != timerGo)
            {
                timer.SetActive(false);
            }
        }
        startLevelScript.settedObjescts[startLevelScript.lastMainObject[startLevelScript.lastMainObject.Count - 1]].Key = number;
        startLevelScript.settedObjescts[startLevelScript.lastMainObject[startLevelScript.lastMainObject.Count - 1]].Value = timerGo.GetComponent<SetTimer>().time;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (inCol)
        {
            interfaceSound.Play();
            transform.position = windowPos;
            GetComponent<Image>().raycastTarget = false;
            startLevelScript.secondObjectsUsed[number] += 1;
            startLevelScript.SetOffSecondObjects();

            if (haveTimer)
            {
                startLevelScript.lastSecondObject.Add(number);
                foreach (var obj in timers)
                {
                    obj.GetComponentInChildren<Button>().interactable = true;
                    obj.GetComponentInChildren<Slider>().interactable = true;
                    obj.SetActive(true);
                }
            }
            else
            {
                startLevelScript.settedObjescts[startLevelScript.lastMainObject[startLevelScript.lastMainObject.Count - 1]].Key = number;
                startLevelScript.settedObjescts[startLevelScript.lastMainObject[startLevelScript.lastMainObject.Count - 1]].Value = 0;
                startLevelScript.lastSecondObject.Add(number);
                startLevelScript.SetMain();
                startLevelScript.secondWindowNumb++;
                startLevelScript.UpdateSecondWindows();
/*                startLevelScript.lastSecondObject.Add(number);*/
            }

        }
        else
        {
            transform.position = startPos;
        }
    }
}
