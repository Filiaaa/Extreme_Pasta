using System.Collections;
using UnityEngine;

public class KnifeLevel21stStage : ItemInScene
{
    public AudioSource knifeStringSound;
    public Transform[] firstPoints;
    public Transform[] secondPoints;
    public float knifeSpeed = 3f;
    public float arriveDistance = 0.2f;

    int number;

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
        number = 0;
        switch (caseNumb)
        {
            case 0:
                if (firstPoints != null && firstPoints.Length > 0)
                    StartCoroutine(FlyAlong(firstPoints));
                break;
            case 1:
                if (secondPoints != null && secondPoints.Length > 0)
                    StartCoroutine(FlyAlong(secondPoints));
                break;
        }
    }

    IEnumerator FlyAlong(Transform[] points)
    {
        float angle = Vector2.Angle(transform.up, points[number].position - transform.position);
        Vector2 destination = (points[number].position - transform.position).normalized;
        RotateTowardPoint(points[number].position, angle);

        while (true)
        {
            transform.Translate(destination * Time.deltaTime * knifeSpeed, Space.World);

            if (Vector2.Distance(transform.position, points[number].position) <= arriveDistance)
            {
                if (number == points.Length - 1)
                {
                    if (knifeStringSound != null)
                        knifeStringSound.Play();
                    GetComponent<SpriteRenderer>().enabled = false;
                    RopeRollingCut1stStage.OnKnifeFinished(caseNumb);
                    yield break;
                }

                number++;
                destination = (points[number].position - transform.position).normalized;
                angle = Vector2.Angle(transform.up, points[number].position - transform.position);
                RotateTowardPoint(points[number].position, angle);
            }

            yield return new WaitForSeconds(0.02f);
        }
    }

    void RotateTowardPoint(Vector3 point, float angle)
    {
        if (point.x < transform.position.x)
            transform.Rotate(0f, 0f, angle);
        else
            transform.Rotate(0f, 0f, -angle);
    }
}
