using UnityEngine;
using System.Collections.Generic;

public class HexSlot : MonoBehaviour
{
    public static List<HexSlot> slots = new List<HexSlot>();

    public bool IsEmpty = true;

    private void Awake()
    {
        slots.Add(this);
    }

    public void Occupy(DragHex tile)
    {
        IsEmpty = false;
    }

    public static HexSlot FindNearest(Vector3 position)
    {
        float minDist = Mathf.Infinity;
        HexSlot nearest = null;

        foreach (var slot in slots)
        {
            float dist = Vector3.Distance(position, slot.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = slot;
            }
        }

        return (minDist < 1.2f) ? nearest : null; // snap limit
    }
}
