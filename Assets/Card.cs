using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Suit
{
    Hearts = 1,
    Clubs = 2,
    Diamonds = 3,
    Spades = 4,
}
//public enum Rank
//{
//    Ace = 1,
//    2 = 2
//}
[CreateAssetMenu]
public class Card : ScriptableObject
{
    public int _rank;
    public Suit _suit;

    public int Rank { get { return _rank; } }
    public Suit Suit { get { return _suit; } }

    private GameObject _card;

    public Card(Suit suit, int rank)
    {
        // to do: validate rank, position, and rotation
        string assetName = string.Format("Card_{0}_{1}", suit, rank);  // Example:  "Card_1_10" would be the Jack of Hearts.
        GameObject asset = GameObject.Find(assetName);
        if (asset == null)
        {
            Debug.LogError("Asset '" + assetName + "' could not be found.");
        }
        else
        {
            //_card = Instantiate(asset, position, rotation);
            _suit = suit;
            _rank = rank;
        }
    }

    public Sprite art;
}
