using UnityEngine;

public class LimbAccelerate : Limb
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //protected override void EnterPickupState() 
    //{
    //    FlipY(1);
    //    FlipX(1);
    //    PickUpExtra(_attachedPlayer);
    //    Physics2D.IgnoreCollision(_attachedPlayer.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
    //    State = LimbState.PickUp;
    //    _attachedPlayer = null;
    //    _attachedPlayerLimbs = null;
    //    if (Trail != null)
    //    {
    //        Trail.SetActive(false);
    //    }
    //    if (PickUpIndicator != null)
    //    {
    //        PickUpIndicator.SetActive(true);
    //    }
    //}


    private Player currentPlayer;
    private bool isAdd;
    public override void PickUpExtra(Player player)
    {
        base.PickUpExtra(player);
        if (isAdd)
            return;
        isAdd = true;
        currentPlayer= player;
        currentPlayer.PlayerMovement.AddAccelerationLimb();
    }


    public override void ThrowLimb(int direction)
    {
        base.ThrowLimb(direction);
        if (isAdd)
        {
            currentPlayer.PlayerMovement.RemoveAccelerationLimb();
            currentPlayer = null;
            isAdd = false;
            transform.parent = null;
        }
    }

}
