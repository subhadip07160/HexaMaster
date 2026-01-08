using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public BottomSpawner spawner;
    public ScoreManager scoreManager;

    public int targetScore;

    void Start()
    {
        int expectedScore = 0;

        // spawn 3 stacks at start
        for (int i = 0; i < 3; i++)
        {
          ////  List<HexPieceData> group =
          //      spawner.SpawnRandomStack_UniqueValue_ColorFree();

          //  expectedScore +=
          //      scoreManager.CalculateGroupScore(group);
        }

        // 🔥 difficulty control
        targetScore = Mathf.RoundToInt(expectedScore * 0.75f);

        Debug.Log("Target Score = " + targetScore);
    }

    public bool CheckWin()
    {
        return scoreManager.currentScore >= targetScore;
    }
}
