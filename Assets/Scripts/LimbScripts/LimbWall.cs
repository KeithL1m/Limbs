using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbWall : Limb
{
    [SerializeField] private float changeTime = 1f;
    [SerializeField] private GameObject wall;
   
    public override void ThrowLimb(int direction) 
    {
        base.ThrowLimb(direction);

        StartCoroutine(CreateWall());
    }

    public IEnumerator CreateWall() 
    {
        yield return new WaitForSeconds(changeTime);
        GameObject wall = Instantiate(this.wall, transform.position, transform.rotation);
        wall.transform.eulerAngles = new Vector3(0, 0, 0);
        ServiceLocator.Get<LimbManager>().RemoveList(this);
        Destroy(gameObject);
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
    }
}
