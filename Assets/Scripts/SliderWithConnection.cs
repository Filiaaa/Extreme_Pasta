using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderWithConnection : MonoBehaviour
{
    public Slider slider;
    public float[] times;

    public void OnValueChanged()
    {
        float maxTime = 0;
        foreach (var time in times)
        {
            if (slider.value > time)
            {
                maxTime = time;
            }
        }
        slider.value = maxTime;
    }
}
