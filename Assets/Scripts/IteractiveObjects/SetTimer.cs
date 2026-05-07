using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTimer : MonoBehaviour
{
    public int time;
    public SecondIteractiveClass secondIteractiveClass;
    public Slider timeSlider;
    public void Clicked()
    {
        time = (int)timeSlider.value;
        timeSlider.interactable = true;
        secondIteractiveClass.SetTimer((int)timeSlider.value, gameObject);
        GetComponentInChildren<Button>().interactable = false;
        GetComponentInChildren<Slider>().interactable = false;
    }
}
