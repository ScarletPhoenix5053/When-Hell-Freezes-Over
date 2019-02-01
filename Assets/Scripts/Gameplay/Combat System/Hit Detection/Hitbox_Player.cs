using Sierra.Combat2D;
using Spine.Unity;
using UnityEngine;
using System;

public class Hitbox_Player : Hitbox
{
    public PosAndScale[] SizeOnAnims;

    private SkeletonAnimation sk_an;
    private void Awake()
    {
        sk_an = GetComponentInParent<SkeletonAnimation>();
    }
    private void Start()
    {
        sk_an.state.Event += State_Event;
    }
    
    private void State_Event(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        const int AnimCount = 2;
        if (SizeOnAnims.Length != AnimCount) throw new IndexOutOfRangeException();

        var eventName = e.Data.Name;
        if (eventName == "Axe1") SetPosAndSize(0);
        else if (eventName == "Axe2") SetPosAndSize(1);
    }

    public void SetPosAndSize(int index)
    {
        SetPos(SizeOnAnims[index].Pos);
        SetSize(SizeOnAnims[index].Size);
    }
    public void SetSize(Vector2 newSize)
    {
        Size = new Vector3(newSize.x, newSize.y, Size.z);
    }
    public void SetPos(Vector2 newScale)
    {
        transform.localPosition = new Vector3(newScale.x, newScale.y, Size.z);
    }
}
[Serializable]
public struct PosAndScale
{
    public Vector2 Pos;
    public Vector2 Size;
}