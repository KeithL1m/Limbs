using UnityEngine;

public class ShotGunLimb : Limb
{
    [Header("Special Info")]
    [SerializeField, Range(0, 100)] float _forceAmt = 0;
    [SerializeField] private Animator _animation;
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioClip _specialPickUpSound;

    public override void ThrowLimb(int direction)
    {
        ServiceLocator.Get<CameraManager>()?.StartScreenShake(0.14f, 0.15f);
        _audioManager.PlaySound(_shootSound, transform.position, SoundType.SFX);
        _animation.SetBool("isBeingPicked", false);

        base.ThrowLimb(direction);

        float gravScale = _attachedPlayer.GetRBGravity();

        if (gravScale < 5f)
        {
            _attachedPlayer.GetComponent<Rigidbody2D>().AddForce(LimbRB.velocity.normalized * (-_forceAmt * 0.55f), ForceMode2D.Impulse);
        }
        else
        {
            _attachedPlayer.GetComponent<Rigidbody2D>().AddForce(LimbRB.velocity.normalized * (-_forceAmt), ForceMode2D.Impulse);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
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
            _animation.SetBool("isBeingPicked", true);
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
            _audioManager.PlaySound(_specialPickUpSound, transform.position, SoundType.SFX);
        }
    }
}
