using UnityEngine;

public class DragHexGroup : MonoBehaviour
{
    private Camera cam;
    private Vector3 offset;
    private bool dragging;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        dragging = true;
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        if (!dragging) return;
        transform.position = GetMouseWorldPos() + offset;
    }

    void OnMouseUp()
    {
        dragging = false;

        HexSlot slot = FindNearestEmptySlot();
        if (slot != null)
        {
            SnapToSlot(slot);
            HexGridGenerator grid =
      Object.FindFirstObjectByType<HexGridGenerator>();

          //  if (grid != null)
               // grid.CheckNeighbours(slot);

        }
    }

    Vector3 GetMouseWorldPos()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        plane.Raycast(ray, out float dist);
        return ray.GetPoint(dist);
    }

    HexSlot FindNearestEmptySlot()
    {
        HexSlot[] slots =
            FindObjectsByType<HexSlot>(FindObjectsSortMode.None);

        float minDist = float.MaxValue;
        HexSlot best = null;

        foreach (var s in slots)
        {
            if (!s.IsEmpty) continue;

            float d = Vector3.Distance(transform.position, s.transform.position);
            if (d < minDist)
            {
                minDist = d;
                best = s;
            }
        }
        return best;
    }

    void SnapToSlot(HexSlot slot)
    {
        transform.position = slot.transform.position;

        HexPiece p = GetComponentInChildren<HexPiece>();

        slot.currentPiece = p;   // 🔥 MUST
       // slot.IsEmpty = false;

        p.transform.SetParent(slot.transform);

        HexGridGenerator grid =
            Object.FindFirstObjectByType<HexGridGenerator>();

    }


}
