using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public Button PlayBtn;

    void OnEnable()
    {
        PlayBtn.interactable = true;
    }

    public void OnPlayClick()
    {
        PlayBtn.interactable = false;
        NetworkManager.Instance.Socket.Emit("findMatch");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
