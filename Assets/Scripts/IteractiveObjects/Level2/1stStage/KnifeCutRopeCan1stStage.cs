using System.Collections;
using UnityEngine;

public class KnifeCutRopeCan1stStage : ItemInScene
{
    public AudioSource knifeStringSound;
    public Transform[] waypoints;
    public float knifeSpeed = 3f;
    public float arriveDistance = 0.2f;
    [Tooltip("0 = only for rope (caseNumb 0), 1 = only for dough (caseNumb 1).")]
    public int knifeRole = -1;

    int number;

    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        if (knifeRole >= 0 && caseNumb != knifeRole)
            yield break;
        if (waypoints == null || waypoints.Length == 0)
            yield break;

        StartCoroutine(FlyAlong());
    }

    IEnumerator FlyAlong()
    {
        number = 0;
        float angle = Vector2.Angle(transform.up, waypoints[number].position - transform.position);
        Vector2 destination = (waypoints[number].position - transform.position).normalized;
        RotateTowardPoint(waypoints[number].position, angle);

        while (true)
        {
            transform.Translate(destination * Time.deltaTime * knifeSpeed, Space.World);

            if (Vector2.Distance(transform.position, waypoints[number].position) <= arriveDistance)
            {
                if (number == waypoints.Length - 1)
                {
                    if (knifeStringSound != null)
                        knifeStringSound.Play();
                    GetComponent<SpriteRenderer>().enabled = false;
                    OnKnifeFinished();
                    yield break;
                }

                number++;
                destination = (waypoints[number].position - transform.position).normalized;
                angle = Vector2.Angle(transform.up, waypoints[number].position - transform.position);
                RotateTowardPoint(waypoints[number].position, angle);
            }

            yield return new WaitForSeconds(0.02f);
        }
    }

    void OnKnifeFinished()
    {
        switch (caseNumb)
        {
            case 0:
                RopeCanCut1stStage.OnKnifeAtRope();
                break;
            case 1:
                DoughRise1stStage.OnKnifeCutPacket();
                break;
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
