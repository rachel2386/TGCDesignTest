using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public event Action PlayerWon;
    [SerializeField]private GameObject winText;
    [SerializeField] private GameObject missionSign;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        PlayerWon += ShowWinText;
        winText.SetActive(false);
        missionSign.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
    public void OnPlayerWin()
    {
        PlayerWon?.Invoke();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    void ShowWinText()
    {
        winText.SetActive(true);
        missionSign.SetActive(false);
    }
    
    
}
