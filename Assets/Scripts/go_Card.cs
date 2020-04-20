using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class go_Card : MonoBehaviour
{
    public Card card;

    public SpriteRenderer sr;
    public GameObject go_owner_player;
    public GameObject back;
    // Start is called before the first frame update
    void Awake()
    {
        back = gameObject.transform.GetChild(1).gameObject;

        Debug.Log(back);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Player owner_player = go_owner_player.GetComponent<Player>();
  
        //owner_player.Discard(this.GetComponent<Card>());

        if (transform.parent.gameObject == owner_player.go_hand)
        {//if card is in hand
            owner_player.Discard(this);
        }
        else if (transform.parent.gameObject == owner_player.go_discard)
        {//if card is in discard pile
            Debug.Log("Index: " + this.transform.GetSiblingIndex());
        }
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
