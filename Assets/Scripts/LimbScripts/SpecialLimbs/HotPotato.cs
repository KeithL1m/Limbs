using UnityEngine;

public class HotPotato : Limb
{
    // counter
    [SerializeField] private int _explodeCounter;

    //explosion force
    Collider2D[] explosionRadius = null;
    private float _explosionForce = 600;
    private float _explosionRadius = 3;

    //Hot potato phases
    [SerializeField] Sprite _potatoSprite;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Initialize()
    {
        base.Initialize();

        _specialLimbs = true;
        _explodeCounter = 5;
        
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            _explodeCounter--;
            HotPotatoCheck();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (State != LimbState.Throwing)
            return;

    }

  

    private void HotPotatoCheck()
    {
        // Art and what happens each state of hot potato
        switch (_explodeCounter)
        {
            case 5:
                // potato sprite 5

                break;
            case 4:
                // potato sprite 4

                break;
            case 3:
                // potato sprite 3

                break;
            case 2:
                // potato sprite 2

                break;
            case 1:
                // potato sprite 1

                break;
            case 0:
                Explode();
                ServiceLocator.Get<LimbManager>().RemoveLimb(this);
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    private void Explode()
    {
        Debug.Log("POTATO BOOOOM");

        explosionRadius = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        if (ServiceLocator.Get<CameraManager>() != null)
            ServiceLocator.Get<CameraManager>().StartScreenShake(screenShakePower * screenShakePercent, screenShakeTime);
        foreach (Collider2D item in explosionRadius)
        {
            Rigidbody2D item_rigidbody = item.GetComponent<Rigidbody2D>();

            if (item_rigidbody != null)
            {
                Vector2 distanceVector = item.transform.position - transform.position;
                if (distanceVector.magnitude > 0)
                {
                    float explosion = _explosionForce / distanceVector.magnitude;
                    item_rigidbody.AddForce(distanceVector.normalized * explosion);

                    if (item.CompareTag("Player"))
                    {
                        item.GetComponent<PlayerHealth>().AddDamage(1000);
                    }
                }
            }
        }

    }
}
