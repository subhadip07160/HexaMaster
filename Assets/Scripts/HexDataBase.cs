using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "HexTile3D", menuName = "Hex Game/3D Tile Data")]
public class HexDataBase : ScriptableObject
{
    [Header("3D Visuals")]
    public GameObject tilePrefab;
    public Material tileMaterial;
    public Color tileColor;

    [Header("Value Settings")]
    public int minValue = 1;
    public int maxValue = 10;
}
