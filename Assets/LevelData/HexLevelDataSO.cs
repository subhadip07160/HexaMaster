using UnityEngine;
using System.Collections.Generic;

public enum HexColor
{
    Red,
    Blue,
    Yellow,
    Green
}

[System.Serializable]
public class HexPieceData
{
    public HexColor color;
    public int number;          // 1, 4, 7, 5 etc
    public int span;            // কয়টা hex cell cover করবে
}

[System.Serializable]
public class SpawnPiece
{
    public HexPieceData piece;
    public int count;           // stack size / spawn probability
}

[System.Serializable]
public class MatchRule
{
    public int number;
    public int requiredCount;
    public int score;
}

[System.Serializable]
public class StarScore
{
    public int score;
}

[CreateAssetMenu(
    fileName = "HexLevel",
    menuName = "Hex Master/Data Level"
)]
public class HexLevelDataSO : ScriptableObject
{
    [Header("Level Info")]
    public int levelNumber;

    [Header("Grid")]
    public int gridRadius;       // 3D hex radius (easy generation)
    public int blockedCellCount;

    [Header("Spawn Pieces (Bottom)")]
    public List<SpawnPiece> spawnPieces;

    [Header("Match Rules")]
    public List<MatchRule> matchRules;

    [Header("Win Condition")]
    public int targetScore;
    public int maxMoves;

    [Header("Stars")]
    public List<StarScore> stars;
}
