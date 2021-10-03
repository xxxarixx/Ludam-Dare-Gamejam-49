using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxCreator : MonoBehaviour
{
    [SerializeField] GameObject SFXPrefab;
    public static SfxCreator instance;
    public void PlaySound(AudioClip clip, float Volume = 1f, float PitchRandomized = .25f)
    {
        var spawnedSfx = Instantiate(SFXPrefab, transform.position, Quaternion.identity);
        var Audio = spawnedSfx.GetComponent<AudioSource>();
        float randomPitch = Random.Range(-PitchRandomized, PitchRandomized);
        Audio.pitch = 1f + randomPitch;
        Audio.volume = Volume;
        Audio.PlayOneShot(clip);
    }
    private void Awake()
    {
        instance = this;
    }
}
