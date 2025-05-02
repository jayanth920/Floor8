using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerInputHandler : MonoBehaviour
{
    public PlayerNew1 player;
    public Transform cameraTransform;
    private AudioSource footstepAudio;
    public AudioClip footstepClip;

    private bool isMoving = false;
    private bool isRunning = false;
    private bool currentRunState = false;
    private Coroutine footstepCoroutine;

    void Start()
    {
        footstepAudio = gameObject.AddComponent<AudioSource>();
        footstepAudio.clip = footstepClip;
        footstepAudio.loop = false;
        footstepAudio.volume = 1f;
    }

    void Update()
    {
        Vector3 playerForward = player.transform.forward;
        playerForward.y = 0;
        Vector3 playerRight = player.transform.right;
        playerRight.y = 0;
        Vector3 finalMovement = Vector3.zero;

        bool inputW = Input.GetKey(KeyCode.W);
        bool inputA = Input.GetKey(KeyCode.A);
        bool inputS = Input.GetKey(KeyCode.S);
        bool inputD = Input.GetKey(KeyCode.D);

        bool hasInput = inputW || inputA || inputS || inputD;
        isRunning = Input.GetKey(KeyCode.LeftShift) && inputW;

        if (hasInput)
        {
            finalMovement += inputW ? playerForward : Vector3.zero;
            finalMovement += inputA ? -playerRight : Vector3.zero;
            finalMovement += inputS ? -playerForward : Vector3.zero;
            finalMovement += inputD ? playerRight : Vector3.zero;

            if (!isMoving || currentRunState != isRunning)
            {
                isMoving = true;
                currentRunState = isRunning;

                if (footstepCoroutine != null)
                    StopCoroutine(footstepCoroutine);

                footstepCoroutine = StartCoroutine(PlayFootsteps());
            }
        }
        else
        {
            isMoving = false;

            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
                footstepCoroutine = null;
            }
        }

        player.playerSpeed = isRunning ? 9f : 7f;

        finalMovement.Normalize();
        player.MoveWithCC(finalMovement);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private IEnumerator PlayFootsteps()
    {
        while (isMoving)
        {
            if (!footstepAudio.isPlaying)
            {
                footstepAudio.pitch = isRunning ? 1.3f : 1.0f;
                footstepAudio.Play();
            }

            float interval = isRunning ? 0.1f : 0.3f;
            yield return new WaitForSeconds(interval);
        }
    }
}
