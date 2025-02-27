using UnityEngine;
using System.Collections;

public class MicrophoneManager : MonoBehaviour
{
    public int activationThreshold = 50;
    public int sampleWindow = 128;
    [Range(1, 100)]
    public int scaledVolume;

    private AudioClip micClip;
    private string micName;
    public GameObject targetObject; // Ȱ��ȭ�� ������Ʈ
    public float deactivationTime = 20f; // ��Ȱ��ȭ������ �ð�

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
            Debug.LogWarning("����ũ ��ġ�� �����ϴ�.");
        }
    }

    void Update()
    {
        if (Microphone.IsRecording(micName))
        {
            float volume = GetMaxVolume();
            scaledVolume = ScaleVolume(volume);
    

            if (scaledVolume >= activationThreshold && !targetObject.activeSelf)
            {
                ActivateObjectAtMousePosition();
            }
        }
    }

    void ActivateObjectAtMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));
        worldPosition.z = 0f;

        targetObject.transform.position = worldPosition;
        targetObject.SetActive(true);
        Debug.Log("������Ʈ Ȱ��ȭ��! ��ġ: " + worldPosition);
        StartCoroutine(DeactivateAfterTime());
    }

    IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(deactivationTime);
        targetObject.SetActive(false);
        Debug.Log("������Ʈ ��Ȱ��ȭ�� (20�� ���)");
    }

    float GetMaxVolume()
    {
        if (micClip == null) return 0;

        float[] samples = new float[sampleWindow];
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

    int ScaleVolume(float volume)
    {
        float scaledVolume = Mathf.Log10(1 + volume * 9) * 100;
        return Mathf.RoundToInt(Mathf.Clamp(scaledVolume, 1, 100));
    }
}
