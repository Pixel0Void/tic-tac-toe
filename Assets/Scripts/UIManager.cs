using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Image[] CellImages;

    [Header("Sprites")]
    public Sprite NoneSprite;
    public Sprite XSprite;
    public Sprite OSprite;

    public void UpdateCells(PlayerSymbol value)
    {
        for (int i = 0; i < CellImages.Length; i++)
        {
            UpdateCell(i, value);
        }
    }

    public void UpdateCells(PlayerSymbol[] board)
    {
        for (int i = 0; i < CellImages.Length; i++)
        {
            UpdateCell(i, board[i]);
        }
    }

    public void UpdateCell(int index, PlayerSymbol value)
    {
        switch (value)
        {
            case PlayerSymbol.None:
                CellImages[index].sprite = NoneSprite;
                break;
            case PlayerSymbol.X:
                CellImages[index].sprite = XSprite;
                break;
            case PlayerSymbol.O:
                CellImages[index].sprite = OSprite;
                break;
        }
    }
}
