using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IteractiveString : InventoryObj
{
    public GameObject left;
    public GameObject right;
    public Transform leftEndPoint;
    public Transform rightEndPoint;
    public Transform newDownLeftPoint;
    public Transform newDownRightPoint;
    public float speed;

    public override void Use(int numberOfItem)
    {
        left.GetComponent<String>().downPoint = newDownLeftPoint;
        right.GetComponent<String>().downPoint = newDownRightPoint;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(LeftMove());
        StartCoroutine(RightMove());
    }



    Vector2 leftDestination;
    IEnumerator LeftMove()
    {
        leftDestination = (leftEndPoint.position - left.transform.position).normalized;
        while (true)
        {
            if (Vector2.Distance(left.transform.position, leftEndPoint.position) >= 0.05f)
            {
                left.transform.Translate(leftDestination * Time.deltaTime * speed);
                yield return new WaitForSeconds(0.02f);
            }
            else
            {
                StopCoroutine(LeftMove());
                break;

            }

        }



    }

    Vector2 rightDestination;
    IEnumerator RightMove()
    {
        rightDestination = (rightEndPoint.position - right.transform.position).normalized;
        while (true)
        {
            if (Vector2.Distance(right.transform.position, rightEndPoint.position) >= 0.05f)
            {
                right.transform.Translate(rightDestination * Time.deltaTime * speed);
                yield return new WaitForSeconds(0.02f);
            }
            else
            {
                StopCoroutine(RightMove());
                break;

            }

        }


    }
}
