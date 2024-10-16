using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisorientLimb : Limb
{
    [SerializeField] private float _disorientLength;
    private Player _hitPlayer = null;
    private PlayerInputHandler _playerInputHandler = null;
    private ParticleManager _particleManager;

    protected override void Initialize()
    {
        base.Initialize();
        _particleManager = ServiceLocator.Get<ParticleManager>();
    }

    private void Update()
    {
        if (_hitPlayer != null)
        {
            _disorientLength -= Time.deltaTime;

            if (_disorientLength <= 0.0f)
            {
                _hitPlayer.MakeAimNormal();
                _playerInputHandler.MakeAimNormal();
                ServiceLocator.Get<LimbManager>().RemoveLimb(this);
                ServiceLocator.Get<ParticleManager>().PlayRedExplosionParticle(transform.position);
                Destroy(gameObject);
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (State != LimbState.Throwing)
        {
            return;
        }
        if (collision.gameObject.tag != "Player")
        {
            return;
        }
        else if (collision.gameObject.GetComponent<Player>() == _attachedPlayer)
        {
            return;
        }
        else
        {
            _hitPlayer = collision.gameObject.GetComponent<Player>();
            _playerInputHandler = collision.gameObject.GetComponent<PlayerInputHandler>();
            _hitPlayer.MakeAimOpposite();
            _playerInputHandler.MakeAimOpposite();
            _particleManager.PlayDrunkLiquidBubbleBurstParticle(gameObject.transform.position);

            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            _sprite.color = new Color(0, 0, 0, 0);
            transform.position = new Vector2(-100, -100);
        }
    }

    private void OnDestroy()
    {
        if(_hitPlayer != null)
        {
            _hitPlayer.MakeAimNormal();
            _playerInputHandler.MakeAimNormal();
        }
    }
}
