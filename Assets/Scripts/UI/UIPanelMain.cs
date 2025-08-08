using System;
using System.Collections;
using System.Collections.Generic;
using NewGameplay;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelMain : MonoBehaviour, IMenu
{
    [SerializeField] private Button btnTimer;

    [SerializeField] private Button btnMoves;

    [SerializeField] private Button btnAutoPlay;
    [SerializeField] private Button btnAutoLose;


    private UIMainManager m_mngr;

    private void Awake()
    {
        btnMoves.onClick.AddListener(OnClickMoves);
        btnTimer?.onClick.AddListener(OnClickTimer);


        btnAutoPlay.onClick.AddListener(() =>
        {

            OnClickMoves();
            GameController.Instance.AutoWin();
        });

        btnAutoLose.onClick.AddListener(() =>
        {
            OnClickMoves();
            GameController.Instance.AutoLose();
        });
    }

    private void OnDestroy()
    {
        if (btnMoves) btnMoves.onClick.RemoveAllListeners();
        if (btnTimer) btnTimer.onClick.RemoveAllListeners();
    }

    public void Setup(UIMainManager mngr)
    {
        m_mngr = mngr;
    }

    private void OnClickTimer()
    {
        m_mngr.LoadLevelTimer();
    }

    private void OnClickMoves()
    {
        // m_mngr.LoadLevelMoves();
        GameController gameController = FindFirstObjectByType<GameController>();
        gameController.LoadLevel();
        Hide();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
