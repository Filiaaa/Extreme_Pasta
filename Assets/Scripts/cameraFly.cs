using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFly : MonoBehaviour
{
    public float movingSpeed;
    void Update()
    {
        transform.Translate(Vector2.right * movingSpeed * Time.deltaTime);
    }
}
