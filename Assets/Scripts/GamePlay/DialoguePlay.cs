using UnityEngine;

public class DialoguePlay : MonoBehaviour
{
    [SerializeField] private AudioSource dialogue;
    [SerializeField] private GameObject Dialogue;
    private static bool hasPlayed = false; // Static variable to track dialogue play status

    private void Start()
    {
        if (hasPlayed) // If dialogue has already played in this session
        {
            Destroy(gameObject); // Destroy if already played
            return;
        }

        Dialogue.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogue.Play();
            Dialogue.SetActive(true);
            hasPlayed = true; // Set static variable so it doesn't play again in this session
            this.transform.position = new Vector3(999, 999, 999);
            Destroy(gameObject, dialogue.clip.length); // Destroy after the audio clip finishes
        }
    }
}
