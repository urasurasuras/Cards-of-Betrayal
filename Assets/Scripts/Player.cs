using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.ComponentModel;

public enum PlayerState
{
    None,
    [Description("Draw")]
    Draw,
    [Description("Discard")]
    Discard
}
public class Player : MonoBehaviour
{
    public new string name;
    public int score;
    public GameObject prefab_card;

    public GameObject go_hand;
    public List<Card> hand;
    public GameObject go_discard;
    public List<Card> discard;

    public PlayerState state;

    // Start is called before the first frame update
    void Start()
    {
        hand = new List<Card>();
        discard = new List<Card>();
        state = PlayerState.None;
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
        //card.transform.position -= (.5f * transform.forward + new Vector3(0, 0, .1f)) * go_discard.transform.childCount;
        if (card.back.activeSelf)
        {
            card.back.SetActive(false);
        }
        //Debug.Log( card.transform.GetSiblingIndex());

        //logical card transfer
        DrawCard(discard, hand, card.card);

        //Reorder this player's hand
        for (int i = 0; i< go_hand.transform.childCount; i++)
        {
            var anana = go_hand.transform.GetChild(i);
            anana.position = this.transform.position;
            anana.rotation = transform.rotation;
            //Offset based on number of cards in hand
            //print(go_hand.transform.childCount);
            anana.position += (.5f * transform.right + new Vector3(0, 0, .1f)) * i;
        }
        //foreach (Transform anana in go_hand.transform)
        //{
        //    anana.position = this.transform.position;
        //    anana.rotation = transform.rotation;
        //    //Offset based on number of cards in hand
        //    print(go_hand.transform.childCount);
        //    anana.position += (.5f * transform.right + new Vector3(0, 0, .1f)) * go_hand.transform.childCount;
        //}
        //End this player's turn
        state = PlayerState.None;
        GameManager.Instance.SetPlayerNext();

        //destroy go_Card below
        if (go_discard.transform.childCount > 1)
        {
            var destroyedCard = go_discard.transform.GetChild(0).gameObject;
            EditorGUIUtility.PingObject(destroyedCard);
            GameObject.Destroy(destroyedCard);
        }
    }

    public void DeckToHand()
    {
        if (state == PlayerState.Discard)
        {
            StartCoroutine(WarningPanel.Instance.ShowMessage("You already drew, now discard a card.", 2f));

            print("You already drew, now discard a card.");
            return;
        }
        DrawCard(this, GameManager.Instance.neutralCardList);
        state = PlayerState.Discard;
    }
    public void DiscardToHand(Player from, Player to)
    {
        //Draw game object as is

        //DrawCard(to.hand, from.discard);
    }
  
    /// <summary>
    /// Draws a card from the one list and adds it to the other
    /// </summary>
    /// <param name="dst">Destination deck</param>
    /// <param name="src">Source deck</param>
    /// <return><c>if (src.Count <= 0)</c></return>
    public void DrawCard(Player dst, List<Card> src)
    {
        if (src.Count <= 0)
        {
            Debug.Log(src + " out of cards.");
            return;
        }
        var card = src[src.Count - 1];
        DrawCard(dst.hand, src, card);
        
        //Instantiate new go_Card
        GameObject go_card = Instantiate(prefab_card, go_hand.transform);
        go_Card anana = go_card.GetComponent<go_Card>();
        anana.go_owner_player = dst.gameObject;
        anana.sr.sprite = card._art;
        anana.card = card;
        //anana.gameObject.AddComponent<go_Card>();

        setOwner(anana, go_hand);
       
        //Offset based on number of cards in hand
        //anana.transform.position += (.5f * transform.right +  new Vector3(0, 0, .1f) ) * go_hand.transform.childCount;
        //anana.FaceUp();//flip up

    }

    public void setOwner(go_Card anana, GameObject pile)
    {
        anana.go_owner_player = gameObject;//set owner
        anana.gameObject.transform.SetParent(pile.transform);//set parent
        //anana.transform.parent = pile.transform;
        anana.transform.position = this.transform.position;
        anana.transform.rotation = transform.rotation;
        //Offset based on number of cards in hand
        anana.transform.position += (.5f * transform.right + new Vector3(0, 0, .1f)) * go_hand.transform.childCount;
        anana.FaceUp();//flip up
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

    [ContextMenu("Current score")]
    public int GetScore()
    {
        int total = 0;
        bool sameSuit = false;
        bool sequential = false;

        List<Card> sortedHand = hand.OrderBy(card => card._rank).ToList();  //Sort hand by ranks

        for (int i = 0; i < sortedHand.Count - 1; i++)//Check if sequential
        {
            int current = sortedHand[i]._rank;
            int next = sortedHand[i + 1]._rank;

            //TODO: fix for king => ace
            //if (current + 1 < 13)
            //{
            //    current = 0;
            //}
            if (current + 1 != next)
            {
                sequential = false;
                break;
            }
            sequential = true;
        }
        for (int i = 0; i < sortedHand.Count - 1; i++)//Check if same suit
        {
            if (sortedHand[i]._suit != sortedHand[i + 1]._suit)
            {
                sameSuit = false;
                break;
            }
            sameSuit = true;
        }
        if (sameSuit && sequential)
        {
            return 150;
        }
        else if (!sameSuit && sequential)
        {
            return 75;
        }
        else if (sameSuit && !sequential)
        {
            return 75;
        }

        Dictionary<int, int> dup_Ranks = new Dictionary<int, int>();//(Rank, amount)
        
        //Adds each card from hand to this dict, increments key if duplicate
        foreach (Card card in hand)
        {
            if (!dup_Ranks.ContainsKey(card._rank))
                dup_Ranks.Add(card._rank, 1);
            else
                dup_Ranks[card._rank]++;
        }
        foreach (var dup in dup_Ranks)
        {
            if (dup.Key == 1) total += 5 * dup.Value;   //If ace
            if (dup.Value == 2) total += 10;            //double
            else if (dup.Value == 3) total += 20;       //triple
            else if (dup.Value == 4) total += 50;       //quadruple
        }

        return total;
    }
}
