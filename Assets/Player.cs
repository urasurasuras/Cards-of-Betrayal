using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Card> hand; 
    public List<Card> discard;

    // Start is called before the first frame update
    void Start()
    {
        hand = new List<Card>();
        discard = new List<Card>();
        GameManager.Instance.RegisterPlayerControl(this);
        Debug.Log(this + " is registered");
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
        Debug.Log(this.name + " drew " + card._rank + " of "+card._suit);
    }
}
