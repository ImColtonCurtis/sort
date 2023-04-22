using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeLogic : MonoBehaviour
{
    Animator myAnim;
    public bool isActiveObject;

    [SerializeField] int goodDirection;

    private void Awake()
    {
        isActiveObject = false;
        myAnim = gameObject.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (!isActiveObject)
            isActiveObject = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (ControlsLogic.swiped && isActiveObject)
        {
            if (ControlsLogic.swipeDirection == 1) // 1 is left (-1.778)
            {
                if (myAnim != null)
                {
                    myAnim.SetTrigger("moveLeft");
                    if (goodDirection == 1)
                        GameManager.staticSoundManager.Play("SlideDown"); // sliding noise into sliding down noise
                    else
                        GameManager.staticSoundManager.Play("Slide"); // just sliding noise
                }
            }
            else if (ControlsLogic.swipeDirection == 2) // 2 is right (3.859)
            {
                if (myAnim != null)
                {
                    myAnim.SetTrigger("moveRight");
                    if (goodDirection == 2)
                        GameManager.staticSoundManager.Play("SlideDown"); // sliding noise into sliding down noise
                    else
                        GameManager.staticSoundManager.Play("Slide"); // just sliding noise
                }
            }
            else if (ControlsLogic.swipeDirection == 3) //  3 is up (1.088)
            {
                if (myAnim != null)
                {
                    myAnim.SetTrigger("moveUp");
                    if (goodDirection == 3)
                        GameManager.staticSoundManager.Play("SlideDown"); // sliding noise into sliding down noise
                    else
                        GameManager.staticSoundManager.Play("Slide"); // just sliding noise
                }
            }
            else if (ControlsLogic.swipeDirection == 4) // 4 is down (-4.553)
            {
                if (myAnim != null)
                {
                    myAnim.SetTrigger("moveDown");
                    if (goodDirection == 4)
                        GameManager.staticSoundManager.Play("SlideDown"); // sliding noise into sliding down noise
                    else
                        GameManager.staticSoundManager.Play("Slide"); // just sliding noise
                }
            }
            else
            {
                Debug.Log("swipe direction is not set");
            }
            isActiveObject = false;
            ControlsLogic.previousSwipe = ControlsLogic.swipeDirection;
            ControlsLogic.swipeDirection = 0;
            ControlsLogic.swiped = false;
        }
    }
}
