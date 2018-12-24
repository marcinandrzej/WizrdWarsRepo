using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataScript : MonoBehaviour
{
    public static DataScript instance;

    private int player1Model;
    private int player2Model;
    private int playersNumber;

    public int Player1Model
    {
        get
        {
            return player1Model;
        }

        set
        {
            player1Model = value;
        }
    }

    public int Player2Model
    {
        get
        {
            return player2Model;
        }

        set
        {
            player2Model = value;
        }
    }

    public int PlayersNumber
    {
        get
        {
            return playersNumber;
        }

        set
        {
            playersNumber = value;
        }
    }

    void Start()
    {
        if (instance == null)
            instance = this;
        DontDestroyOnLoad(this);
    }
	
	// Update is called once per frame
	void Update ()
    {		
	}
}
