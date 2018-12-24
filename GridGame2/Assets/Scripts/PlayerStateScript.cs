using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateScript
{
    private int duration;
    private int damage;
    private int elemIndex;
    private GameObject player;

    public int Duration
    {
        get
        {
            return duration;
        }

        set
        {
            duration = value;
        }
    }

    public void SetState(int _duration, int _damage, int _elemIndex, GameObject _player)
    {
        Duration = _duration;
        damage = _damage;
        elemIndex = _elemIndex;
        player = _player;
    }

    public void Execute()
    {
        player.GetComponent<PlayerScript>().PlayHurt();
        player.GetComponent<PlayerScript>().TakeDamage(damage, elemIndex);
        Duration--;
    }

    public bool IsOver()
    {
        if (Duration <= 0)
        {
            return true;
        }
        return false;
    }

    public bool IsPoison()
    {
        if (elemIndex == 1)
            return true;
        return false;
    }
}
