using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private int elemIndex;
    private int damage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetBullet(int _elemIndex, int _damage)
    {
        damage = _damage;
        elemIndex = _elemIndex;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<PlayerScript>().TakeDamage(damage, elemIndex);
        Destroy(gameObject);
    }
}
