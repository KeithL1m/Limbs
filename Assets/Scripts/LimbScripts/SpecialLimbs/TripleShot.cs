using UnityEngine;

public class TripleShot : Limb
{
    [SerializeField] private GameObject _bullet;
    public int _shotsLeft = 3;

    [SerializeField] private Sprite _launcherTwo;
    [SerializeField] private Sprite _launcherOne;
    [SerializeField] private Sprite _launcherEmpty;
    [SerializeField] private Sprite _bullet1;
    [SerializeField] private Sprite _bullet2;
    [SerializeField] private Sprite _bullet3;
    private Sprite _currentBullet;

    protected override void Initialize()
    {
        base.Initialize();

        TripleShot = true;
        _currentBullet = _bullet1;
    }

    public override void ThrowLimb(int direction)
    {
        if (_shotsLeft > 0)
        {
            Vector2 throwDirection = new Vector2();

            if (_attachedPlayer._inputHandler.Aim.x == 0.0f && _attachedPlayer._inputHandler.Aim.y == 0.0f && !_attachedPlayer._inputHandler.FlickAiming)
            {
                throwDirection = new Vector2(direction, 0);
                
            }
            else if (_attachedPlayer._inputHandler.FlickAiming)
            {
                throwDirection = _attachedPlayer.LastAimed;
            }
            else
            {
                throwDirection = _attachedPlayer._inputHandler.Aim;
            }

            Bullet bullet = Instantiate(_bullet).GetComponent<Bullet>();
            bullet.Initialize(throwDirection, transform.position, _currentBullet);

            Physics2D.IgnoreCollision(_attachedPlayer.GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>(), true);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>(), true);

            _shotsLeft--;

            if (_shotsLeft == 2)
            {
                _sprite.sprite = _launcherTwo;
                _currentBullet = _bullet2;
            }
            else if (_shotsLeft == 1)
            {
                _sprite.sprite = _launcherOne;
                _currentBullet = _bullet3;
            }
            else
            {
                _sprite.sprite = _launcherEmpty;
            }


            return;
        }

        TripleShot = false;
        _attachedPlayerLimbs.MoveBodyDown();
        LimbRB.simulated = true;
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
}

