using UnityEngine;
using TMPro;

public class HexSpawner : MonoBehaviour
{
    public HexDataBase[] tileTypes;
    public Transform[] spawnPoints;

    private int currentSpawnIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnNextTile();
        }
    }

    void SpawnNextTile()
    {
        Debug.Log(tileTypes.Length);

        for (int i = 0; i < Random.Range(0, 10); i++)
        {
            int h = Random.Range(0, tileTypes.Length);
            Debug.Log(h);

            for (int j = 0; j < h + 1; j++)
            {
                Transform point = spawnPoints[h];

                // *** main stacking fix ***
                Vector3 stackOffset = new Vector3(0, 0.1f * j, 0);

                GameObject tile = Instantiate(tileTypes[h].tilePrefab, point.position + stackOffset, Quaternion.identity);

                Renderer rend = tile.GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.material = tileTypes[h].tileMaterial;
                    rend.material.color = tileTypes[h].tileColor;
                }

                TMP_Text txt = tile.GetComponentInChildren<TMP_Text>();
                if (txt != null)
                {
                    int randValue = Random.Range(tileTypes[h].minValue, tileTypes[h].maxValue + 1);
                    txt.text = randValue.ToString();
                }

                currentSpawnIndex++;

                if (currentSpawnIndex >= spawnPoints.Length)
                {
                    currentSpawnIndex = 0;
                }
            }
        }
    }
}
