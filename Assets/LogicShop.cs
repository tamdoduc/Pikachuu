using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicShop : MonoBehaviour
{
    public Button x2Hint, x5Hint,x2Shuffle;

    public LogicMenu logicMenu;
    private void Awake()
    {
        x2Hint.onClick.AddListener(OnClickx2HintButton);
        x5Hint.onClick.AddListener(OnClickx5HintButton);
        x2Shuffle.onClick.AddListener(OnClickx2ShuffleButton);
    }

    private void OnClickx2ShuffleButton()
    {
        int coin = PlayerPrefs.GetInt(PlayerPrefsManager.Coin);
        int shuffle = PlayerPrefs.GetInt(PlayerPrefsManager.Shuffle);
        if (coin >= 100)
        {
            coin = coin - 100;
            shuffle += 2;
            PlayerPrefs.SetInt(PlayerPrefsManager.Hint, shuffle);
            PlayerPrefs.SetInt(PlayerPrefsManager.Coin, coin);
            logicMenu.coinText.text = coin.ToString();
            logicMenu.shuffleText.text = shuffle.ToString();
        }
    }
    private void OnClickx5HintButton()
    {
        int coin = PlayerPrefs.GetInt(PlayerPrefsManager.Coin);
        int hint = PlayerPrefs.GetInt(PlayerPrefsManager.Hint);
        if (coin >= 200)
        {
            coin = coin - 200;
            hint += 5;
            PlayerPrefs.SetInt(PlayerPrefsManager.Hint, hint);
            PlayerPrefs.SetInt(PlayerPrefsManager.Coin, coin);
            logicMenu.coinText.text = coin.ToString();
            logicMenu.hintText.text = hint.ToString();
        }
    }
    private void OnClickx2HintButton()
    {
        int coin = PlayerPrefs.GetInt(PlayerPrefsManager.Coin);
        int hint = PlayerPrefs.GetInt(PlayerPrefsManager.Hint);
        if (coin >= 100)
        {
            coin = coin -100;
            hint += 5;
            PlayerPrefs.SetInt(PlayerPrefsManager.Hint, hint);
            PlayerPrefs.SetInt(PlayerPrefsManager.Coin, coin);
            logicMenu.coinText.text = coin.ToString();
            logicMenu.hintText.text = hint.ToString();
        }
    }
}
