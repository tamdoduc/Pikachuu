using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelUIGame : MonoBehaviour
{
    [SerializeField] PopupWin popupWin;
    /*[SerializeField] EasyButton btnShuffle, btnHint, btnReplay, btnHome;

    private void Awake()
    {
        ManagerEvent.RegEvent(EventCMD.EVENT_WIN, ShowPopupWin);

        btnShuffle.OnClick(() =>
        {
            ManagerEvent.RaiseEvent(EventCMD.EVENT_SHUFFLE);
        });

        btnHint.OnClick(() =>
        {
            ManagerEvent.RaiseEvent(EventCMD.EVENT_HINT);
        });

        btnReplay.OnClick(() =>
        {
            ManagerEvent.ClearEvent();
            SceneManager.LoadScene("ScenePikachu");
        });
    }*/

    private void ShowPopupWin(object e)
    {
        Debug.Log("you win");
        popupWin.gameObject.SetActive(true);
    }
}
