using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button[] GameCells;
    public Image[] CellImages;

    public Text ScoresTxt;

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

    public void SetCellsInteractable(bool interactable)
    {
        foreach (var cell in GameCells)
        {
            cell.interactable = interactable;
        }
    }

    public void SetCellInteractable(int index, bool interactable)
    {
        GameCells[index].interactable = interactable;
    }

    public void UpdateScoreBoard(PlayerSymbol mySymbol, int xScore, int oScore)
    {
        if (mySymbol == PlayerSymbol.X)
            ScoresTxt.text = $"{xScore} : {oScore}";
        else
            ScoresTxt.text = $"{oScore} : {xScore}";
    }
}
