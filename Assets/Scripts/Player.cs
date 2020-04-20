using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public int index;
    public GameObject prefab_card;

    public GameObject go_hand;
    public List<Card> hand;
    public GameObject go_discard;
    public List<Card> discard;

    // Start is called before the first frame update
    void Start()
    {
        hand = new List<Card>();
        discard = new List<Card>();
        //GameManager.Instance.RegisterPlayerControl(this);
        //Debug.Log(this + " is registered");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Discard(go_Card card)
    {
        //Physical transfer
        card.transform.parent = go_discard.transform;
        card.transform.position = go_discard.transform.position;
        card.transform.position -= (.5f * transform.forward + new Vector3(0, 0, .1f)) * go_discard.transform.childCount;
        Debug.Log( card.transform.GetSiblingIndex());

        //logical card transfer
        DrawCard(discard, hand, card.card);

        //destroy go_Card 2 below
        //if (go_discard.transform.childCount > 2)
        //{
        //    var destroyedCard = go_discard.transform.GetChild(-2).gameObject;
        //    EditorGUIUtility.PingObject(destroyedCard);
        //    Destroy(destroyedCard);
        //}
    }

    public void DeckToHand()
    {

    }
    public void DiscardToHand(GameObject card)
    {
        //Draw game object as is
    }
  
    /// <summary>
    /// Draws a card from the one list and adds it to the other
    /// </summary>
    /// <param name="dst">Destination deck</param>
    /// <param name="src">Source deck</param>
    /// <return><c>if (src.Count <= 0)</c></return>
    public void DrawCard(List<Card> dst, List<Card> src)
    {
        if (src.Count <= 0)
        {
            Debug.Log(src + " out of cards.");
            return;
        }
        var card = src[src.Count - 1];
        dst.Add(card);
        src.Remove(card);
        //Debug.Log(this.name + " drew " + card._rank + " of " + card._suit);

        GameObject go_card = Instantiate(prefab_card, this.transform);
        go_Card anana = go_card.GetComponent<go_Card>();
        anana.sr.sprite = card._art;
        anana.go_owner_player = gameObject;
        anana.transform.SetParent(go_hand.transform, true);
        anana.transform.rotation = transform.rotation;
        anana.card = card;
        //Debug.Log(gameObject.name + " " + transform.rotation.eulerAngles);

        //Offset based on number of cards in hand
        anana.transform.position += (.5f * transform.right +  new Vector3(0, 0, .1f) ) * go_hand.transform.childCount;
    }

    public void DrawCard(List<Card> dst, List<Card> src, Card card)
    {
        dst.Add(card);
        src.Remove(card);
    }


    public void FlipHand()
    {
        GameObject current_hand = gameObject.transform.GetChild(0).gameObject;

        foreach (Transform child in current_hand.transform)
        {
            child.gameObject.GetComponent<go_Card>().Flip();
        }
    }

    public void FaceHandUp()
    {
        GameObject current_hand = gameObject.transform.GetChild(0).gameObject;

        foreach (Transform child in current_hand.transform)
        {
            child.gameObject.GetComponent<go_Card>().FaceUp();
        }
    }
    
    public void FaceHandDown()
    {
        GameObject current_hand = gameObject.transform.GetChild(0).gameObject;

        foreach (Transform child in current_hand.transform)
        {
            child.gameObject.GetComponent<go_Card>().FaceDown();
        }
    }

    [ContextMenu("Size of hands")]
    public void PrintSize()
    {
        Debug.Log("Discarded: " + discard.Count);
    }
}
