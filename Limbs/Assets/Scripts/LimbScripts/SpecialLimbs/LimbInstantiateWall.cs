using UnityEngine;

public class LimbInstantiateWall : MonoBehaviour
{
    [SerializeField] private float maximumHeight = 50f;
    [SerializeField] private float speed = 10f;
    private bool isStop;
    private Color initColor;
    [SerializeField] private Sprite[] hitedSprites;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int hp = 5;
    private int maxHP;
    public int HP { get { return hp; } }
    [SerializeField] private float colorDuration = 0.2f;
    private float damageCD = 0.1f;
    private bool isHited;

    // Start is called before the first frame update
    void Start()
    {
        maxHP = hp;
        initColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        damageCD -= Time.fixedDeltaTime;
        if (isStop)
            return;
        transform.localScale += new Vector3(0, speed * Time.fixedDeltaTime, 0);
        if (transform.localScale.y > maximumHeight)
        {
            transform.localScale = new Vector3(transform.localScale.x, maximumHeight, transform.localScale.z);
            isStop = true;
        }

    }

    public void Damage()
    {
        if (damageCD > 0)
            return;
        hp--;
        damageCD = 0.1f;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            spriteRenderer.sprite = hitedSprites[(maxHP - hp)];
        }
    }
}





