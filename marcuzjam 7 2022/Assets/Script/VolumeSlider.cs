using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour{ 

    [SerializeField] private Slider _slider;

    private void Start() {
        _slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMasterVolume(val));
        _slider.onValueChanged.AddListener(val => SaveAudio(val));
        LoadAudio();
    }

    private void SaveAudio(float val){
        PlayerPrefs.SetFloat("audioVolume", val);

    }

    private void LoadAudio(){
        if(PlayerPrefs.HasKey("audioVolume")){

            AudioListener.volume = PlayerPrefs.GetFloat("audioVolume");
            _slider.value = PlayerPrefs.GetFloat("audioVolume");
        }
        else{
            PlayerPrefs.SetFloat("audioVolume", 0.5f);

            AudioListener.volume = PlayerPrefs.GetFloat("audioVolume");
            _slider.value = PlayerPrefs.GetFloat("audioVolume");
        }

    }

}
