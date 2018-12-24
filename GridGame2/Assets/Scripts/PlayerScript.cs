using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerScript : MonoBehaviour
{
    private Animator playerAnim;

    private int[] elementalShield;
    private int maxHp;
    private int currentHp;
    private bool isDead;

    private List<PlayerStateScript> states;

    private AudioClip[] playerSounds;
    private AudioSource audioPlayer;

    public bool IsDead
    {
        get
        {
            return isDead;
        }

        set
        {
            isDead = value;
        }
    }

    public int[] ElementalShield
    {
        get
        {
            return elementalShield;
        }

        set
        {
            elementalShield = value;
        }
    }

    public int CurrentHp
    {
        get
        {
            return currentHp;
        }

        set
        {
            currentHp = value;
        }
    }

    public List<PlayerStateScript> States
    {
        get
        {
            return states;
        }

        set
        {
            states = value;
        }
    }

    public int MaxHp
    {
        get
        {
            return maxHp;
        }

        set
        {
            maxHp = value;
        }
    }

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetUpPlayer(Vector3 position, Vector3 scale, RuntimeAnimatorController animCon, int elementsCount,
        bool reverse, AudioClip[] _playerSounds, AudioMixerGroup audioGroup)
    {
        IsDead = false;
        MaxHp = 100;
        CurrentHp = 100;

        gameObject.transform.position = position;
        gameObject.transform.localScale = scale;

        gameObject.AddComponent<SpriteRenderer>();
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
        if (reverse)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        playerAnim = gameObject.AddComponent<Animator>();
        playerAnim.runtimeAnimatorController = animCon;

        gameObject.AddComponent<BoxCollider2D>();
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 3.5f);
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

        gameObject.AddComponent<Rigidbody2D>();
        gameObject.GetComponent<Rigidbody2D>(). gravityScale = 0;
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;

        ElementalShield = new int[elementsCount];
        for (int i = 0; i < elementsCount; i++)
        {
            ElementalShield[i] = 0;
        }

        States = new List<PlayerStateScript>();

        playerSounds = _playerSounds;
        audioPlayer = gameObject.AddComponent<AudioSource>();
        audioPlayer.outputAudioMixerGroup = audioGroup;
}

    public void PlayAttack()
    {
        playerAnim.SetTrigger("Attack");
        audioPlayer.PlayOneShot(playerSounds[0]);
    }

    public void PlayHurt()
    {
        playerAnim.SetTrigger("Hurt");
        audioPlayer.PlayOneShot(playerSounds[1]);
    }

    public void PlayDie()
    {
        playerAnim.SetTrigger("Die");
        audioPlayer.PlayOneShot(playerSounds[2]);
    }

    public void AddShield(int index, int add)
    {
        ElementalShield[index] = Mathf.Max((ElementalShield[index] + add), 0);
    }

    public void TakeDamage(int damage, int elemIndex)
    {
        int dmg = damage;
        if (ElementalShield[elemIndex] >= damage)
        {
            ElementalShield[elemIndex] -= damage;
        }
        else
        {
            dmg = damage - ElementalShield[elemIndex];
            ElementalShield[elemIndex] = 0;
            CurrentHp = Mathf.Max((CurrentHp - dmg), 0);
        }

        if (CurrentHp <= 0)
        {
            PlayDie();
            IsDead = true;
        }
        else
        {
            PlayHurt();
        }
    }

    public void Heal(int amount)
    {
        currentHp = Mathf.Min(amount + currentHp, MaxHp);
    }
}
