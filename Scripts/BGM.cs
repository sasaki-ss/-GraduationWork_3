using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioClip[] ac;
    private AudioSource aSrc;

    private void Start()
    {
        aSrc = GetComponent<AudioSource>();
        aSrc.clip = ac[0];
        aSrc.Play();
    }

    public void ChangeBossBGM()
    {
        aSrc.Stop();
        aSrc.clip = ac[1];
        aSrc.Play();
    }
}
