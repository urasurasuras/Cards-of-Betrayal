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
        EditorGUIUtility.PingObject(this);

        if (this.transform.parent == go_owner_player.transform.GetChild(0))
        {//if card is in hand
            this.transform.parent = owner_player.go_discard.transform;
            this.transform.position = this.transform.parent.position;
            owner_player.DrawCard(owner_player.discard, owner_player.hand, card);
        }
        else if (this.transform.parent == go_owner_player.transform.GetChild(1))
        {//if card is in discard pile
            Debug.Log("this card is in discard pile");
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
