using System;
using System.Collections;
using System.Collections.Generic;
using DevDuck;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Color = System.Drawing.Color;

public class LogicMenu : MonoBehaviour
{ 
    public Button playButton;
    public Button quitButton;

    [Header("Top UI")] public Button shopButton;
    public Button inAppButton, coinsButton;
    public GameObject shopPanel, tabInAppPanel, tabCoinPanel;
    public Button closeShopButton;
    
    public TextMeshProUGUI coinText,shuffleText,hintText;
    
    [Header("Settings:")]
    public Sprite soundOn, soundOff,musicOn,musicOff;
    
    public Image soundIcon,musicIcon;
    
    public Button musicButton,soundButton,settingsButton;
    public GameObject settingsPanel;
    public List<GameObject> elementsOnSettings = new List<GameObject>();
    public Button closeSettingsButton;
    private void Awake()
    {
        playButton.onClick.AddListener(OnClickEasyButton);
        quitButton.onClick.AddListener(OnClickQuitButton);
        shopButton.onClick.AddListener(OnClickShopButton);
        inAppButton.onClick.AddListener(OnClickInAppButton);
        coinsButton.onClick.AddListener(OnClickCoinsButton);
        closeShopButton.onClick.AddListener(OnClickCloseShopButton);
        musicButton.onClick.AddListener(OnClickMusicButton);
        soundButton.onClick.AddListener(OnClickSoundButton);
        settingsButton.onClick.AddListener(OnClickSettingsButton);
        closeSettingsButton.onClick.AddListener(OnclickCloseSettingsButton);
    }

    private void OnclickCloseSettingsButton()
    {
       settingsPanel.SetActive(false);
    }

    private void OnClickSettingsButton()
    {
        settingsPanel.SetActive(true);
        for (int i = 0; i < elementsOnSettings.Count; i++)
        {
            var a = i;
            elementsOnSettings[i].transform.DOScale(1,0.2f).SetEase(Ease.OutBack).SetDelay(a*0.2f).From(0);
        }
    }

    private void OnClickSoundButton()
    {
        int sound = PlayerPrefs.GetInt("SOUND");
        if (sound == 0)
        {
            PlayerPrefs.SetInt("SOUND", 1);
            soundIcon.sprite = soundOff;
            AudioManager.instance.StopPlaySound();
        }
        else
        {
            PlayerPrefs.SetInt("SOUND", 0);
            soundIcon.sprite = soundOn;
            AudioManager.instance.ContinuePlaySound();;

        }
    }

    private void OnClickMusicButton()
    {
        int music = PlayerPrefs.GetInt("MUSIC");
        if (music == 0)
        {
            PlayerPrefs.SetInt("MUSIC", 1);
            musicIcon.sprite = musicOff;
            AudioManager.instance.StopPlayMusic();
        }
        else
        {
            PlayerPrefs.SetInt("MUSIC", 0);
            musicIcon.sprite = musicOn;
            AudioManager.instance.ContinuePlayMusic();

        }
    }

    private void Start()
    {
        GetData();
        AudioManager.instance.PlayBGMSound("BGM");
    }

    public void GetData()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsManager.isGetDefaultItems) == 0)
        {
            PlayerPrefs.SetInt(PlayerPrefsManager.Hint, 5);
            PlayerPrefs.SetInt(PlayerPrefsManager.Shuffle, 5);
            PlayerPrefs.SetInt(PlayerPrefsManager.isGetDefaultItems,1);
        }
       
        coinText.text = PlayerPrefs.GetInt(PlayerPrefsManager.Coin).ToString();
        hintText.text = PlayerPrefs.GetInt(PlayerPrefsManager.Hint).ToString();
        shuffleText.text = PlayerPrefs.GetInt(PlayerPrefsManager.Shuffle).ToString();
    }
    private void OnClickCloseShopButton()
    {
        shopPanel.gameObject.SetActive(false);
    }

    private void OnClickCoinsButton()
    {
        tabInAppPanel.SetActive(false);
        tabCoinPanel.SetActive(true);
        inAppButton.GetComponent<Image>().color = new Color32(183,183,183,255);
        coinsButton.GetComponent<Image>().color =new Color32(255,255,255,255);
    }

    private void OnClickInAppButton()
    {
        tabInAppPanel.SetActive(true);
        tabCoinPanel.SetActive(false);
        coinsButton.GetComponent<Image>().color = new Color32(183,183,183,255);
        inAppButton.GetComponent<Image>().color =new Color32(255,255,255,255);
    }

    private void OnClickShopButton()
    {
        shopPanel.gameObject.SetActive(true);
    }

    private void OnClickQuitButton()
    {
        Application.Quit();
    }

    private void OnClickHardButton()
    {
        throw new NotImplementedException();
    }

    private void OnClickNormalButton()
    {
        throw new NotImplementedException();
    }

    private void OnClickEasyButton()
    {
        ManagerScene.ins.LoadScene("GamePlay");
    }
}
