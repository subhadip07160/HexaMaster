using System.Collections.Generic;
using UnityEngine;

public class HexSlot : MonoBehaviour
{
    public int q, r; // grid coordinates
    public bool occupied = false;

    public List<HexSlot> neighbors = new List<HexSlot>();

    // highlight visuals
    private Renderer rend;
    private Color defaultColor;
    public Color highlightColor = Color.yellow;

    void Awake()
    {
        rend = GetComponentInChildren<Renderer>();

        if (rend != null)
            defaultColor = rend.material.color;
    }

    public void AddNeighbor(HexSlot s)
    {
        if (!neighbors.Contains(s))
            neighbors.Add(s);
    }

    public void ShowHighlight(bool status)
    {
        if (rend == null) return;

        if (status)
            rend.material.color = highlightColor;
        else
            rend.material.color = defaultColor;
    }
}
