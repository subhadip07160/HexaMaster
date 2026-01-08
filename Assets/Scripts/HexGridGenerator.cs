using UnityEngine;
using System.Collections.Generic;

public class HexGridGenerator : MonoBehaviour
{
    [Header("Grid")]
    public GameObject hexSlotPrefab;
    public float hexSize = 1f;
    public HexLevelDataSO levelData;

    private Dictionary<(int, int), HexSlot> slotMap =
        new Dictionary<(int, int), HexSlot>();

    private float xOffset;
    private float zOffset;

    // 🔥 Axial hex directions (HONEY GRID)
    static readonly Vector2Int[] HexDirs =
    {
        new Vector2Int(+1, 0),
        new Vector2Int(+1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, +1),
        new Vector2Int(0, +1)
    };

    void Start()
    {
        xOffset = hexSize * 1.5f;
        zOffset = Mathf.Sqrt(3) * hexSize * 0.5f;

        GenerateEmptyGrid();
        AssignNeighbors();
        CenterGrid();
        ApplyRandomBlockedCells();
    }

    // ================= GRID GENERATION =================

    void GenerateEmptyGrid()
    {
        slotMap.Clear();

        int radius = levelData.gridRadius;

        for (int q = -radius; q <= radius; q++)
        {
            for (int r = -radius; r <= radius; r++)
            {
                int t = -q - r;
                if (Mathf.Abs(t) > radius) continue;

                float xPos = (q + r * 0.5f) * xOffset;
                float zPos = r * zOffset * 2f;

                Vector3 pos = new Vector3(xPos, 0, zPos);

                GameObject obj = Instantiate(
                    hexSlotPrefab,
                    pos,
                    Quaternion.identity,
                    transform
                );

                HexSlot slot = obj.GetComponent<HexSlot>();
                if (slot == null)
                    slot = obj.AddComponent<HexSlot>();

                slot.q = q;
                slot.r = r;
             //   slot.ClearSlot();

                slotMap[(q, r)] = slot;
            }
        }
    }

    // ================= BLOCKED =================

    void ApplyRandomBlockedCells()
    {
        if (levelData.blockedCellCount <= 0) return;

        List<HexSlot> slots = new List<HexSlot>(slotMap.Values);

        int count = Mathf.Min(levelData.blockedCellCount, slots.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, slots.Count);
            slots[index].SetBlocked(true);
            slots.RemoveAt(index);
        }
    }

    // ================= CENTER =================

    void CenterGrid()
    {
        if (slotMap.Count == 0) return;

        Vector3 min = Vector3.one * 9999f;
        Vector3 max = Vector3.one * -9999f;

        foreach (HexSlot slot in slotMap.Values)
        {
            Vector3 pos = slot.transform.position;
            min = Vector3.Min(min, pos);
            max = Vector3.Max(max, pos);
        }

        Vector3 center = (min + max) * 0.5f;

        foreach (HexSlot slot in slotMap.Values)
        {
            slot.transform.position -= center;
        }
    }

    // ================= NEIGHBOURS =================

    void AssignNeighbors()
    {
        foreach (var pair in slotMap)
        {
            HexSlot slot = pair.Value;

            foreach (var dir in HexDirs)
            {
                var key = (slot.q + dir.x, slot.r + dir.y);

                if (slotMap.TryGetValue(key, out HexSlot neighbor))
                {
                    slot.AddNeighbor(neighbor);
                }
            }
        }
    }

   
    bool TryMerge(HexSlot startSlot)
    {
        if (startSlot == null || startSlot.currentPiece == null)
            return false;

        List<HexSlot> matched = new List<HexSlot>();
        HashSet<HexSlot> visited = new HashSet<HexSlot>();

        HexPiece basePiece = startSlot.currentPiece;

        CollectSameColor(startSlot, basePiece.color, matched, visited);

        if (matched.Count < 2)
            return false;

        HexSlot target = matched[0];
        int totalValue = 0;

        foreach (HexSlot s in matched)
        {
            int v = s.currentPiece.number;
            totalValue += v;

            if (v < target.currentPiece.number)
                target = s;
        }

        target.currentPiece.number = totalValue;
        target.currentPiece.UpdateText();

        foreach (HexSlot s in matched)
        {
            if (s == target) continue;

            Destroy(s.currentPiece.gameObject);
            s.Clear();
        }

        FindAnyObjectByType<ScoreManager>()?.AddScore(totalValue);

        // chain reaction
        TryMerge(target);

        return true;
    }


    void CollectSameColor(
        HexSlot slot,
        HexColor color,
        List<HexSlot> result,
        HashSet<HexSlot> visited)
    {
        if (visited.Contains(slot)) return;
        visited.Add(slot);

        if (slot.currentPiece == null) return;
        if (slot.currentPiece.color != color) return;

        result.Add(slot);

        foreach (HexSlot n in slot.GetNeighbors())
        {
            CollectSameColor(n, color, result, visited);
        }
    }
}
