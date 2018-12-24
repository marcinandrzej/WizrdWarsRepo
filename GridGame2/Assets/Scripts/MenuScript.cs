using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject selectionPanel;
    public GameObject mainPanel;

    public Sprite[] avatars;
    public Image player1Avatar;
    public Image player2Avatar;

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnePlayer()
    {
        DataScript.instance.PlayersNumber = 1;
        mainPanel.SetActive(false);
        selectionPanel.SetActive(true);
    }

    public void TwoPlayers()
    {
        DataScript.instance.PlayersNumber = 2;
        mainPanel.SetActive(false);
        selectionPanel.SetActive(true);
    }

    public void Settings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
        selectionPanel.SetActive(false);
    }

    public void Next(bool player1)
    {
        if (player1)
        {
            int temp = DataScript.instance.Player1Model;
            temp++;
            if (temp >= avatars.Length)
            {
                temp = 0;
            }
            player1Avatar.sprite = avatars[temp];
            DataScript.instance.Player1Model = temp;
        }
        else
        {
            int temp = DataScript.instance.Player2Model;
            temp++;
            if (temp >= avatars.Length)
            {
                temp = 0;
            }
            player2Avatar.sprite = avatars[temp];
            DataScript.instance.Player2Model = temp;
        }
    }

    public void Previous(bool player1)
    {
        if (player1)
        {
            int temp = DataScript.instance.Player1Model;
            temp--;
            if (temp < 0)
            {
                temp = (avatars.Length - 1);
            }
            player1Avatar.sprite = avatars[temp];
            DataScript.instance.Player1Model = temp;
        }
        else
        {
            int temp = DataScript.instance.Player2Model;
            temp--;
            if (temp < 0)
            {
                temp = (avatars.Length - 1);
            }
            player2Avatar.sprite = avatars[temp];
            DataScript.instance.Player2Model = temp;
        }
    }
}
