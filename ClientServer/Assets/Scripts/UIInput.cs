using UnityEngine;
using UnityEngine.UI;

public class UIInput : MonoBehaviour
{
    public Button SendBtn;
    public InputField InputLabel;

    private void Start()
    {
        SendBtn.onClick.AddListener(() => {
            if (InputLabel.text != "")
            {
                NetAsyncMgr.Instance.Send(InputLabel.text);
            }
        });
    }
}
