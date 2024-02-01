using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class LimbInstantiateWall : MonoBehaviour
{
    [SerializeField] private float maximumHeight = 50f;
    [SerializeField] private float speed = 10f;
    private bool isStop;
    private Color initColor;
    [SerializeField] private Color hitedColor;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int hp = 5;
    public int HP { get { return hp; } }
    [SerializeField] private float colorDuration = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        damageCD -= Time.fixedDeltaTime;
        if (isHited)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, hitedColor, 0.2f);
        }
        else
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, initColor, 0.2f);
        }
        if (isStop)
            return;
        transform.localScale += new Vector3(0, speed * Time.fixedDeltaTime, 0);
        if (transform.localScale.y > maximumHeight)
        {
            transform.localScale = new Vector3(transform.localScale.x, maximumHeight, transform.localScale.z);
            isStop = true;
        }
        
    }
    private float damageCD = 0.1f;
    public void Damage()
    {
        if (damageCD > 0)
            return;
        hp--;
        damageCD = 0.1f;
        if (hp <= 0)
            Destroy(gameObject);
        ChangeHitedColor();
    }
    private bool isHited;
    private void ChangeHitedColor()
    {
        isHited = true;
        //spriteRenderer.color = hitedColor;
        StopCoroutine(ReturnSpriteInitColor());
        StartCoroutine(ReturnSpriteInitColor());
    }

    private IEnumerator ReturnSpriteInitColor()
    {
        yield return new WaitForSeconds(colorDuration);
        //spriteRenderer.color = initColor;
        isHited = false;
    }



}
