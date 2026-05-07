using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knife4thStage : ItemInScene
{
    public AudioSource knifeStringSound;
    public Transform[] firstPoints;
    public Transform[] secondPoints;
    public float knifeSpeed;
    public Rigidbody2D solt;
    public Rigidbody2D pelmen_;
    public String string_;
    public SpriteRenderer pelmeni;
    public Sprite usedPelmeni;
    int number = 0;
    public float pelmenSpeed;
    public Transform[] pelmenPoints;
    int pelmenNumber = 0;

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
            case 0:
                StartCoroutine(FirstWay());
                break;
            case 1:
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
        if (firstPoints[number].position.x < transform.position.x)
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


            if (Vector2.Distance(transform.position, firstPoints[number].position) <= 1f)
            {

                if (number == firstPoints.Length - 1)
                {
                    knifeStringSound.Play();
                    GetComponent<SpriteRenderer>().enabled = false;
                    solt.isKinematic = false;
                    string_.enabled = true;
                    break;
                }
                else
                {
                    number += 1;
                    destination = (firstPoints[number].position - transform.position).normalized;
                    angle = Vector2.Angle(transform.up, firstPoints[number].position - transform.position);
                    if (firstPoints[number].position.x < transform.position.x)
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


            if (Vector2.Distance(transform.position, secondPoints[number].position) <= 1f)
            {

                if (number == secondPoints.Length - 1)
                {
                    knifeStringSound.Play();
                    GetComponent<SpriteRenderer>().enabled = false;
                    pelmen_.gameObject.SetActive(true);
                    pelmeni.sprite = usedPelmeni;
                    ThrowPelmen();
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


    void ThrowPelmen()
    {
        StartCoroutine(PelmenThrow());
    }
    Vector2 pelmenDestination;
    IEnumerator PelmenThrow()
    {
        pelmenDestination = (pelmenPoints[pelmenNumber].position - pelmen_.transform.position).normalized;
        while (true)
        {

            pelmenDestination = (pelmenPoints[pelmenNumber].position - pelmen_.transform.position).normalized;
            pelmen_.transform.Translate(pelmenDestination * Time.deltaTime * pelmenSpeed, Space.World);


            if (Vector2.Distance(pelmen_.transform.position, pelmenPoints[pelmenNumber].position) <= 0.2f)
            {

                if (pelmenNumber == pelmenPoints.Length - 1)
                {
                    
                    break;
                }
                else
                {
                    pelmenNumber += 1;
                    pelmenDestination = (pelmenPoints[pelmenNumber].position - pelmen_.transform.position).normalized;
                }

            }
            yield return new WaitForSeconds(0.02f);
        }


    }
}
