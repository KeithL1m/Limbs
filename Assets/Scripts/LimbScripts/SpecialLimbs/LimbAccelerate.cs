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

    public override void Clear()
    {
        if (isAdd)
        {
            currentPlayer.PlayerMovement.RemoveAccelerationLimb();
            currentPlayer = null;
            isAdd = false;
            transform.parent = null;
        }
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
