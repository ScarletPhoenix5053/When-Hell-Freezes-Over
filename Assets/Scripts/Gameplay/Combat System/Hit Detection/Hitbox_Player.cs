using Sierra.Combat2D;
using Spine.Unity;

public class Hitbox_Player : Hitbox
{
    private SkeletonAnimation sk_an;
    private void Awake()
    {
        sk_an.state.Event += State_Event;
    }

    private void State_Event(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "Axe")
        {

        }
    }
}