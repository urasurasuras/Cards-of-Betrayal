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

public class Card
{
    public static Dictionary<Suit, string> suitDict = new Dictionary<Suit, string>()
    {
        {Suit.Hearts,"H"},
        {Suit.Clubs, "C"},
        {Suit.Diamonds,"D"},
        {Suit.Spades, "S"}
    };
    public static Dictionary<int, string> rankDict = new Dictionary<int, string>()
    {
        {1, "A"},
        {2, "2"},
        {3, "3"},
        {4, "4"},
        {5, "5"},
        {6, "6"},
        {7, "7"},
        {8, "8"},
        {9, "9"},
        {10, "10"},
        {11, "J"},
        {12, "Q"},
        {13, "K"}
    };


    public int _rank;
    public Suit _suit;

    public Sprite _art;

    GameObject _card;

    public Card(Suit suit, int rank, Sprite art)
    {
        _suit = suit;
        _rank = rank;
        _art = art;
    }
}

public struct Pile
{

    List<Card> cards;
}