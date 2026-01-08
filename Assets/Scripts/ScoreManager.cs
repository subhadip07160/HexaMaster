using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int currentScore;

    public int GetScoreForNumber(int number)
    {
        return number * 2; // simple & safe rule
    }

    public int CalculateGroupScore(
        System.Collections.Generic.List<HexPieceData> group)
    {
        int total = 0;
        foreach (var h in group)
            total += GetScoreForNumber(h.number);

        return total;
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
    }
}
