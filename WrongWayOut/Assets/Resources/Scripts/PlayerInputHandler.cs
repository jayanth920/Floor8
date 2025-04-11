using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerInputHandler : MonoBehaviour
{
    public PlayerNew1 player;
    public Transform cameraTransform; // Reference to the FPS camera
    private AudioSource footstepAudio;  // Footstep sound source
    public AudioClip footstepClip;      // Assign this in the Inspector

    private bool isMoving = false;

    void Start()
    {
        footstepAudio = gameObject.AddComponent<AudioSource>();  // Create audio source if not added
        footstepAudio.clip = footstepClip;
        footstepAudio.loop = false;  // Play sound once per step
        footstepAudio.volume = 0.4f;

    }

    void Update()
    {
        Vector3 playerForward = player.transform.forward;
        playerForward.y = 0;

        Vector3 playerRight = player.transform.right;
        playerRight.y = 0;

        Vector3 finalMovement = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            finalMovement += (Input.GetKey(KeyCode.W) ? playerForward : Vector3.zero);
            finalMovement += (Input.GetKey(KeyCode.A) ? -playerRight : Vector3.zero);
            finalMovement += (Input.GetKey(KeyCode.S) ? -playerForward : Vector3.zero);
            finalMovement += (Input.GetKey(KeyCode.D) ? playerRight : Vector3.zero);

            if (!isMoving)
            {
                isMoving = true;
                StartCoroutine(PlayFootsteps());
            }
        }
        else
        {
            isMoving = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            player.playerSpeed = 9f;
        }
        else
        {
            player.playerSpeed = 7f;
        }

        finalMovement.Normalize();
        player.MoveWithCC(finalMovement);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private IEnumerator PlayFootsteps()
    {
        while (isMoving)
        {
            if (!footstepAudio.isPlaying)
            {
                footstepAudio.Play();
            }
            if (player.playerSpeed == 7f)
            {
                yield return new WaitForSeconds(0.1f);  // Adjust step interval

            }
            else if (player.playerSpeed == 9f)
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
