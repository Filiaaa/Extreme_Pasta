using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knife2ndStage : ItemInScene
{
    public AudioSource knifeStringSound;
    public Transform[] firstPoints;
    public Transform[] secondPoints;
    public GameObject butter;
    public Transform butterLastPoint;
    public float knifeSpeed;
    public float butterSpeed;
    public GameObject[] butterTrails;
    public Rigidbody2D rollingPinRb;
    int number = 0;
    public float spawnTime;
    float time;

    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        Throw();
    }

    void Throw()
    {
        switch (caseNumb)
        {
            case 2:
                StartCoroutine(FirstWay());
                break;
            case 3:
                StartCoroutine(SecondWay());
                break;
            default:
                break;
        }
    }
    float angle;
    Vector2 destination;
    IEnumerator FirstWay()
    {
        angle = Vector2.Angle(transform.up, firstPoints[number].position - transform.position);
        destination = (firstPoints[number].position - transform.position).normalized;
        transform.Rotate(new Vector3(0, 0, angle));

        while (true)
        {
            transform.Translate(destination * Time.deltaTime * knifeSpeed, Space.World);


            if (Vector2.Distance(transform.position, firstPoints[number].position) <= 1f)
            {

                if (number == firstPoints.Length - 1)
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                    SpillTheButter();
                    StopCoroutine(FirstWay());
                    break;
                }
                else
                {
                    number += 1;
                    destination = (firstPoints[number].position - transform.position).normalized;
                    angle = Vector2.Angle(transform.up, firstPoints[number].position - transform.position);
                    transform.Rotate(new Vector3(0, 0, angle));
                }

            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    void SpillTheButter()
    {
        butter.SetActive(true);
        StartCoroutine(butterThrow());
    }
    Vector2 butterDestination;
    IEnumerator butterThrow()
    {
        butterDestination = (butterLastPoint.position - butter.transform.position).normalized;
        while (true)
        {
            if (Vector2.Distance(butter.transform.position, butterLastPoint.position) >= 0.05f)
            {
                butter.transform.Translate(butterDestination * Time.deltaTime * butterSpeed);
                yield return new WaitForSeconds(0.02f);
            }
            else
            {
                StopCoroutine(butterThrow());
                break;

            }

        }
        foreach (var trail in butterTrails)
        {
            trail.gameObject.SetActive(true);
            StartCoroutine(Spawn(trail.GetComponent<SpriteRenderer>()));
        }
        


    }

    IEnumerator SecondWay()
    {
        angle = Vector2.Angle(transform.up, secondPoints[number].position - transform.position);
        destination = (secondPoints[number].position - transform.position).normalized;
        if (secondPoints[number].position.x < transform.position.x)
        {
            transform.Rotate(new Vector3(0, 0, angle));
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -angle));
        }


        while (true)
        {
            transform.Translate(destination * Time.deltaTime * knifeSpeed, Space.World);


            if (Vector2.Distance(transform.position, secondPoints[number].position) <= 0.2f)
            {

                if (number == secondPoints.Length - 1)
                {
                    knifeStringSound.Play();
                    GetComponent<SpriteRenderer>().enabled = false;
                    rollingPinRb.isKinematic = false;
                    break;
                }
                else
                {
                    number += 1;
                    destination = (secondPoints[number].position - transform.position).normalized;
                    angle = Vector2.Angle(transform.up, secondPoints[number].position - transform.position);
                    if (secondPoints[number].position.x < transform.position.x)
                    {
                        transform.Rotate(new Vector3(0, 0, angle));
                    }
                    else
                    {
                        transform.Rotate(new Vector3(0, 0, -angle));
                    }
                }

            }
            yield return new WaitForSeconds(0.02f);
        }
    }
    Color newColor;
    IEnumerator Spawn(SpriteRenderer sr)
    {

        while (sr.color.a < 1)
        {
            time += Time.deltaTime;
            newColor = sr.color;
            newColor.a = (time / spawnTime) * 1;
            sr.color = newColor;
            yield return new WaitForSeconds(0.02f);
        }
        
    }


}
