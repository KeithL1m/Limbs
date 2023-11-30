using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Limb : MonoBehaviour
{
    public enum LimbType
    {
        Arm,
        Leg
    }

    public enum LimbState
    {
        Attached,
        Throwing,
        Returning,
        PickUp
    }

    [HideInInspector] public Player AttachedPlayer { get; set; }
    [HideInInspector] public PlayerLimbs AttachedPlayerLimbs { get; set; }
    [HideInInspector] public Transform AnchorPoint { get; set; } = null;
    [HideInInspector] public Rigidbody2D LimbRB { get; set; }

    [SerializeField] private SpriteRenderer _sprite;

    [HideInInspector] public LimbType Type { get; set; } //this will help most with animations
    [HideInInspector] public LimbState State { get; set; }

    [SerializeField] protected LimbData _limbData;
    [field: SerializeField] public GameObject Trail { get; set; }
    [field: SerializeField]  public GameObject PickUpIndicator { get; set; }

    //limb properties
    public float Size { get; set; }
    protected Vector2 _throwVelocity; //used when not aiming
    protected float _throwSpeed; //used when aiming
    protected float _damage;
    protected float _specialDamage;
    protected Vector3 _returnVelocity;
    protected float _rVMultiplier;
    public bool _specialLimb = false;

    private void Start()
    {
        // rework to add limb managers to the service locator  

        /*var limbManager = ServiceLocator.Get<LimbManager>();
        limbManager.AddLimb(this);*/

        State = LimbState.PickUp;
        LimbRB = GetComponent<Rigidbody2D>();
        LimbRB.SetRotation(0);

        Trail.SetActive(false);

        float angle = _limbData._throwAngle * Mathf.Deg2Rad;

        Size = GetComponent<CapsuleCollider2D>().bounds.size.y;
        _throwVelocity.x = _limbData._throwSpeed * Mathf.Cos(angle);
        _throwSpeed = _limbData._throwSpeed;
        _throwVelocity.y = _limbData._throwSpeed * Mathf.Sin(angle);
        _damage = _limbData._damage;
        _specialDamage = _limbData._specialDamage;
        _rVMultiplier = _limbData._returnVelocityMultiplier;
    }

    public void ThrowLimb(int direction)
    {
        AttachedPlayerLimbs.MoveBodyDown();
        LimbRB.simulated = true;
        State = LimbState.Throwing;

        Trail.SetActive(true);

        if (AttachedPlayer._inputHandler.Aim.x == 0.0f && AttachedPlayer._inputHandler.Aim.y == 0.0f)
        {
            _throwVelocity.x = Mathf.Abs(_throwVelocity.x);
            _throwVelocity.x *= direction;
            LimbRB.velocity = _throwVelocity;
        }
        else
        {
            Vector2 tVelocity = AttachedPlayer._inputHandler.Aim;
            tVelocity *= _throwSpeed;
            LimbRB.velocity = tVelocity;
        }
    }

    private void ReturnLimb()
    {
        Trail.SetActive(true);
        State = LimbState.Returning;
        if (_returnVelocity.y < 0)
        {
            _returnVelocity.y *= -1;
        }
        LimbRB.velocity = _returnVelocity;
    }


    public void LimbAttack()
    {
        //for if we ever do melee
    }

    public void Flip(int i )
    {
        if (i < 0)
        {
            _sprite.flipY = true;
        }
        else
        {
            _sprite.flipY = false;
        }
    }

    // Limb damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        else if (State != LimbState.Throwing)
            return;

        PlayerHealth _healthPlayer = collision.gameObject.GetComponent<PlayerHealth>();
        _healthPlayer.AddDamage(_damage + _specialDamage);
        ReturnLimb();
    }

    // Limb pickup
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        else if (State == LimbState.Attached)
            return;
        else if (State == LimbState.Returning && collision.gameObject.GetComponent<Player>() != AttachedPlayer)
            return;

        if (State == LimbState.Throwing)
        {
            _returnVelocity = new Vector3(-LimbRB.velocity.x * _rVMultiplier, -LimbRB.velocity.y * _rVMultiplier, 0f);
            return;
        }

        if (collision.gameObject.GetComponent<PlayerLimbs>().CanPickUpLimb(this))
        {
            PickUpIndicator.SetActive(false);
            AttachedPlayer = collision.gameObject.GetComponent<Player>();
            AttachedPlayerLimbs = collision.gameObject.GetComponent<PlayerLimbs>();
            if (Type == LimbType.Arm)
            {

                LimbRB.SetRotation(90);
            }
            if (Type == LimbType.Leg)
            {
                LimbRB.SetRotation(0);
            }
        }
    }
}
