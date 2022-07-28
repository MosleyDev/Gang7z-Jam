using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField] private Image _img;
    [SerializeField] private Sprite _default, _pressed;
    [SerializeField] private AudioClip _compressClip;
    private void Start()
    {
        _img.sprite = _default;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _img.sprite = _pressed;
        SoundManager.Instance.PlaySound(_compressClip);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _img.sprite = _default;

    }


    public void GoToSettingsMenu(){

        SceneManager.LoadScene(1);

    }

    public void ResetScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void GoToChoiceChapters(){
        SceneManager.LoadScene(2);
    }

    public void ExitGame(){
        Application.Quit();
    }

    public void GoToMainMenu(){
        SceneManager.LoadScene(0);
    }


}
