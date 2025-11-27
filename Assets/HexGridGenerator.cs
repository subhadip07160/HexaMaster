using UnityEngine;
using System.Collections.Generic;

public class HexGridGenerator : MonoBehaviour
{
    public GameObject hexSlotPrefab;
    public int width = 6;
    public int height = 5;
    public float hexSize = 1f;

    private float xOffset;
    private float zOffset;

    private Dictionary<(int, int), HexSlot> slotMap = new Dictionary<(int, int), HexSlot>();

    void Start()
    {
        xOffset = hexSize * 1.5f;
        zOffset = Mathf.Sqrt(3) * hexSize * 0.5f;

        GenerateGrid();
        AssignNeighbors();
    }

    void GenerateGrid()
    {
        for (int r = 0; r < height; r++)
        {
            for (int q = 0; q < width; q++)
            {
                // Offset positioning (odd-r rule)
                float xPos = q * xOffset;
                float zPos = r * (zOffset * 2f);

                if (r % 2 == 1)
                    xPos += xOffset * 0.5f;

                Vector3 position = new Vector3(xPos, 0, zPos);

                GameObject obj = Instantiate(hexSlotPrefab, position, Quaternion.identity, transform);

                HexSlot slot = obj.AddComponent<HexSlot>();
                slot.q = q;
                slot.r = r;

                slotMap[(q, r)] = slot;
            }
        }
    }

    void AssignNeighbors()
    {
        // DIFFERENT DIRECTION SET FOR EVEN AND ODD ROW
        Vector2Int[] evenRowDir = new Vector2Int[]
        {
            new Vector2Int(+1, 0), new Vector2Int(0, -1),
            new Vector2Int(-1, -1), new Vector2Int(-1, 0),
            new Vector2Int(-1, +1), new Vector2Int(0, +1)
        };

        Vector2Int[] oddRowDir = new Vector2Int[]
        {
            new Vector2Int(+1, 0), new Vector2Int(+1, -1),
            new Vector2Int(0, -1), new Vector2Int(-1, 0),
            new Vector2Int(0, +1), new Vector2Int(+1, +1)
        };

        foreach (var slotPair in slotMap)
        {
            HexSlot slot = slotPair.Value;

            var dirs = (slot.r % 2 == 0) ? evenRowDir : oddRowDir;

            foreach (var d in dirs)
            {
                var key = (slot.q + d.x, slot.r + d.y);

                if (slotMap.ContainsKey(key))
                {
                    slot.AddNeighbor(slotMap[key]);
                }
            }
        }
    }
}
