using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class go_Card : MonoBehaviour
{
    public SpriteRenderer sr;
    public GameObject owner_player;
    public GameObject back;
    // Start is called before the first frame update
    void Start()
    {
        back = gameObject.transform.GetChild(1).gameObject;

        Debug.Log(back);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Flip")]
    public void Flip()
    {
        if (back.activeSelf) back.SetActive(false);
        else back.SetActive(true);
    }

    public void FaceDown()
    {
        back.SetActive(true);
    }
    public void FaceUp()
    {
        back.SetActive(false);
    }
}
