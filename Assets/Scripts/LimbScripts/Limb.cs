using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
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

    protected Player _attachedPlayer;
    protected PlayerLimbs _attachedPlayerLimbs;
    [HideInInspector] public Rigidbody2D LimbRB { get; private set; } = null;

    [SerializeField] protected SpriteRenderer _sprite;
    public Sprite Sprite { get; private set; }

    [HideInInspector] public LimbType Type { get; set; } //this will help most with animations
    public LimbState State; //{ get; set; }

    [SerializeField] private LimbData _limbData;
    [field: SerializeField] public GameObject Trail { get; set; }
    [field: SerializeField] public GameObject PickUpIndicator { get; set; }

    [HideInInspector] public bool CanPickUp { get; set; }
    [field: SerializeField] public float PickupTimer { get; set; }


    //limb properties
    public float Size { get; set; }
    protected Vector2 _throwVelocity; //used when not aiming
    protected float _throwSpeed; //used when aiming
    private float _damage;
    private float _specialDamage;
    protected Vector3 _returnVelocity;
    protected float _rVMultiplier;
    protected float _knockbackAmt;
    protected float _weight;

    public bool Clashing { get; private set; }


    [HideInInspector] public bool TripleShot = false;
    [HideInInspector] public bool _bombLimb = false;
    private bool _initialized = false;
    
    public bool IsSpecial = false;

    protected virtual void Awake()
    {
        GameLoader loader = ServiceLocator.Get<GameLoader>();
        loader.CallOnComplete(Initialize);
    }

    protected virtual void Initialize()
    {
        ServiceLocator.Get<LimbManager>().AddLimb(this);
        State = LimbState.PickUp;
        LimbRB = GetComponent<Rigidbody2D>();
        LimbRB.SetRotation(0);

        Trail.SetActive(false);

        Sprite = _sprite.sprite;

        float angle = _limbData._throwAngle * Mathf.Deg2Rad;

        Size = GetComponent<CapsuleCollider2D>().bounds.size.y;
        _throwVelocity.x = _limbData._throwSpeed * Mathf.Cos(angle);
        _throwSpeed = _limbData._throwSpeed;
        _throwVelocity.y = _limbData._throwSpeed * Mathf.Sin(angle);
        _damage = _limbData._damage;
        _specialDamage = _limbData._specialDamage;
        _rVMultiplier = _limbData._returnVelocityMultiplier;
        _knockbackAmt = _limbData._knockback;
        _weight = _limbData._weight;

        PickupTimer = 0.3f;
        CanPickUp = true;

        _bombLimb = false;
        _initialized = true;
    }

    public virtual void ThrowLimb(int direction)
    {
        PickupTimer = 0.3f;
        CanPickUp = false;
        _attachedPlayerLimbs.MoveBodyDown();
        LimbRB.simulated = true;
        transform.SetParent(ServiceLocator.Get<EmptyDestructibleObject>().transform);

        State = LimbState.Throwing;

        Trail.SetActive(true);

        if (_attachedPlayer._inputHandler.Aim.x == 0.0f && _attachedPlayer._inputHandler.Aim.y == 0.0f && !_attachedPlayer._inputHandler.FlickAiming)
        {
            _throwVelocity.x = Mathf.Abs(_throwVelocity.x);
            _throwVelocity.x *= direction;
            LimbRB.velocity = _throwVelocity;
        }
        else if (_attachedPlayer._inputHandler.FlickAiming)
        {
            Vector2 tVelocity = _attachedPlayer.LastAimed;
            tVelocity *= _throwSpeed;
            LimbRB.velocity = tVelocity;
        }
        else
        {
            Vector2 tVelocity = _attachedPlayer._inputHandler.Aim;
            tVelocity *= _throwSpeed;
            LimbRB.velocity = tVelocity;
        }

        _returnVelocity = new Vector3(-LimbRB.velocity.x * _rVMultiplier * 0.6f, -LimbRB.velocity.y * _rVMultiplier * 0.6f, 0f);
    }

    public void LimbUpdate()
    {
        if (!_initialized)
        {
            return;
        }

        if (State == LimbState.Attached)
        {
            if (Trail != null)
            {
                Trail.SetActive(false);
            }
        }
        else if (State == LimbState.Throwing || State == LimbState.Returning)
        {
            if (LimbRB.velocity.magnitude < 4.0f && _bombLimb == false)
            {
                PickupTimer -= Time.deltaTime;
            }
            if (PickupTimer <= 0.0f)
            {
                Debug.Log("Entering pickup state");
                CanPickUp = true;
                EnterPickupState();
            }
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

    protected virtual void EnterPickupState()
    {
        FlipY(1);
        FlipX(1);
        Physics2D.IgnoreCollision(_attachedPlayer.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
        State = LimbState.PickUp;
        _attachedPlayer = null;
        _attachedPlayerLimbs = null;
        if (Trail != null)
        {
            Trail.SetActive(false);
        }
        if (PickUpIndicator != null)
        {
            PickUpIndicator.SetActive(true);
        }
    }
    public virtual void PickUpExtra(Player player) { }

    public void FlipY(int i)
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

    public void FlipX(int i)
    {
        if (i < 0)
        {
            _sprite.flipX = true;
        }
        else
        {
            _sprite.flipX = false;
        }
    }

    public virtual void Clear() { }


    public void SetMaterial(Material material)
    {
        _sprite.material = material;
    }

    // Limb damage
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (State != LimbState.Throwing)
        {
            return;
        }

        if (collision.gameObject.CompareTag("BreakWall"))
        {
            collision.gameObject.GetComponent<LimbInstantiateWall>().Damage();
            ContactPoint2D contactPoint = collision.GetContact(0);
            ServiceLocator.Get<ParticleManager>().PlayBreakableWallParticle(contactPoint.point);
            ReturnLimb();
            return;
        }
        if (collision.gameObject.CompareTag("Limb"))
        {
            Limb other = collision.gameObject.GetComponent<Limb>();
            if (other.State != LimbState.Throwing)
            {
                return;
            }
            if (other.Clashing == false && other._attachedPlayer != _attachedPlayer)
            {
                Clashing = true;
                ContactPoint2D contactPoint = collision.GetContact(0);
                ServiceLocator.Get<ParticleManager>().PlaySwordClashParticle(contactPoint.point);
            }
            return;
        }
        else if (collision.gameObject.tag != "Player")
        {
            return;
        }

        if (_weight > 0)
        {
            if (ServiceLocator.Get<CameraManager>() != null)
                ServiceLocator.Get<CameraManager>().StartScreenShake(_weight * 0.01f, 0.1f);
        }

        PlayerHealth _healthPlayer = collision.gameObject.GetComponent<PlayerHealth>();

        if (_healthPlayer.IsDead())
        {
            if (collision.gameObject.GetComponent<PlayerLimbs>().CanPickUpLimb(this))
            {
                _attachedPlayer = collision.gameObject.GetComponent<Player>();
                _attachedPlayerLimbs = collision.gameObject.GetComponent<PlayerLimbs>();
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
        else
        {
            _healthPlayer.AddDamage(_damage + _specialDamage);

            ReturnLimb();
        }
    }

    protected virtual void OnCollisionExi2D(Collision2D collision)
    {
        Clashing = false;
    }

    // Limb pickup
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        else if (State == LimbState.Attached)
            return;
        else if (State == LimbState.Returning && collision.gameObject.GetComponent<Player>() != _attachedPlayer)
            return;

        if (State == LimbState.Throwing)
        {
            _returnVelocity = new Vector3(-LimbRB.velocity.x * _rVMultiplier, -LimbRB.velocity.y * _rVMultiplier, 0f);
            return;
        }

        if (collision.gameObject.GetComponent<PlayerLimbs>().CanPickUpLimb(this))
        {
            PickUpIndicator.SetActive(false);
            _attachedPlayer = collision.gameObject.GetComponent<Player>();
            _attachedPlayerLimbs = collision.gameObject.GetComponent<PlayerLimbs>();
            if (Type == LimbType.Arm)
            {
                LimbRB.SetRotation(90);
            }
            if (Type == LimbType.Leg)
            {
                LimbRB.SetRotation(0);
            }
            PickUpExtra(_attachedPlayer);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        else if (State == LimbState.Attached)
            return;
        else if (State == LimbState.Returning && collision.gameObject.GetComponent<Player>() != _attachedPlayer)
            return;

        if (collision.gameObject.GetComponent<PlayerLimbs>().CanPickUpLimb(this))
        {
            PickUpIndicator.SetActive(false);
            _attachedPlayer = collision.gameObject.GetComponent<Player>();
            _attachedPlayerLimbs = collision.gameObject.GetComponent<PlayerLimbs>();
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
