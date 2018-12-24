using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerScript : MonoBehaviour
{
    private int cost;
    private int elementIndex;
    private int actionIndex;
    private int damage;
    private RuntimeAnimatorController animCon;

    public int Cost
    {
        get
        {
            return cost;
        }

        set
        {
            cost = value;
        }
    }

    public int ElementIndex
    {
        get
        {
            return elementIndex;
        }

        set
        {
            elementIndex = value;
        }
    }

    public int Damge
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetUpPower(int _cost, int _damge, int _elementIndex,
        int _actionIndex, RuntimeAnimatorController _animCon)
    {
        Cost = _cost;
        Damge = _damge;
        ElementIndex = _elementIndex;
        actionIndex = _actionIndex;
        animCon = _animCon;
    }

    public void Action(GameObject player, GameObject enemy, Vector3 spawnPoint, int currentPlayer, GameManagerScript gM)
    {
        switch (actionIndex)
        {
            case 0:
                CreateBullet(spawnPoint, currentPlayer);
                break;
            case 1:
                MakeShield(player, currentPlayer);
                break;
            case 2:
                SpecialPower(player, enemy, gM, currentPlayer);
                break;
            default:
                break;
        }
    }

    private void CreateBullet(Vector3 position, int currentPlayer)
    {
        GameObject bullet = new GameObject("Bullet");

        bullet.AddComponent<SpriteRenderer>();
        if (currentPlayer != 1)
        {
            bullet.GetComponent<SpriteRenderer>().flipX = true;
        }

        bullet.transform.position = position;
        bullet.transform.localScale = new Vector3(1, 1, 1);

        Animator powerAnim = bullet.AddComponent<Animator>();
        powerAnim.runtimeAnimatorController = animCon;

        bullet.AddComponent<BoxCollider2D>();

        switch (elementIndex)
        {
            case 0:
                powerAnim.SetTrigger("Fire");
                bullet.GetComponent<BoxCollider2D>().size = new Vector2(2, 1);
                break;
            case 1:
                powerAnim.SetTrigger("Earth");
                bullet.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
                break;
            case 2:
                powerAnim.SetTrigger("Water");
                bullet.GetComponent<BoxCollider2D>().size = new Vector2(2, 1);
                break;
            case 3:
                powerAnim.SetTrigger("Electric");
                bullet.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
                break;
            case 4:
                powerAnim.SetTrigger("Wind");
                bullet.GetComponent<BoxCollider2D>().size = new Vector2(4, 4);
                bullet.GetComponent<BoxCollider2D>().offset = new Vector2(0, -2);
                break;
            default:
                break;
        }      

        bullet.AddComponent<Rigidbody2D>();
        bullet.GetComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().mass = 1;
        bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(currentPlayer * 500, 0));

        bullet.AddComponent<BulletScript>();
        bullet.GetComponent<BulletScript>().SetBullet(elementIndex, damage);
    }

    private void MakeShield(GameObject player, int currentPlayer)
    {
        GameObject shield = new GameObject("Shield");

        shield.AddComponent<SpriteRenderer>();

        float x = -6.3f;
        if (currentPlayer != 1)
        {
            x = 6.3f;
        }
        shield.transform.position = new Vector3(x, -1.5f, 0);
        shield.transform.localScale = new Vector3(1, 1.5f, 1);

        Animator powerAnim = shield.AddComponent<Animator>();
        powerAnim.runtimeAnimatorController = animCon;

        powerAnim.SetTrigger("Shield");

        player.GetComponent<PlayerScript>().AddShield(elementIndex, 2 * damage);

        Destroy(shield, 1.5f);
    }

    private void SpecialPower(GameObject player, GameObject enemy, GameManagerScript gM, int currentPlayer)
    {
        switch (elementIndex)
        {
            case 0:
                player.GetComponent<PlayerScript>().Heal(3 * damage);
                break;
            case 1:
                PoisonBurnFreeze(enemy);
                break;
            case 2:
                PoisonBurnFreeze(enemy);
                break;
            case 3:
                int[] gain = new int[5];
                for (int i = 0; i < 5; i++)
                {
                    gain[i] = Mathf.Min(5, gM.GetPowerPoints(-currentPlayer)[i]);
                }
                for (int i = 0; i < 5; i++)
                {
                    gM.AddPowerPoints(i, gain[i], currentPlayer);
                    gM.AddPowerPoints(i, -gain[i], -currentPlayer);
                }
                break;
            case 4:
                int hpGain = enemy.GetComponent<PlayerScript>().CurrentHp;
                enemy.GetComponent<PlayerScript>().TakeDamage(2 * damage, elementIndex);
                hpGain -= enemy.GetComponent<PlayerScript>().CurrentHp;
                player.GetComponent<PlayerScript>().Heal(hpGain);
                break;
            default:
                break;
        }
    }

    private void PoisonBurnFreeze(GameObject enemy)
    {
        PlayerStateScript state = new PlayerStateScript();
        state.SetState(3, damage, elementIndex, enemy);
        enemy.GetComponent<PlayerScript>().States.Add(state);
    }
}
