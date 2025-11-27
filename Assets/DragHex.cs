using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragHex : MonoBehaviour
{
    Camera cam;
    Vector3 offset;
    Vector3 originalPos;
    bool dragging = false;

    public float snapRange = 1.5f;
    HexSlot highlightedSlot;
    HexSlot currentSlot; // slot where piece is placed

    static List<HexSlot> allSlots = new List<HexSlot>();

    void Start()
    {
        cam = Camera.main;

        // Cache all slots only once
        if (allSlots.Count == 0)
        {
            foreach (var obj in GameObject.FindGameObjectsWithTag("HexSlot"))
            {
                HexSlot slot = obj.GetComponent<HexSlot>();
                if (slot != null) allSlots.Add(slot);
            }
        }
    }

    void OnMouseDown()
    {
        dragging = true;
        originalPos = transform.position;

        // Unoccupy previous tile if placed on one
        if (currentSlot != null)
        {
            currentSlot.occupied = false;
            currentSlot = null;
        }

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        plane.Raycast(ray, out float hit);
        offset = transform.position - ray.GetPoint(hit);
    }

    void OnMouseDrag()
    {
        if (!dragging) return;

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float hit))
        {
            Vector3 pos = ray.GetPoint(hit) + offset;
            pos.y = originalPos.y;
            transform.position = pos;
        }

        UpdateHighlight();
    }

    void OnMouseUp()
    {
        dragging = false;

        // If found a valid free slot, snap to it
        if (highlightedSlot != null)
        {
            highlightedSlot.occupied = true;
            currentSlot = highlightedSlot;
            StartCoroutine(SmoothSnap(highlightedSlot.transform.position));
        }
        else
        {
            StartCoroutine(SmoothSnap(originalPos));
        }

        ClearHighlight();
    }

    IEnumerator SmoothSnap(Vector3 target)
    {
        Vector3 start = transform.position;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 6f;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }

    void UpdateHighlight()
    {
        HexSlot nearest = FindNearestSlot();

        if (nearest != highlightedSlot)
        {
            ClearHighlight();
            highlightedSlot = nearest;

            if (highlightedSlot != null)
                highlightedSlot.ShowHighlight(true);
        }
    }

    void ClearHighlight()
    {
        if (highlightedSlot != null)
            highlightedSlot.ShowHighlight(false);

        highlightedSlot = null;
    }

    HexSlot FindNearestSlot()
    {
        HexSlot nearest = null;
        float minDist = snapRange;

        foreach (var slot in allSlots)
        {
            float dist = Vector3.Distance(transform.position, slot.transform.position);

            // FIXED CONDITION:
            // allow snapping to CURRENT SLOT (not treated as occupied)
            if ((!slot.occupied || slot == currentSlot) && dist < minDist)
            {
                minDist = dist;
                nearest = slot;
            }
        }

        return nearest;
    }
}
