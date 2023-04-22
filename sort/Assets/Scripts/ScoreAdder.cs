using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreAdder : MonoBehaviour
{
    [SerializeField] TextMeshPro scoreTMP;
    [SerializeField] Animator myAnim;

    [SerializeField] GameObject shapeDisapear;
    GameObject tempObj;

    [SerializeField] Transform pivotTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shape")
        {
            GameManager.objectsInPlay--;
            tempObj = other.gameObject;
            StartCoroutine(WaitToSpawn());
            myAnim.SetTrigger("bump");
            StartCoroutine(WaitToUpdate());
        }
    }

    IEnumerator WaitToUpdate()
    {
        yield return new WaitForSecondsRealtime(1f / 6f);
        // dont if 2 or more objects are "in play"
        if (GameManager.objectsInPlay < 1)
        {
            GameManager.subtractTime = true;
            GameManager.swipedDown = true;
        }
        GameManager.score++;
        scoreTMP.text = GameManager.score + "";

        int randomChecker = Random.Range(5, 7);
        if (randomChecker == 6 || GameManager.score < 14)
            randomChecker = 10;

        if (GameManager.score % randomChecker == 0 && GameManager.score > 1)
        {
            GameManager.tenner = true;
        }
    }

    IEnumerator WaitToSpawn()
    {
        for (int i = 0; i <= 6; i++)
            yield return new WaitForFixedUpdate();
        //Instantiate(shapeDisapear);
        GameObject myTtempObj =  Instantiate(shapeDisapear, pivotTransform);
        myTtempObj.transform.position = new Vector3(transform.position.x, -5.5f, transform.position.z);
        yield return new WaitForFixedUpdate();
        Destroy(tempObj.transform.parent.gameObject);
    }
}
