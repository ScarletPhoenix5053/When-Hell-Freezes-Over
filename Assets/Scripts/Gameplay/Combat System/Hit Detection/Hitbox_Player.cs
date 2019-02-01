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
        sk_an.state.Event += ChangeHitboxShapeOnEvent;
    }
    
    private void ChangeHitboxShapeOnEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        const int AnimCount = 10;
        if (SizeOnAnims.Length != AnimCount) throw new IndexOutOfRangeException();

        var eventName = e.Data.Name;

        // Greataxe
        if (eventName == "Axe1") SetPosAndSize(0);
        else if (eventName == "Axe2") SetPosAndSize(1);
        
        // Mace
        else if (eventName == "Mace1") SetPosAndSize(2);
        else if (eventName == "Mace2") SetPosAndSize(3);
        else if (eventName == "Mace3") SetPosAndSize(4);

        // Light Sword
        else if (eventName == "LSword1") SetPosAndSize(5);
        else if (eventName == "LSword2") SetPosAndSize(6);
        else if (eventName == "LSword3") SetPosAndSize(7);
        else if (eventName == "LSword4") SetPosAndSize(8);
        else if (eventName == "LSword5") SetPosAndSize(9);

        // GreatSword
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