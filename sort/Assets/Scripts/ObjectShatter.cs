using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShatter : MonoBehaviour
{
    Vector3 shift;
    float force;

    private void OnEnable()
    {
        GameManager.levelFailed = true;

        GameManager.staticSoundManager.Play("Shatter"); // shatter sound
        GameManager.staticSoundManager.Play("loseJingle"); // shatter sound

        shift = Vector3.zero;

        force = 750f;

        if (ControlsLogic.previousSwipe == 1) // left
            shift = new Vector3(10, 0, 0);
        else if (ControlsLogic.previousSwipe == 2) // right
            shift = new Vector3(-10, 0, 0);
        else if (ControlsLogic.previousSwipe == 3) // up
            shift = new Vector3(0, 0, -10);
        else if (ControlsLogic.previousSwipe == 4) // down
            shift = new Vector3(0, 0, 10);
        else
        {
            shift = new Vector3(Random.Range(-0.5f, 0.5f), 2f, Random.Range(-0.5f, 0.5f));
            force = 850f;
        }

        // get force direction (from swipe direction)
        foreach (Transform child in transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb!= null)
            {
                rb.AddExplosionForce(force, transform.position + shift, 50f);
            }
        }
    }
}
