using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeLimb : MonoBehaviour
{
    public Transform limb; 
    public Transform sprite;
    public Transform pickupIndicator;

    private float radius;
    private Vector2 capsuleScale;
    private Vector2 spriteScale;
    private Vector2 pickupScale;

    public void ResizeMyLimb()
    {
        radius = limb.GetComponent<CircleCollider2D>().radius * limb.transform.localScale.x;
        capsuleScale = new Vector2();
        capsuleScale.x = limb.GetComponent<CapsuleCollider2D>().size.x * limb.transform.localScale.x;
        capsuleScale.y = limb.GetComponent<CapsuleCollider2D>().size.y * limb.transform.localScale.y;

        spriteScale = new Vector2();
        spriteScale.x = sprite.localScale.x * limb.transform.localScale.x;
        spriteScale.y = sprite.localScale.y * limb.transform.localScale.y;

        pickupScale = new Vector2();
        pickupScale.x = pickupIndicator.localScale.x * limb.transform.localScale.x;
        pickupScale.y = pickupIndicator.localScale.y * limb.transform.localScale.y;

        limb.localScale = Vector3.one;
        limb.GetComponent<CircleCollider2D>().radius = radius;
        limb.GetComponent<CapsuleCollider2D>().size = capsuleScale;
        sprite.localScale = spriteScale;
        pickupIndicator.localScale = pickupScale;
    }
}
