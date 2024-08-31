public class LimbFly : Limb
{
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public override void PickUpExtra(Player player)
    { 
        player.SetCanFly(true);
    }
    public override void ThrowLimb(int direction)
    { 
        base.ThrowLimb(direction);
        _attachedPlayer.SetCanFly(false);

    }
}
