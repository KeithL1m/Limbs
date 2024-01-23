using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaserLimb : Limb
{
    [Header("Tase Info")]
    [SerializeField] private float _taseTime = 0;

    private IEnumerator TaseTarget(Rigidbody2D rb2d)
    {
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(_taseTime);
        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag != "Player" || State == LimbState.Attached)
        {
            return;
        }

        else if (State == LimbState.Returning && collision.gameObject.GetComponent<Player>() != _attachedPlayer)
        {
            return;
        }

        if (State == LimbState.Throwing && collision.gameObject.GetComponent<Player>() != _attachedPlayer)
        {
            var playerWalking = collision.gameObject.GetComponent<Rigidbody2D>();
            StartCoroutine(TaseTarget(playerWalking));

            var playerLimbs = collision.gameObject.GetComponent<PlayerLimbs>();

            foreach(var limb in playerLimbs._limbs)
            {
                switch(playerLimbs._selectedLimb)
                {
                    case PlayerLimbs.SelectedLimb.LeftLeg:
                        {
                            playerLimbs.ThrowLimb(-1);
                        }
                        break;
                    case PlayerLimbs.SelectedLimb.RightLeg:
                        {
                            playerLimbs.ThrowLimb(1);
                        }
                        break;
                    case PlayerLimbs.SelectedLimb.LeftArm:
                        {
                            playerLimbs.ThrowLimb(-1);
                        }
                        break;
                    case PlayerLimbs.SelectedLimb.RightArm:
                        {
                            playerLimbs.ThrowLimb(1);
                        }
                        break;
                }
            }

            _returnVelocity = new Vector3(-LimbRB.velocity.x * _rVMultiplier, -LimbRB.velocity.y * _rVMultiplier, 0f);
            return;
        }

        if (collision.gameObject.GetComponent<PlayerLimbs>().CanPickUpLimb(this))
        {
            PickupTimer = 0.2f;
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
