using UnityEngine;

public class MicroPhoneVolumeTest : MonoBehaviour
{
    private AudioClip micClip;
    private string micName;

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            micName = Microphone.devices[0];
            micClip = Microphone.Start(micName, true, 10, AudioSettings.outputSampleRate);
            Debug.Log("����ũ ���۵�: " + micName);
        }
        else
        {
            Debug.LogWarning("����ũ ����!");
        }
    }

    void Update()
    {
        if (Microphone.IsRecording(micName))
        {
            float volume = GetMaxVolume();
            Debug.Log("���� ����: " + volume);
        }

        int micPosition = Microphone.GetPosition(micName);
        Debug.Log("����ũ ���� ��ġ: " + micPosition);

        if (micPosition > 0)
        {
            float volume = GetMaxVolume();
            Debug.Log("���� ����: " + volume);
        }
    }

    float GetMaxVolume()
    {       if (micClip == null) return 0;

            float[] samples = new float[128];
            int micPosition = Microphone.GetPosition(micName);

            if (micPosition < samples.Length)
            {
                Debug.Log("����ũ �����Ͱ� ����� ������ ����");
                return 0;
            }

            micClip.GetData(samples, micPosition - samples.Length);

            float maxVolume = 0f;
            foreach (float sample in samples)
            {
                maxVolume = Mathf.Max(maxVolume, Mathf.Abs(sample));
            }

            return maxVolume;
        }

    }

