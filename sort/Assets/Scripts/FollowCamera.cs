using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;

    [SerializeField] Transform bottom_Border;
    float tracker, clamper, starter;

    public static float targetHeight;

    private void Awake()
    {
        tracker = 0;
        clamper = 0;
        starter = target.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tracker = target.position.y - starter;
        if (tracker > clamper)
            clamper = tracker;
        bottom_Border.transform.position = new Vector3(0, -14.96f+ clamper, 4.5f);

        if (!GameManager.levelFailed)
            transform.position = new Vector3(target.position.x + offset.x, Mathf.Max((target.position.y + offset.y), 2.2f+clamper-0.7f), -10);
        targetHeight = target.position.y;
    }
}