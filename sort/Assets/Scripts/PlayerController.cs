using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    bool isGrounded;
    [SerializeField] GameObject shatteredObject;
    [SerializeField] ShapeLogic myShape;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground" && myShape.isActiveObject && !isGrounded)
        {
            isGrounded = true;

            GameManager.staticSoundManager.Play("Landing"); // hit ground sound
        }
        if (other.tag == "Shape" && isGrounded)
        {
            gameObject.GetComponentInParent<Animator>().enabled = false;
            shatteredObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
