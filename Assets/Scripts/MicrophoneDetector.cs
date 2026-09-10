using UnityEngine;
using UnityEngine.UI;

public class MicrophoneDetector : MonoBehaviour
{
    
    [SerializeField] private float noiseThreshold = 0.1f;

 
    [SerializeField] private Slider noiseSlider;

    [SerializeField] private float smoothSpeed = 10f;

    private AudioClip microphoneClip;
    private string microphoneDevice;
    private bool microphoneStarted = false;

    public bool IsMakingNoise { get; private set; }

    // Current noise level, between 0 and 1
    public float CurrentNoiseLevel { get; private set; }

    private void Start()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone detected!");
            return;
        }

        microphoneDevice = Microphone.devices[0];

        microphoneClip = Microphone.Start(
            microphoneDevice,
            true,
            1,
            44100
        );

        microphoneStarted = true;

        if (noiseSlider != null)
        {
            noiseSlider.minValue = 0f;
            noiseSlider.maxValue = 1f;
            noiseSlider.value = 0f;
        }
    }

    private void Update()
    {
        if (!microphoneStarted)
            return;

        float volume = GetMicrophoneVolume();

        // Convert the microphone volume into a more visible value
        float targetNoise = Mathf.Clamp01(volume * 10f);

        // Smooth the movement of the meter
        CurrentNoiseLevel = Mathf.Lerp(
            CurrentNoiseLevel,
            targetNoise,
            smoothSpeed * Time.deltaTime
        );

        // Update the UI
        if (noiseSlider != null)
        {
            noiseSlider.value = CurrentNoiseLevel;
        }

        // Detect noise
        IsMakingNoise = volume > noiseThreshold;

        if (IsMakingNoise)
        {
            Debug.Log("Noise detected! Volume: " + volume);
        }
    }

    private float GetMicrophoneVolume()
    {
        int microphonePosition = Microphone.GetPosition(
            microphoneDevice
        );

        if (microphonePosition < 128)
            return 0f;

        float[] samples = new float[128];

        microphoneClip.GetData(
            samples,
            microphonePosition - 128
        );

        float sum = 0f;

        for (int i = 0; i < samples.Length; i++)
        {
            sum += Mathf.Abs(samples[i]);
        }

        return sum / samples.Length;
    }

    private void OnDestroy()
    {
        if (!string.IsNullOrEmpty(microphoneDevice))
        {
            if (Microphone.IsRecording(microphoneDevice))
            {
                Microphone.End(microphoneDevice);
            }
        }
    }
}