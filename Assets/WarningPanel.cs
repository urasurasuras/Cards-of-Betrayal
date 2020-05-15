using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningPanel : MonoBehaviour
{
    private static WarningPanel _instance;
    public static WarningPanel Instance
    {
        get
        {
            return _instance;
        }        
    }

    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        text = GetComponent<Text>();
    }
    public IEnumerator ShowMessage(string message, float delay)
    {
        text.text = message;
        text.enabled = true;
        yield return new WaitForSeconds(delay);
        text.enabled = false;
    }

    [ContextMenu("Asdas")]
    void StartBlinking()
    {
        StartCoroutine(ShowMessage("Abc", 2));
    }
    void StopBlinking()
    {
        StopCoroutine(ShowMessage("Abc", 2));
    }


}
