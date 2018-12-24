using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioScript : MonoBehaviour
{
    public AudioMixer master;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider effectSlider;

    // Use this for initialization
    void Start ()
    {
        float f;
        master.GetFloat("Master", out f);
        masterSlider.value = f;

        master.GetFloat("Music", out f);
        musicSlider.value = f;

        master.GetFloat("Effects", out f);
        effectSlider.value = f;
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void UpdateMaster()
    {
        float volume = masterSlider.value;
        master.SetFloat("Master", volume);
    }

    public void UpdateMusic()
    {
        float volume = musicSlider.value;
        master.SetFloat("Music", volume);
    }

    public void UpdateEffect()
    {
        float volume = effectSlider.value;
        master.SetFloat("Effects", volume);
    }
}
