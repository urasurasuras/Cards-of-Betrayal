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

    public GameObject playerPrefab;
    public List<GameObject> playerGOlist;
    public Player currentPlayer;
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
        //_instance.playerList = new List<Player>(4);

        GameObject g = Instantiate(playerPrefab, new Vector2(0, -4), Quaternion.identity);
        playerGOlist.Add(g);
        var discardPos = g.transform.GetChild(1);
        discardPos.position = g.transform.position;
        discardPos.localPosition = new Vector2(30, 8);
        playerList.Add(g.GetComponent<Player>());


        g = (Instantiate(playerPrefab, new Vector2(7, 0), Quaternion.identity)); 
        playerGOlist.Add(g);
        discardPos = g.transform.GetChild(1);
        discardPos.position = g.transform.position;
        discardPos.localPosition = new Vector2(25, 3);
        g.transform.Rotate(0, 0, 90);
        playerList.Add(g.GetComponent<Player>());

        g = Instantiate(playerPrefab, new Vector2(0, 3), Quaternion.identity);
        playerGOlist.Add(g);
        g.transform.Rotate(0, 0, 180);
        playerList.Add(g.GetComponent<Player>());

        g = Instantiate(playerPrefab, new Vector2(-7, 0), Quaternion.identity); 
        playerGOlist.Add(g);
        g.transform.Rotate(0, 0, -90);
        discardPos = g.transform.GetChild(1);
        discardPos.position = g.transform.position;
        discardPos.localPosition = new Vector2(15, 15); 
        playerList.Add(g.GetComponent<Player>());

        //foreach (GameObject player in playerGOlist)
        //{
        //    playerList.Add(player.GetComponent<Player>());
        //}

        //print(_instance.playerList.Count);
    }
    private void Update()
    {

    }

    //Public button functions
    public void SetPlayerNext()
    {
        Player nextPlayer;
        int current_index = playerList.IndexOf(currentPlayer);
        if (current_index == playerList.Count - 1)
        {
            nextPlayer = playerList[0];
        }
        else
        {
            nextPlayer = playerList[current_index + 1];
        }

        setCurrentPlayer(nextPlayer);
    }
    public void PickStartingPlayer()
    {
        Player starting = PickRandom(_instance.playerList);
        starting.DrawCard(starting.hand, neutralCardList);
        setCurrentPlayer(starting);
    }
    public void ShuffleDeck()
    {
        Shuffle(_instance.neutralCardList);
    }

    //ContextMenus

    [ContextMenu("Print players")]
    void PrintPlayers()
    {
        print(_instance.playerList.Count);
    }

    //MenuItems
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

    //Private functions

    /// <summary>
    /// Returns a random item from a list
    /// </summary>
    /// <param name="players">List to pick from</param>
    /// <returns>null on error</returns>
    Player PickRandom(List<Player> players)
    {
        int random = UnityEngine.Random.Range(0, players.Count);
        return players[random];
    }

    /// <summary>
    /// Shuffle deck
    /// </summary>
    /// <param name="alpha">Deck</param>
    static void Shuffle(List<Card> alpha)
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

    /// <summary>
    /// Deals <c>num_cards</c> amount of cards to each player
    /// </summary>
    /// <param name="num_cards">Number of cards to deal</param>
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

    /// <summary>
    /// Set current player and flip cards accordingly
    /// </summary>
    /// <param name="current">The new current player</param>
    void setCurrentPlayer(Player current)
    {
        _instance.currentPlayer = current;

        //Flip cards based on current player
        foreach (Player p in playerList)
        {
            if (p == currentPlayer)
            {
                p.FaceHandUp();
            }
            else
            {
                p.FaceHandDown();
            }
        }

        //Ping current player in editor
        EditorGUIUtility.PingObject(currentPlayer);
    }

//    /// <summary>
//    /// Register player to <c>GameManager.playerList</c> 
//    /// </summary>
//    /// <param name="pc">Player to register</param>
//    public void RegisterPlayerControl(Player pc)
//    {
//        playerList.Insert(pc.index - 1, pc);
//    }

//    /// <summary>
//    /// De-register player to <c>GameManager.playerList</c> 
//    /// </summary>
//    /// <param name="pc">Player to register</param>
//    public void deRegisterPlayerControl(Player pc)
//    {
//        playerList.Remove(pc);
//    }
}
