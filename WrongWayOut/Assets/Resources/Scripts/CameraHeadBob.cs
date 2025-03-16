using UnityEngine;
using Unity.Cinemachine;

public class CameraHeadBob : MonoBehaviour
{
    [System.Obsolete]
    public CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin perlin;
    public float speedThreshold = 2f;  // Speed at which the noise starts increasing
    public float maxAmplitude = 1f;    // Maximum noise amplitude
    public float maxFrequency = 2f;    // Maximum noise frequency

    [System.Obsolete]
    private void Start()
    {
        // Get the Perlin Noise component attached to the Virtual Camera
        perlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        // Calculate the player's speed (or velocity)
        float playerSpeed = GetComponent<CharacterController>().velocity.magnitude;

        // Adjust the noise based on the player's speed
        if (playerSpeed > speedThreshold)
        {
            // Increase amplitude and frequency as speed increases
            float amplitude = Mathf.Lerp(0f, maxAmplitude, playerSpeed / 10f);  // Adjust divisor to control at what speed the effect kicks in
            float frequency = Mathf.Lerp(0f, maxFrequency, playerSpeed / 10f);  // Adjust divisor to control intensity of frequency

            // Apply the changes to the noise profile
            // perlin.m_AmplitudeGain = amplitude;
            // perlin.m_FrequencyGain = frequency;
        }
        else
        {
            // When the player is still, reset the noise effect
            // perlin.m_AmplitudeGain = 0f;
            // perlin.m_FrequencyGain = 0f;
        }
    }
}
