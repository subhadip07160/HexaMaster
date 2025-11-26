using UnityEngine;

public class HexGridGenerator : MonoBehaviour
{
    public GameObject hexSlotPrefab;
    public int width = 6;
    public int height = 5;
    public float hexSize = 1f;

    private float xOffset;
    private float zOffset;

    void Start()
    {
        xOffset = hexSize * 1.5f;
        zOffset = Mathf.Sqrt(3) * hexSize * 0.5f;

        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float xPos = x * xOffset;

                if (z % 2 == 1)
                    xPos += hexSize * 0.75f;

                Vector3 position = new Vector3(xPos, 0, z * zOffset);
                Instantiate(hexSlotPrefab, position, Quaternion.identity, transform);
            }
        }
    }
}
