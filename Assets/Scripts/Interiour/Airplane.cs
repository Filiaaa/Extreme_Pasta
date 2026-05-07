using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour
{

    public PlayerMover playerMover;
    public bool playerSit = false;
    public Transform sitPoint;
    public Transform[] wayPoints;
    public float flySpeed;
    public Transform StayUpAirPlane;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerMover.transform.parent = transform;
            playerMover.SitOnAirPlane(sitPoint);
        }
    }


    public void Fly()
    {
        StartCoroutine(FlyCor());
    }

    float angle;
    Vector2 destination;
    int number = 0;
    IEnumerator FlyCor()
    {
        destination = (wayPoints[number].position - transform.position).normalized;


        while (true)
        {
            transform.Translate(destination * Time.deltaTime * flySpeed, Space.World);


            if (Vector2.Distance(transform.position, wayPoints[number].position) <= 0.1f)
            {

                if (number == wayPoints.Length - 1)
                {
                    if (playerMover.transform.parent == transform)
                    {
                        playerMover.StayUpAirPlane(StayUpAirPlane);
                    }

                    playerMover.transform.parent = null;

                    StopCoroutine(FlyCor());
                    break;
                }
                else
                {
                    number += 1;
                    destination = (wayPoints[number].position - transform.position).normalized;
                }

            }
            yield return new WaitForSeconds(0.02f);
        }
    }

}
