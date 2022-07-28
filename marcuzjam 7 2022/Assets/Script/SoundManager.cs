using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour{ 



    [SerializeField] private AudioSource _musicSoruce, _effectsSource; 
    public static SoundManager Instance;



    private void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }


    public void PlaySound(AudioClip clip){
        _effectsSource.PlayOneShot(clip);
    }

    public void ChangeMasterVolume(float value){
        AudioListener.volume = value;

    }


}
