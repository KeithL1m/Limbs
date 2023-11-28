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

    [HideInInspector]
    public Player AttachedPlayer { get; set; }
    [HideInInspector]
    public PlayerLimbs AttachedPlayerLimbs { get; set; }
    [HideInInspector]
    public Transform AnchorPoint { get; set; } = null;
    [HideInInspector]
    public Rigidbody2D LimbRB { get; set; } 

    [HideInInspector]
    public LimbType Type { get; set; } //this will help most with animations
    [HideInInspector]
    public LimbState State { get; set; }

    [SerializeField]
    private LimbData _limbData;
    [field: SerializeField]
    public GameObject Trail { get; set; }
    [field: SerializeField]
    public GameObject PickUpIndicator { get; set; }

    //limb properties
    public float Size { get; set; }
    private Vector2 _throwVelocity; //used when not aiming
    private float _throwSpeed; //used when aiming
    private float _damage;
    private float _specialDamage;
    private Vector3 _returnVelocity;

    private void Start()
    {
        var limbManager = ServiceLocator.Get<LimbManager>();
        limbManager.AddLimb(this);

        State = LimbState.PickUp;
        LimbRB = GetComponent<Rigidbody2D>();
        LimbRB.SetRotation(0);

        Trail.SetActive(false);

        float angle = _limbData._throwAngle * Mathf.Deg2Rad;

        Size = _limbData._limbSize;
        _throwVelocity.x = _limbData._throwSpeed * Mathf.Cos(angle);
        _throwSpeed = _limbData._throwSpeed;
        _throwVelocity.y = _limbData._throwSpeed * Mathf.Sin(angle);
        _damage = _limbData._damage;
        _specialDamage = _limbData._specialDamage;
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
        _throwVelocity.x *= -1;
        LimbRB.velocity = _returnVelocity;
    }


    public void LimbAttack()
    {
        //for if we ever do melee
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
            _returnVelocity = -LimbRB.velocity;
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
