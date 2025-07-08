using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject MenuPanel;
    public GameObject GamePanel;

    [Header("End Game Panel")]
    public GameObject EndGamePanel;
    public Text ResultTxt;
    public Button RestartBtn;

    public Button[] GameCells;
    public Image[] CellImages;

    public Text ScoresTxt;

    public Image MySignImage;
    public Image OpponentImage;

    [Header("Sprites")]
    public Sprite NoneSprite;
    public Sprite XSprite;
    public Sprite OSprite;

    [Header("Turn Indicators")]
    public GameObject MyTurnIndicator;
    public GameObject OpponentTurnIndicator;

    void Awake()
    {
        UpdateCells(PlayerSymbol.None);
    }

    public void AddListenerToCells(UnityAction<int> call)
    {
        for (int i = 0; i < GameCells.Length; i++)
        {
            int cellIndex = i;
            GameCells[i].onClick.AddListener(() => call(cellIndex));
        }
    }

    public void BackToMenu()
    {
        MenuPanel.SetActive(true);
        GamePanel.SetActive(false);
        EndGamePanel.SetActive(false);
    }

    public void GoToGame()
    {
        GamePanel.SetActive(true);
        MenuPanel.SetActive(false);
        EndGamePanel.SetActive(false);
    }

    public void InitialEndGamePanel(bool gameOver, string message = "")
    {
        EndGamePanel.SetActive(true);
        RestartBtn.onClick.RemoveAllListeners();
        ResultTxt.text = message;
        if (gameOver)
        {
            RestartBtn.onClick.AddListener(BackToMenu);
            RestartBtn.GetComponentInChildren<TMP_Text>().text = "Back to menu";
        }
        else
        {
            RestartBtn.onClick.AddListener(OnRestartClick);
            RestartBtn.GetComponentInChildren<TMP_Text>().text = "Restart";
        }
    }

    public void OnRestartClick()
    {
        EndGamePanel.SetActive(false);
    }

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

    public void UpdateTurn(bool isMyTurn)
    {
        MyTurnIndicator.SetActive(isMyTurn);
        MyTurnIndicator.SetActive(!isMyTurn);
    }

    public void SetSigns(PlayerSymbol mySymbol)
    {
        switch (mySymbol)
        {
            case PlayerSymbol.X:
                MySignImage.sprite = XSprite;
                OpponentImage.sprite = OSprite;
                break;
            case PlayerSymbol.O:
                MySignImage.sprite = OSprite;
                OpponentImage.sprite = XSprite;
                break;
        }
    }
}
