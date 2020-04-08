using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    [SerializeField]
    List<Player> playerList;    //List of players that have cards in their hands and their discard piles
    [SerializeField]
    List<Card> neutralCardList; //List of cards that are in the middle
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        CreateStandardDeck();
        _instance.playerList = new List<Player>();
        DealCards(5);
    }

    [MenuItem("GameObject/Game Manager/Card Game", false, 10)]
    static void CardManager_new()
    {
        if (GameObject.Find("CardManager"))
        {
            Debug.LogWarning("CardManager already exists");
            return;
        }
        new GameObject("CardManager");


    }
    [MenuItem("Create/Deck/Standard Playing Deck")]
    static void CreateStandardDeck()
    {
        //Initialize deck
        _instance.neutralCardList = new List<Card>();
        for (int i = 1; i <= 13; i++)
        {
            for (int j = 1; j <= 4; j++)
            {
                string path = "Cards/" + Card.rankDict[i] + Card.suitDict[(Suit)j];

                Sprite sprite = Resources.Load<Sprite>(path);

                _instance.neutralCardList.Add(new Card((Suit)j, i, sprite));
                //Debug.Log("Created card " + i + " of " + (Suit)j);
            }
        }
    }

    public void ShiffleDeck()
    {
        Shuffle<Card>(_instance.neutralCardList);
    }
    static void Shuffle<List>(List<Card> alpha)
    {
        for (int i = 0; i < alpha.Count; i++)
        {
            var temp = alpha[i];
            int randomIndex = UnityEngine.Random.Range(i, alpha.Count);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }
        Debug.Log("Shuffled "+alpha); ;
    }

    public void DealCards(int num_cards)
    {
        for (int i = 0; i < num_cards; i++) 
        {
            foreach (Player p in playerList)
            {
                p.DrawCard(p.hand, _instance.neutralCardList);
            }
        }
    }
    public void RegisterPlayerControl(Player pc)
    {
        playerList.Add(pc);
    }
    public void deRegisterPlayerControl(Player pc)
    {
        playerList.Remove(pc);
    }
    private void Update()
    {
       
    }

}
