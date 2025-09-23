using System;
using UnityEngine;

public static class AlarmEvent
{
    public static Action<AudioClip, float> OnAlarmTriggered;

    public static void Trigger(AudioClip clip, float volume = 1f)
    {
        OnAlarmTriggered?.Invoke(clip, volume);
    }
}

