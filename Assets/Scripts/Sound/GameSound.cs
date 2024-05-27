using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameSound : MonoBehaviour
{
    public Slider backgroundMusicSlider;
    public Slider effectSoundSlider;
    public AudioMixer masterMixer;
    private AudioManager audioMgr;
    public AudioManager audioManager{
        get
        {
            if (audioMgr == null)
                audioMgr = AudioManager.Instance;
            if (audioMgr.masterMixer == null)
                audioMgr.masterMixer = masterMixer;
            return audioMgr;
        }
    }

    public float MusicVolume
    {
        get { return audioManager.MusicVolume; }
        set { audioManager.MusicVolume = value; }
    }

    public float EffectsVolume
    {
        get { return audioManager.EffectsVolume; }
        set { audioManager.EffectsVolume = value; }
    }

    private AudioSource bgmAudioPlayer;
    public List<AudioClip> audioClips;

    private void Awake()
    {
        bgmAudioPlayer = GetComponent<AudioSource>();
    }

    private void Start()
    {
        var audioClipIndex = (Variables.stageId - 1) / 2;
        audioClipIndex = Mathf.Clamp(audioClipIndex, 0, audioClips.Count);
        bgmAudioPlayer.PlayOneShot(audioClips[audioClipIndex]);

        backgroundMusicSlider.value = MusicVolume;
        effectSoundSlider.value = EffectsVolume;
    }
}
