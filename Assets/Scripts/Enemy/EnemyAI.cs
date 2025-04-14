using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;


public class EnemyAI : MonoBehaviour
{
    // References to necessary components
    [SerializeField] private NavMeshAgent agent; // Handles AI movement
    [SerializeField] private List<Transform> waypoints; // List of patrol waypoints
    [SerializeField] private Animator animator; // Handles enemy animations
    [SerializeField] private AudioSource audioSource; // Audio source for enemy sounds
    [SerializeField] private AudioClip alertClip, huntClip, playerDeathClip; // Sound effects for different states
    [SerializeField] private AudioSource backgroundMusic; // Background music source

    // AI behavior settings
    [SerializeField] private float patrolSpeed, huntSpeed, minRestTime, maxRestTime;
    [SerializeField] private float viewRange, attackRange, minHuntTime, maxHuntTime;
    [SerializeField] private Vector3 rayOffset; // Offset for enemy's vision raycast
    [SerializeField] private string gameOverScene; // Scene to load on player death

    private Transform nextWaypoint; // Stores the next patrol destination
    private Vector3 moveTarget; // Stores current movement target
    private MovePlayer movePlayer; // Reference to player's movement script
    private LookMouse lookMouse; // Reference to player's look script
    private int randomIndex; // Index for selecting waypoints
    public bool isPatrolling = true, isHunting = false; // State management flags

    [SerializeField] private Transform target; // Player reference
    [SerializeField] private GameObject hideText, stopHideText; // UI elements for hiding mechanic
    private float aiDistance; // Distance between enemy and player

    void Start()
    {
        AssignRandomWaypoint();
        movePlayer = target.GetComponent<MovePlayer>(); // Get player movement script
        lookMouse = target.GetComponentInChildren<LookMouse>(); // Get look script
        isPatrolling = true;
    }

    void Update()
    {
        aiDistance = Vector3.Distance(target.position, transform.position);
        NavMeshPath path = new NavMeshPath();
      

        Vector3 toPlayer = (target.position - transform.position).normalized;
        RaycastHit visionCheck;

        // Check if the player is within enemy's view range
        if (Physics.Raycast(transform.position + rayOffset, toPlayer, out visionCheck, viewRange) && visionCheck.collider.CompareTag("Player"))
        {
            if (!isHunting)
            {
                isPatrolling = false; // Stop patrolling
                StopAllCoroutines(); // Stop any ongoing routines
                StartCoroutine(HuntRoutine()); // Start hunting behavior
                isHunting = true;
                PlayAlertSound(); // Play alert sound effect
            }
        }

        // If enemy is in hunting mode, chase the player
        if (isHunting)
        {
            agent.destination = target.position;
            agent.speed = huntSpeed;
            UpdateAnimation("sprint");

            // If within attack range, trigger attack sequence
            if (aiDistance <= attackRange)
            {
                if (target.TryGetComponent(out Collider playerCollider))
                {
                    playerCollider.enabled = false;
                    if (movePlayer) movePlayer.enabled = false;
                    if (lookMouse) lookMouse.enabled = false;
                }

                hideText.SetActive(false);
                stopHideText.SetActive(false);
                UpdateAnimation("jumpscare");
                StartCoroutine(GameOverRoutine());
                isHunting = false;
            }
        }
        // Patrol logic when not hunting
        else if (isPatrolling)
        {
            agent.destination = nextWaypoint.position;
            agent.speed = patrolSpeed;
            UpdateAnimation("walk");

            // If reached waypoint, stop and rest
            if (agent.remainingDistance <= agent.stoppingDistance && agent.velocity.magnitude < 0.1f)
            {
                UpdateAnimation("idle");
                agent.speed = 0;
                StopAllCoroutines();
                StartCoroutine(RestRoutine());
                isPatrolling = false;
            }
        }
    }

    // Updates animation state
    void UpdateAnimation(string state)
    {
        animator.ResetTrigger("walk");
        animator.ResetTrigger("idle");
        animator.ResetTrigger("sprint");
        animator.ResetTrigger("jumpscare");
        animator.SetTrigger(state);
    }

    // Assigns a new random waypoint for patrol
    void AssignRandomWaypoint()
    {
        if (waypoints.Count == 0)
        {
            
            return;
        }

        randomIndex = Random.Range(0, waypoints.Count);
        nextWaypoint = waypoints[randomIndex];
        
    }

    // Coroutine to pause enemy at a waypoint before continuing patrol
    IEnumerator RestRoutine()
    {
        yield return new WaitForSeconds(Random.Range(minRestTime, maxRestTime));
        isPatrolling = true;
        AssignRandomWaypoint();

    }

    // Coroutine to start hunting for a set time
    IEnumerator HuntRoutine()
    {
        PlayHuntMusic();
        yield return new WaitForSeconds(Random.Range(minHuntTime, maxHuntTime));
        StopChase();
    }

    // Stops hunting and resumes patrolling
    public void StopChase()
    {
        isPatrolling = true;
        isHunting = false;
        StopCoroutine("HuntRoutine");
        AssignRandomWaypoint();
        StartCoroutine(StopHuntMusicAfterDelay(2f));
    }

    // Coroutine to fade out hunt music after a delay
    IEnumerator StopHuntMusicAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopHuntMusic();
    }

    // Coroutine to handle player death sequence
    IEnumerator GameOverRoutine()
    {
        if (audioSource && playerDeathClip)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(playerDeathClip);
        }
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(gameOverScene);
    }

    // Plays an alert sound when enemy spots the player
    void PlayAlertSound()
    {
        if (audioSource && alertClip)
        {
            audioSource.PlayOneShot(alertClip);
            Invoke("PlayHuntMusic", alertClip.length); // Delay transition to hunt music
        }
    }

    // Plays hunting music when enemy is chasing
    void PlayHuntMusic()
    {
        if (audioSource && huntClip && (audioSource.clip != huntClip || !audioSource.isPlaying))
        {
            audioSource.clip = huntClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        if (backgroundMusic && backgroundMusic.isPlaying)
        {
            StartCoroutine(FadeOut(backgroundMusic, 0.5f));
        }
    }

    // Stops hunting music and resumes background music
    void StopHuntMusic()
    {
        if (audioSource && audioSource.isPlaying)
        {
            StartCoroutine(FadeOut(audioSource, 0.5f));
        }
        if (backgroundMusic && !backgroundMusic.isPlaying)
        {
            StartCoroutine(FadeIn(backgroundMusic, 0.5f));
        }
    }

    // Smoothly fades out an audio source over time
    IEnumerator FadeOut(AudioSource audio, float duration)
    {
        float startVolume = audio.volume;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            audio.volume = Mathf.Lerp(startVolume, 0, time / duration);
            yield return null;
        }
        audio.Stop();
        audio.volume = startVolume;
    }

    // Smoothly fades in an audio source over time
    IEnumerator FadeIn(AudioSource audio, float duration)
    {
        audio.Play();
        audio.volume = 0;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            audio.volume = Mathf.Lerp(0, 1, time / duration);
            yield return null;
        }
    }
}
