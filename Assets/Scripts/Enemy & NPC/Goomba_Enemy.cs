using GGJ2021;
using GGJ2021.NPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba_Enemy : NPC_Base, IContactDamager ,IMobileActor<Vector3> , IHittable<int>
{
    /*
     Interfaces
     */
    public override void hit()
    {

    }
    public override void hit(int attackValue)
    {

    }
    public override void contact()
    {

    }
    public override void aim()
    {

    }
    public override void fire() 
    { 
    }
     public override void walk(Vector3 pos1, Vector3 pos2)
    {
        this.GetComponent<Rigidbody2D>().AddRelativeForce(Vector3.Lerp(positions[0].transform.position, positions[1].transform.position, Mathf.PingPong(Time.time * moveSpeed, 1.0f))); // Lerp between points
    }
     public void fly(Vector3 pos1, Vector3 pos2)
    {
        
    }
     public void jump(float jumpForce)
    {

    }
    public void parabolicJump(Vector3 pos1, Vector3 pos2, Vector3 pos3)  //3 point arc
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
