using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

using UnityEngine.Rendering;

public class MapController : MonoBehaviour
{
    [SerializeField]GameObject mazeMap;  // Assign the UI Image in Inspector
    [SerializeField] Transform player;


    AudioSource mapSound;

    private void Start()
    {
        mapSound = GetComponent<AudioSource>();// Get the audiosource 
    }

    void Update()
    {
        //toggles map on and off
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool isMapActive = !mazeMap.activeSelf;
            mapSound.Play();
            mazeMap.SetActive(isMapActive);

            // Enable/Disable Player Movement
           player.GetComponent<MovePlayer>().enabled = !isMapActive;

          
        }
    }
}
