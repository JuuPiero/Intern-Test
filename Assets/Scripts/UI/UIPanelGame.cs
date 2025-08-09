using System;
using System.Collections;
using System.Collections.Generic;
using NewGameplay;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelGame : MonoBehaviour,IMenu
{
    public Text LevelConditionView;

    [SerializeField] private Button btnPause;

    private UIMainManager m_mngr;

    private void Awake()
    {
        btnPause.onClick.AddListener(OnClickPause);
    }

    void Start()
    {
        if (!GameController.Instance.isTimeAttackMode)
        {
            LevelConditionView.gameObject.SetActive(false);
        }
        else
        {
            LevelConditionView.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        
        LevelConditionView.text = ((int)GameController.Instance.timeRemaining).ToString();
    }


    private void OnClickPause()
    {
        m_mngr.ShowPauseMenu();
    }

    public void Setup(UIMainManager mngr)
    {
        m_mngr = mngr;
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
