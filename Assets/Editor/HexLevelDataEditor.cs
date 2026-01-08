using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexLevelDataSO))]
public class HexLevelDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HexLevelDataSO data = (HexLevelDataSO)target;

        GUILayout.Space(10);
        GUILayout.Label("🔍 Level Validation", EditorStyles.boldLabel);

        bool isValid = true;

        // -------- GRID CHECK --------
        if (data.gridRadius <= 0)
        {
            ShowError("Grid Radius must be greater than 0");
            isValid = false;
        }

        if (data.blockedCellCount < 0)
        {
            ShowError("Blocked Cell Count cannot be negative");
            isValid = false;
        }

        // -------- SPAWN PIECES CHECK --------
        if (data.spawnPieces == null || data.spawnPieces.Count == 0)
        {
            ShowError("Spawn Pieces list is empty");
            isValid = false;
        }
        else
        {
            foreach (var sp in data.spawnPieces)
            {
                if (sp.piece == null)
                {
                    ShowError("Spawn Piece has null HexPieceData");
                    isValid = false;
                    break;
                }

                if (sp.piece.number <= 0)
                {
                    ShowError("HexPiece number must be > 0");
                    isValid = false;
                    break;
                }

                if (sp.count <= 0)
                {
                    ShowError("Spawn Piece count must be > 0");
                    isValid = false;
                    break;
                }
            }
        }

        // -------- MATCH RULE CHECK --------
        if (data.matchRules == null || data.matchRules.Count == 0)
        {
            ShowWarning("No Match Rules defined");
        }
        else
        {
            foreach (var rule in data.matchRules)
            {
                if (rule.requiredCount < 2)
                {
                    ShowError("Match required count must be >= 2");
                    isValid = false;
                    break;
                }

                if (rule.score <= 0)
                {
                    ShowError("Match score must be > 0");
                    isValid = false;
                    break;
                }
            }
        }

        // -------- WIN CONDITION --------
        if (data.targetScore <= 0)
        {
            ShowError("Target Score must be > 0");
            isValid = false;
        }

        if (data.maxMoves <= 0)
        {
            ShowWarning("Max Moves is not set properly");
        }

        // -------- STAR CHECK --------
        if (data.stars == null || data.stars.Count == 0)
        {
            ShowWarning("No Star thresholds defined");
        }

        GUILayout.Space(10);

        // -------- FINAL RESULT --------
        if (isValid)
        {
            GUI.color = Color.green;
            GUILayout.Label("✅ LEVEL DATA IS VALID", EditorStyles.boldLabel);
        }
        else
        {
            GUI.color = Color.red;
            GUILayout.Label("❌ LEVEL DATA HAS ERRORS", EditorStyles.boldLabel);
        }

        GUI.color = Color.white;
    }

    void ShowError(string msg)
    {
        EditorGUILayout.HelpBox(msg, MessageType.Error);
    }

    void ShowWarning(string msg)
    {
        EditorGUILayout.HelpBox(msg, MessageType.Warning);
    }
}
