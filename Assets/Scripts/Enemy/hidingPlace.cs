using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class hidingPlace : MonoBehaviour
{
    [SerializeField] GameObject hideText, stopHideText; // UI prompts
    [SerializeField] GameObject normalPlayer, hidingPlayer; // Player states
    [SerializeField] EnemyAI monsterScript; // Reference to enemy AI
    [SerializeField] Transform monsterTransform; // Enemy position
    [SerializeField] float loseDistance; // Distance to lose enemy while hiding
    [SerializeField] roomDetector detector; // Checks if player is in a hiding spot
    [SerializeField] TextMeshProUGUI hideTextMesh, stopHideTextMesh; // Text components
    [SerializeField] AudioSource door; // Door sound effect

    bool interactable = false, hiding = false;

    void Start()
    {
        // Initialize text elements
        hideTextMesh = hideText.GetComponent<TextMeshProUGUI>();
        stopHideTextMesh = stopHideText.GetComponent<TextMeshProUGUI>();

        // Prevent lag spikes by enabling then disabling text
        hideTextMesh.enabled = true;
        stopHideTextMesh.enabled = true;
        hideTextMesh.enabled = false;
        stopHideTextMesh.enabled = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Show or hide the "Press E to Hide" prompt based on detector
            hideTextMesh.enabled = detector.inTrigger;
            interactable = detector.inTrigger;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hideTextMesh.enabled = false;
            interactable = false;
        }
    }

    void Update()
    {
        if (interactable && Input.GetKeyDown(KeyCode.E))
        {
            Hide();
        }
        else if (hiding && Input.GetKeyDown(KeyCode.Q))
        {
            Unhide();
        }
    }

    void Hide()
    {
        hideTextMesh.enabled = false;
        door.Play();
        hidingPlayer.SetActive(true);
        normalPlayer.SetActive(false);
        hiding = true;
        interactable = false;

        // Stop the enemy chase if far enough
        if (Vector3.Distance(monsterTransform.position, normalPlayer.transform.position) > loseDistance && monsterScript.isHunting)
        {
            monsterScript.StopChase();
        }

        stopHideTextMesh.enabled = true;
    }

    void Unhide()
    {
        stopHideTextMesh.enabled = false;
        door.Play();
        normalPlayer.SetActive(true);
        hidingPlayer.SetActive(false);
        hiding = false;
    }
}
