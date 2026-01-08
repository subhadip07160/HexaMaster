using TMPro;
using UnityEngine;

public class HexPiece : MonoBehaviour
{
    public int number;
    public HexColor color;
    public TMP_Text numberText;

    public void Setup(HexPieceData data)
    {
        number = data.number;
        color = data.color;
        UpdateText();
    }

    public void UpdateText()
    {
        numberText.SetText(number.ToString());
    }
}
