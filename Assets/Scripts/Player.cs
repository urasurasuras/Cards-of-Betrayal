using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public int index;
    public GameObject go_hand;
    public List<Card> hand;
    public GameObject go_discard;
    public List<Card> discard;
    public GameObject prefab_card;

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

        var go_card = Instantiate(prefab_card, this.transform);
        var anana = go_card.GetComponent<go_Card>();
        anana.sr.sprite = card._art;
        anana.owner_player = gameObject;
        anana.transform.SetParent(go_hand.transform, true);
        anana.transform.rotation = transform.rotation;
        //Debug.Log(gameObject.name + " " + transform.rotation.eulerAngles);
        anana.transform.position += (.5f * transform.right +  new Vector3(0, 0, .1f) ) * go_hand.transform.childCount;
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
}
