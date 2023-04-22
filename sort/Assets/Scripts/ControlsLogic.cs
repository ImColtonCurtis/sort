using System.Collections;
using System.Collections.Generic;
using Unity.Services.Mediation.Samples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsLogic : MonoBehaviour
{
    bool touchedDown;

    Vector3 startingTapLoc;
    public static int swipeDirection, previousSwipe;
    float swipeRange;
    public static bool swiped;

    [SerializeField] GameObject noIcon;

    [SerializeField] Animator soundAnim;

    int cheatCounter;

    void Awake()
    {
        previousSwipe = 0;
        swipeRange = 0.015f;
        swiped = false;
        touchedDown = false;
        swipeDirection = 0; // 1 is left, 2 is right, 3 is up, 4 is down

        cheatCounter = 0;

        if (PlayerPrefs.GetInt("SoundStatus", 1) == 1)
        {
            noIcon.SetActive(false);
            AudioListener.volume = 1;
        }
        else
        {
            noIcon.SetActive(true);
            AudioListener.volume = 0;
        }
    }

    void OnTouchDown(Vector3 point)
    {
        if (!touchedDown)
        {
            startingTapLoc = point;
            touchedDown = true;

            if (ShowAds.poppedUp)
            {
                if (point.x <= 0)
                    ShowAds.shouldShowRewardedAd = true;
                else
                    ShowAds.dontShow = true;
            }
            else
            {
                // cheat: top-right, top-right, top-left, bottom-right
                // top right tap
                if (!GameManager.levelStarted && (cheatCounter == 0 || cheatCounter == 1) && point.x >= 0.03f && point.y >= 8f)
                {
                    cheatCounter++;
                }
                // top left tap
                else if (!GameManager.levelStarted && (cheatCounter == 2) && point.x <= -0.03f && point.y >= 8f)
                {
                    cheatCounter++;
                }
                // bottom right tap
                else if (!GameManager.levelStarted && (cheatCounter == 3) && point.x >= 0.03f && point.y <= 7.92f)
                {
                    cheatCounter = 0;
                    if (!GameManager.cheatOn)
                        GameManager.cheatOn = true;
                    else
                        GameManager.cheatOn = false;
                }
                else if (!GameManager.levelStarted && point.x <= -0.01f && point.y <= 7.92f) // bottom left button clicked
                {
                    if (PlayerPrefs.GetInt("SoundStatus", 1) == 1)
                    {
                        PlayerPrefs.SetInt("SoundStatus", 0);
                        noIcon.SetActive(true);
                        AudioListener.volume = 0;
                    }
                    else
                    {
                        PlayerPrefs.SetInt("SoundStatus", 1);
                        noIcon.SetActive(false);
                        AudioListener.volume = 1;
                    }
                    soundAnim.SetTrigger("Bump");
                }
                else
                {
                    if (!GameManager.levelFailed)
                    {
                        if (!GameManager.levelStarted)
                            GameManager.levelStarted = true;
                    }
                }
            }
            if (GameManager.levelFailed && !GameManager.isRestarting)
            {
                GameManager.isRestarting = true;
                GameManager.shouldRestart = true;
            }
        }
    }

    void OnTouchStay(Vector3 point)
    {
        if (touchedDown && swipeDirection == 0)
        {
            Vector3 Distance = point - startingTapLoc;
            if (Distance.x < -swipeRange) // left
            {
                switch (GameManager.boardOriantation)
                {
                    case 0:
                        if (!GameManager.cheatOn)
                            swipeDirection = 1;
                        else
                            swipeDirection = 2;
                        break;
                    case 1:
                        if (!GameManager.cheatOn)
                            swipeDirection = 4;
                        else
                            swipeDirection = 3;
                        break;
                    case 2:
                        if (!GameManager.cheatOn)
                            swipeDirection = 2;
                        else
                            swipeDirection = 1;
                        break;
                    case 3:
                        if (!GameManager.cheatOn)
                            swipeDirection = 3;
                        else
                            swipeDirection = 4;
                        break;
                    default:
                        if (!GameManager.cheatOn)
                            swipeDirection = 1;
                        else
                            swipeDirection = 2;
                        break;
                }

                touchedDown = false;
                swiped = true;
            }
            else if (Distance.x > swipeRange) // right
            {
                switch (GameManager.boardOriantation)
                {
                    case 0:
                        if (!GameManager.cheatOn)
                            swipeDirection = 2;
                        else
                            swipeDirection = 1;
                        break;
                    case 1:
                        if (!GameManager.cheatOn)
                            swipeDirection = 3;
                        else
                            swipeDirection = 4;
                        break;
                    case 2:
                        if (!GameManager.cheatOn)
                            swipeDirection = 1;
                        else
                            swipeDirection = 2;
                        break;
                    case 3:
                        if (!GameManager.cheatOn)
                            swipeDirection = 4;
                        else
                            swipeDirection = 3;
                        break;
                    default:
                        if (!GameManager.cheatOn)
                            swipeDirection = 2;
                        else
                            swipeDirection = 1;
                        break;
                }


                touchedDown = false;
                swiped = true;
            }
            else if (Distance.y > swipeRange) // top
            {
                switch (GameManager.boardOriantation)
                {
                    case 0:
                        if (!GameManager.cheatOn)
                            swipeDirection = 3;
                        else
                            swipeDirection = 4;
                        break;
                    case 1:
                        if (!GameManager.cheatOn)
                            swipeDirection = 1;
                        else
                            swipeDirection = 2;
                        break;
                    case 2:
                        if (!GameManager.cheatOn)
                            swipeDirection = 4;
                        else
                            swipeDirection = 3;
                        break;
                    case 3:
                        if (!GameManager.cheatOn)
                            swipeDirection = 2;
                        else
                            swipeDirection = 1;
                        break;
                    default:
                        if (!GameManager.cheatOn)
                            swipeDirection = 3;
                        else
                            swipeDirection = 4;
                        break;
                }

                touchedDown = false;
                swiped = true;
            }
            else if (Distance.y < -swipeRange) // botom
            {
                switch (GameManager.boardOriantation)
                {
                    case 0:
                        if (!GameManager.cheatOn)
                            swipeDirection = 4;
                        else
                            swipeDirection = 3;
                        break;
                    case 1:
                        if (!GameManager.cheatOn)
                            swipeDirection = 2;
                        else
                            swipeDirection = 1;
                        break;
                    case 2:
                        if (!GameManager.cheatOn)
                            swipeDirection = 3;
                        else
                            swipeDirection = 4;
                        break;
                    case 3:
                        if (!GameManager.cheatOn)
                            swipeDirection = 1;
                        else
                            swipeDirection = 2;
                        break;
                    default:
                        if (!GameManager.cheatOn)
                            swipeDirection = 4;
                        else
                            swipeDirection = 3;
                        break;
                }
                touchedDown = false;
                swiped = true;
            }
        }
        else if (touchedDown)
            touchedDown = false;
    }

    void OnTouchUp()
    {
        if (touchedDown)
        {
            touchedDown = false;        
        }
    }

    void OnTouchExit()
    {
        if (touchedDown)
        {
            touchedDown = false;          
        }
    }
}
