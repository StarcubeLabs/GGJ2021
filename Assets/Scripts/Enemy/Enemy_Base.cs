using System.Collections;
using System.Collections.Generic;
using UnityEngine;







public class Enemy_Base : MonoBehaviour
{
    public float health;
    public float damage;
    public float moveSpeed;
    public float jellyLevel;
    public float coolDown;
    public GameObject projectile;
    public List<GameObject> positions;
    public Collider2D hitbox;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = Vector3.Lerp(positions[0].transform.position,   
            positions[1].transform.position, Mathf.PingPong(Time.time * moveSpeed, 1.0f)); // Lerp between points

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))  //If the player collies with this object a cooldown happens
        {
            coolDown = 5;
        }
        else
        {
          coolDown--;
        }
    }

    public virtual void Move()
    {
       
    }

    public virtual void Attack()
    {

    }

    public virtual void TakeDamage()
    {

    }
}
