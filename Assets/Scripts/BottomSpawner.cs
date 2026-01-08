using UnityEngine;
using System.Collections.Generic;

public class BottomSpawner : MonoBehaviour
{
    [Header("Prefabs & Parent")]
    public GameObject hexPiecePrefab;
    public GameObject groupPrefab;      // Empty HexGroup prefab
    public Transform spawnParent;

    [Header("Random Settings")]
    public int minNumber = 1;
    public int maxNumber = 10;

    [Header("Spawn Settings")]
    public int visibleCount = 3; // কয়টা group দেখাবে

    void Start()
    {
        // game start এ শুধু group spawn হবে
        for (int i = 0; i < visibleCount; i++)
        {
            SpawnRandomGroup_UniqueValue_ColorFree();
        }
    }

    // ================= GROUP SPAWN =================

    void SpawnRandomGroup_UniqueValue_ColorFree()
    {
        // 1️⃣ Group container বানাও
        GameObject group =
            Instantiate(groupPrefab, spawnParent);

        group.transform.localPosition =
            Vector3.right * (spawnParent.childCount - 1) * 1.6f;

        // 2️⃣ Unique value রাখার জন্য
        HashSet<int> usedValues = new HashSet<int>();

        // 3️⃣ Stack of 4 hex
        for (int i = 0; i < 4; i++)
        {
            GameObject hex =
                Instantiate(hexPiecePrefab, group.transform);

            HexPiece hp = hex.GetComponent<HexPiece>();

            int value;
            do
            {
                value = Random.Range(minNumber, maxNumber + 1);
            }
            while (usedValues.Contains(value));

            usedValues.Add(value);

            HexPieceData data = new HexPieceData
            {
                number = value,                          // ✅ unique
                color = (HexColor)Random.Range(0, 4),    // ✅ random (same allowed)
                span = 1
            };

            hp.Setup(data);

            // stack vertically
            hex.transform.localPosition =
                Vector3.up * i * 0.15f;
        }
    }
}
