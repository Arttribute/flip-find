using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class MenuController : MonoBehaviour
{
    [SerializeField] private AudioMixer volumeMixer; // AudioMixer component to adjust the volume
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        float defaultVolume = -20f;
        volumeMixer.SetFloat("Volume", defaultVolume);


        volumeSlider.onValueChanged.RemoveAllListeners();

        //float sliderValue = Mathf.InverseLerp(-80f, 0f, defaultVolume);
        float sliderValue = Mathf.Pow(10f, defaultVolume / 20f);
        volumeSlider.value = sliderValue;

        volumeSlider.onValueChanged.AddListener(setVolume);


    }

    public void setVolume(float sliderValue)
    {
        float volume = Mathf.Log10(Mathf.Clamp(sliderValue, 0.001f, 1f)) * 20;

        volumeMixer.SetFloat("Volume", volume);
    }
}