using UnityEngine;

public class RopeCanCut1stStage : ItemInScene
{
    static RopeCanCut1stStage _active;

    public GameObject rope;
    public Transform canDropPoint;
    public GameObject can;

    bool _done;

    void OnEnable()
    {
        _active = this;
        // if (can != null)
        //     can.SetActive(false);
    }

    void OnDisable()
    {
        if (_active == this)
            _active = null;
    }

    public static void OnKnifeAtRope()
    {
        if (_active != null)
            _active.CutAndDropCan();
    }

    void CutAndDropCan()
    {
        if (_done)
            return;
        _done = true;

        if (rope != null)
            Destroy(rope);

        if (can == null)
            return;

        // if (canDropPoint != null)
        //     can.transform.position = canDropPoint.position;

        can.SetActive(true);
        Rigidbody2D rb = can.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.isKinematic = false;
    }
}
