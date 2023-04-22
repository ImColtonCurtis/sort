using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool levelStarted, levelFailed, shouldRestart, subtractTime, swipedDown, isRestarting, cheatOn, tenner, rotatingStage;

    public static int score, objectsInPlay, boardOriantation;

    [SerializeField] Transform levelObjectsFolder;

    [SerializeField]
    Camera myCam;

    [SerializeField] Transform spawnFolder;

    [SerializeField] TextMeshPro scoreTMP;
    [SerializeField] SpriteRenderer highscoreText, highscoreTextBG, swipeText, swipeBG, sortText, sortBGIMG, fullSquare, retryText, retryBG, solidShapeBar;

    bool scoreSet, restartFadedIn;

    [SerializeField] GameObject[] shapes = new GameObject[4];

    [SerializeField] Animator cameraShakeAnim;
    int prevTimer;

    [SerializeField] SpriteRenderer[] soundIcons;

    [SerializeField] SoundManagerLogic mySoundManager;
    public static SoundManagerLogic staticSoundManager;

    [SerializeField] AudioSource mainMenuMusic;

    [SerializeField] GameObject activatedObj, deactivatedObj, mirrorModeText;

    [SerializeField] Transform mainPivot, shapeBarPos;

    [SerializeField] Material bgMat;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        StartCoroutine(FadeImageOut(fullSquare, 30));

        restartFadedIn = false;
        shouldRestart = false;
        levelStarted = false;
        levelFailed = false;
        scoreSet = false;
        subtractTime = false;
        isRestarting = false;
        swipedDown = false;
        tenner = false;
        rotatingStage = false;
        score = 0;
        objectsInPlay = 0;
        boardOriantation = 0;

        shapeBarPos.transform.localPosition = new Vector3(-3.93f, 5.55f, 0);

        bgMat.color = new Color(0.4941f, 0.4826f, 0.4078f);

        StartCoroutine(Spawner());

        staticSoundManager = mySoundManager;

        cheatOn = false;

        if (PlayerPrefs.GetInt("EggEnabled", 0) == 0) // is off
        {
            mirrorModeText.SetActive(false);
            scoreTMP.text = PlayerPrefs.GetInt("mainhighScore", 0) + "";
            cheatOn = false;
        }
        else if (PlayerPrefs.GetInt("EggEnabled", 0) == 1) // is on
        {
            mirrorModeText.SetActive(true);

            scoreTMP.text = PlayerPrefs.GetInt("egghighScore", 0) + "";
            cheatOn = true;
        }
    }

    // SOUNDS EXIST IN: ObjectShatter.cs, ShapeLogic.cs, and PlayerController.cs

    IEnumerator RotatePivot()
    {
        float timer = 0, totalTimer = 120;

        int rotateInt = Random.Range(0, 2);
        if (rotateInt == 0)
            rotateInt = -1;

        float startRot = mainPivot.transform.eulerAngles.y;
        int newAngle = 0;

        rotatingStage = true;

        switch (Random.Range(0, 3))
        {
            case 0:
                newAngle = 90;
                break;
            case 1:
                newAngle = 180;
                break;
            case 2:
                newAngle = 270;
                break;
            default:
                newAngle = 90;
                break;
        }

        if (score < 15)
            newAngle = 90;

        float endRot = startRot + (newAngle * rotateInt);

        Color startColor = bgMat.color;
        Color endColor = new Color(0.4941f, 0.4826f, 0.4078f);

        switch ((endRot+360)%360)
        {
            case 0:
                boardOriantation = 0;
                endColor = new Color(0.4941f, 0.4826f, 0.4078f);
                break;
            case 90:
                boardOriantation = 1;
                endColor = new Color(0.4078431f, 0.4941176f, 0.4394771f);
                break;
            case 180:
                boardOriantation = 2;
                endColor = new Color(0.4078431f, 0.4193464f, 0.4941176f);
                break;
            case 270:
                boardOriantation = 3;
                endColor = new Color(0.4941176f, 0.4078431f, 0.4624836f);
                break;
            default:
                boardOriantation = 0;
                endColor = new Color(0.4941f, 0.4826f, 0.4078f);
                break;
        }
        
        while (timer <= totalTimer)
        {
            bgMat.color = Color.Lerp(startColor, endColor, timer/totalTimer);
            mainPivot.transform.eulerAngles = Vector3.Lerp(new Vector3(0, startRot, 0), new Vector3(0, endRot, 0), timer / totalTimer);
            yield return new WaitForFixedUpdate();
            timer++;
        }

        rotatingStage = false;
    }

    private void Update()
    {
        if (cheatOn && PlayerPrefs.GetInt("EggEnabled", 0) == 0) // turn on
        {
            mirrorModeText.SetActive(true);
            activatedObj.SetActive(true);
            deactivatedObj.SetActive(false);
            scoreTMP.text = PlayerPrefs.GetInt("egghighScore", 0) + "";
            PlayerPrefs.SetInt("EggEnabled", 1);
        }
        else if (!cheatOn && PlayerPrefs.GetInt("EggEnabled", 0) == 1) // turn off
        {
            mirrorModeText.SetActive(false);
            deactivatedObj.SetActive(true);
            activatedObj.SetActive(false);
            scoreTMP.text = PlayerPrefs.GetInt("mainhighScore", 0) + "";
            PlayerPrefs.SetInt("EggEnabled", 0);
        }

        if (tenner)
        {
            StartCoroutine(RotatePivot());
            tenner = false;
        }

        if (levelStarted && !scoreSet)
        {
            scoreTMP.text = score + "";

            foreach (SpriteRenderer sprite in soundIcons)
            {
                StartCoroutine(FadeImageOut(sprite, 24));
            }

            mirrorModeText.SetActive(false);

            StartCoroutine(FadeOutAudio(mainMenuMusic));

            StartCoroutine(FadeImageOut(highscoreText, 24));
            StartCoroutine(FadeImageOut(highscoreTextBG, 24));
            StartCoroutine(FadeImageOut(swipeText, 18));
            StartCoroutine(FadeImageOut(swipeBG, 18));
            StartCoroutine(FadeImageOut(sortText, 18));
            StartCoroutine(FadeImageOut(sortBGIMG, 30));

            scoreSet = true;
        }

        if (levelFailed && !restartFadedIn)
        {
            // set highscore
            if (PlayerPrefs.GetInt("EggEnabled", 0) == 0) // is off
            {
                if (score > PlayerPrefs.GetInt("mainhighScore", 0))
                    PlayerPrefs.SetInt("mainhighScore", score);
            }
            else if (PlayerPrefs.GetInt("EggEnabled", 0) == 1) // is on
            {
                if (score > PlayerPrefs.GetInt("egghighScore", 0))
                    PlayerPrefs.SetInt("egghighScore", score);
            }

            PlayerPrefs.SetInt("PointsSinceLastAdPop", PlayerPrefs.GetInt("PointsSinceLastAdPop", 0) + score);
            StartCoroutine(RestartWait());
            restartFadedIn = true;
        }

        if (levelFailed && shouldRestart)
        {
            StartCoroutine(RestartLevel(fullSquare));
            shouldRestart = false;
        }

        if (subtractTime && score == 1)
        {
            StartCoroutine(Spawner());
            subtractTime = false;
        }
    }
    IEnumerator FadeOutAudio(AudioSource myAudio)
    {
        float timer = 0, totalTime = 24;
        float startingLevel = myAudio.volume;
        while (timer <= totalTime)
        {
            myAudio.volume = Mathf.Lerp(startingLevel, 0, timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }

    IEnumerator Spawner()
    {
        int timer = Random.Range(14, 32);
        if (prevTimer < 20 && timer < 20)
            timer = Random.Range(20, 35);
        prevTimer = timer;
        int spawnInt = Random.Range(0, 6);
        if (score > 3)
            spawnInt = Random.Range(0, 9);
        else if (score == 3)
            spawnInt = 6;
        else if (score == 0)
            timer = 0;

        shapeBarPos.transform.localPosition = new Vector3(-3.93f, 5.55f, 0);
        solidShapeBar.enabled = true;
        solidShapeBar.color = Color.white;

        timer *= 6;

        for (int i = 0; i < timer; i++)
        {
            // its subtracting right after new one spawns
            if (subtractTime)
            {
                subtractTime = false;
                i = (int)(timer * 0.95f);
            }
            while (rotatingStage)
                yield return new WaitForSecondsRealtime(0.1f);

            if (score > 0)
                shapeBarPos.transform.localPosition = Vector3.Lerp(new Vector3(-4.74f, 6.25f, 0), new Vector3(-0.72f, 1.48f, 0), (float)i / (float)timer);

            yield return new WaitForFixedUpdate();

            if (levelFailed)
                break;
        }
        if (!swipedDown)
        {
            StartCoroutine(FadeImageOut(solidShapeBar, 12));
            yield return new WaitForSecondsRealtime(0.4f);
        }
        else
            swipedDown = false;

        int shapeInt = 0;
        if (spawnInt < 3)
            shapeInt = 0;
        else if (spawnInt >= 3 && spawnInt < 6)
            shapeInt = 1;
        else if(spawnInt >= 6 && spawnInt < 8)
            shapeInt = 2;
        else
            shapeInt = 3;
        GameObject tempObj;
        if (!levelFailed)
        {
            tempObj = Instantiate(shapes[shapeInt], spawnFolder);
            StartCoroutine(ShakeCamera());
        }
        objectsInPlay++;
        if (!levelFailed && score > 0)
        {
            StartCoroutine(Spawner());
        }
    }

    IEnumerator ShakeCamera()
    {
        yield return new WaitForSeconds(7f/60f);
        cameraShakeAnim.SetTrigger("shake");
    }

    IEnumerator RestartWait()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        StartCoroutine(FadeImageIn(retryText, 48));
        StartCoroutine(FadeImageIn(retryBG, 47));
    }

    IEnumerator RestartLevel(SpriteRenderer myImage)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myImage.color;
        myImage.enabled = true;
        while (timer <= totalTime)
        {
            myImage.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 0), new Color(startingColor.r, startingColor.g, startingColor.b, 1), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    IEnumerator FadeImageOut(SpriteRenderer myImage, float totalTime)
    {
        float timer = 0;
        Color startingColor = myImage.color;
        myImage.enabled = true;
        while (timer <= totalTime)
        {
            myImage.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 1), new Color(startingColor.r, startingColor.g, startingColor.b, 0), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
        myImage.enabled = false;
    }

    IEnumerator FadeImageIn(SpriteRenderer myImage, float totalTime)
    {
        float timer = 0;
        Color startingColor = myImage.color;
        myImage.enabled = true;
        while (timer <= totalTime)
        {
            myImage.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 0), new Color(startingColor.r, startingColor.g, startingColor.b, 1), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }

    IEnumerator FadeTextOut(TextMeshPro myTtext)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myTtext.color;
        while (timer <= totalTime)
        {
            myTtext.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 1), new Color(startingColor.r, startingColor.g, startingColor.b, 0), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }

    IEnumerator FadeTextIn(TextMeshPro myTtext)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myTtext.color;
        while (timer <= totalTime)
        {
            myTtext.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 0), new Color(startingColor.r, startingColor.g, startingColor.b, 1), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }
}
