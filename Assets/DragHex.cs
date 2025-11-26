using UnityEngine;

public class DragHex : MonoBehaviour
{
    private Camera cam;
    private bool dragging = false;
    private Vector3 offset;
    private Vector3 originalPos;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        dragging = true;
        originalPos = transform.position;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        plane.Raycast(ray, out float enter);
        offset = transform.position - ray.GetPoint(enter);
    }

    void OnMouseDrag()
    {
        if (!dragging) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out float enter))
        {
            transform.position = ray.GetPoint(enter) + offset;
        }
    }

    void OnMouseUp()
    {
        dragging = false;

        HexSlot nearest = HexSlot.FindNearest(transform.position);

        if (nearest != null && nearest.IsEmpty)
        {
            transform.position = nearest.transform.position;
            nearest.Occupy(this);
        }
        else
        {
            transform.position = originalPos;
        }
    }
}
