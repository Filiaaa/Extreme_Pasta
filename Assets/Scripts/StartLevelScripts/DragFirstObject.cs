using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragFirstObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public AudioSource interfaceSound;
    public int number;
    public StartLevelScript startLevelScript;
    Vector2 offset;
    public bool inCol = false;
    Vector2 windowPos;
    Vector2 startPos;
    public GameObject[] secondObjects;
    private void Start()
    {
        startPos = transform.position;
    }
    public void SetStartPos()
    {
        transform.position = startPos;
        startLevelScript.mainObjectsUsed[number] = 0;
        GetComponent<Image>().raycastTarget = true;
        startLevelScript.mainWindowNumb--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "firstIteractionObjectWindow")
        {
            inCol = true;
            windowPos = collision.transform.position;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "firstIteractionObjectWindow")
        {
            inCol = false;
        }
    }


    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        offset = - transform.position + Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 diff = currentMousePosition - offset;
        transform.position = new Vector3(diff.x, diff.y, 0);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (inCol)
        {
            interfaceSound.Play();
            startLevelScript.mainObjectsUsed[number] += 1;
            transform.position = windowPos;
            GetComponent<Image>().raycastTarget = false;
            startLevelScript.SetOffMainObjects();
            startLevelScript.SetSecondObjects(secondObjects);
            startLevelScript.mainWindowNumb++;
            startLevelScript.UpdateMainWindows();
            startLevelScript.lastMainObject.Add(number);

        }
        else
        {
            transform.position = startPos;
        }
    }


}
