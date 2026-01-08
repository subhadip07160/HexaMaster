using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HexSlot : MonoBehaviour
{
    // axial coordinates
    public int q;
    public int r;

    // piece placed on this slot
    public HexPiece currentPiece;

    // blocked state
    public bool isBlocked = false;

    // 🔒 read-only empty check
    public bool IsEmpty => currentPiece == null && !isBlocked;

    // neighbours (PRIVATE)
    private List<HexSlot> neighbours = new List<HexSlot>();

    // ================= NEIGHBOURS =================
    public void AddNeighbor(HexSlot slot)
    {
        if (!neighbours.Contains(slot))
            neighbours.Add(slot);
    }

    public List<HexSlot> GetNeighbors()
    {
        return neighbours;
    }

    // ================= SLOT CONTROL =================
    public void SetPiece(HexPiece piece)
    {
        currentPiece = piece;
        piece.transform.position = transform.position;
    }

    public void Clear()
    {
        currentPiece = null;
    }

    public void SetBlocked(bool value)
    {
        isBlocked = value;
    }
}
