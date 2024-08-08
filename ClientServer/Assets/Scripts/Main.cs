using UnityEngine;

public class Main : MonoBehaviour
{
    public UIInput UIComponent;
    // Start is called before the first frame update
    void Start()
    {
        if (NetAsyncMgr.Instance == null)
        {
            GameObject AsyncMgr = new GameObject();
            AsyncMgr.AddComponent<NetAsyncMgr>();
            AsyncMgr.name = "AsyncMgr";
        }
        NetAsyncMgr.Instance.Connect("127.0.0.1",8080);
    }
    
}
