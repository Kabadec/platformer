using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioUtils
{
    public const string SfxSourceTag = "SfxAudioSource";

    public static AudioSource FindSfxSource()
    {
        return GameObject.FindWithTag(SfxSourceTag).GetComponent<AudioSource>();
    }
}
