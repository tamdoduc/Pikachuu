using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemWatchAdsCoin : MonoBehaviour
{
    [SerializeField] private int coinReceive;
    [SerializeField] private TextMeshProUGUI txtCoin;

    private void Start()
    {
        txtCoin.text = coinReceive.ToString();
    }
    public void OnClickItem()
    {
        AdMobRewardedAd.Instance.ShowRewardedAd(() =>
        {
        int coin = PlayerPrefs.GetInt(PlayerPrefsManager.Coin);
            coin += coinReceive;
            PlayerPrefs.SetInt(PlayerPrefsManager.Coin, coin);
            LogicMenu.Instance.coinText.text = coin.ToString();
        });
    }
}
