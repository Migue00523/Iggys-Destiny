using UnityEngine;

public class MicrophoneDetector : MonoBehaviour
{
    
    public float noiseThreshold = 0.1f;

    private AudioClip microphoneClip;
    private string microphoneDevice;
    private bool microphoneStarted = false;

    public bool IsMakingNoise { get; private set; }

    void Start()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone detected!");
            return;
        }

        // Uses the first available microphone
        microphoneDevice = Microphone.devices[0];

        // Starts recording in a loop
        microphoneClip = Microphone.Start(
            microphoneDevice,
            true,
            1,
            44100
        );

        microphoneStarted = true;
    }

    void Update()
    {
        if (!microphoneStarted)
            return;

        float volume = GetMicrophoneVolume();

        IsMakingNoise = volume > noiseThreshold;

        if (IsMakingNoise)
        {
            Debug.Log("Noise detected! Volume: " + volume);
        }
    }

    float GetMicrophoneVolume()
    {
        int microphonePosition = Microphone.GetPosition(microphoneDevice);

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

    void OnDestroy()
    {
        if (Microphone.IsRecording(microphoneDevice))
        {
            Microphone.End(microphoneDevice);
        }
    }
}