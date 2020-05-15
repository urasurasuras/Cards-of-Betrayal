using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    None, Action, End
}
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    [SerializeField]
    public List<Player> playerList;     //List of players that have cards in their hands and their discard piles
    public GameObject buttons;          //GO that parents all the buttons
    //Prefabs
    public GameObject playerPrefab;
    public GameObject endScorePrefab;
    public GameObject totalScorePrefab;

    public List<GameObject> go_PlayerList;
    public Player currentPlayer;

    public List<Card> neutralCardList; //List of cards that are in the middle
    public GameObject go_deck; //Clickable deck in the middle

    public GameObject endScorePanel;
    public GameObject totalScorePanel;
    public GameObject statePanel;

    public Text t_currentPlayer;
    public Text t_currentScore;
    public Text t_currentDiscard;
    public int minDiscard;

    public bool showingTotal = false;                  //Toggle for showing total scores

    public GameState GameState = GameState.None;
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

        t_currentPlayer = statePanel.transform.GetChild(0).GetComponent<Text>();
        t_currentScore = statePanel.transform.GetChild(1).GetComponent<Text>();
        t_currentDiscard = statePanel.transform.GetChild(2).GetComponent<Text>();

        //_instance.playerList = new List<Player>(4);

        GameObject g = Instantiate(playerPrefab, new Vector2(0, -4), Quaternion.identity);
        g.GetComponent<Player>().name = "Player 1";
        go_PlayerList.Add(g);
        var discardPos = g.transform.GetChild(1);
        discardPos.position = g.transform.position;
        discardPos.localPosition = new Vector2(30, 8);
        playerList.Add(g.GetComponent<Player>());


        g = (Instantiate(playerPrefab, new Vector2(7, 0), Quaternion.identity));
        g.GetComponent<Player>().name = "Player 2";
        go_PlayerList.Add(g);
        discardPos = g.transform.GetChild(1);
        discardPos.position = g.transform.position;
        discardPos.localPosition = new Vector2(17, 13);
        g.transform.Rotate(0, 0, 90);
        playerList.Add(g.GetComponent<Player>());

        g = Instantiate(playerPrefab, new Vector2(0, 3), Quaternion.identity);
        g.GetComponent<Player>().name = "Player 3";
        go_PlayerList.Add(g);
        discardPos = g.transform.GetChild(1);
        discardPos.position = g.transform.position;
        discardPos.localPosition = new Vector2(23, 2.5f);
        g.transform.Rotate(0, 0, 180);
        playerList.Add(g.GetComponent<Player>());

        g = Instantiate(playerPrefab, new Vector2(-7, 0), Quaternion.identity);
        g.GetComponent<Player>().name = "Player 4";
        go_PlayerList.Add(g);
        g.transform.Rotate(0, 0, -90);
        discardPos = g.transform.GetChild(1);
        discardPos.position = g.transform.position;
        discardPos.localPosition = new Vector2(15, 15); 
        playerList.Add(g.GetComponent<Player>());

        foreach (Transform button in buttons.transform)
        {
            button.gameObject.SetActive(false);
        }
        buttons.transform.GetChild(0).gameObject.SetActive(true);
        buttons.transform.GetChild(4).gameObject.SetActive(true);
        //foreach (GameObject player in playerGOlist)
        //{
        //    playerList.Add(player.GetComponent<Player>());
        //}

        //print(_instance.playerList.Count);
    }

    [ContextMenu("End game")]
    public void EndGame()
    {
        //ends everyone's turn
        foreach (Player p in playerList)
        {
            p.state = PlayerState.None;
        }
        endScorePanel.SetActive(true);
        //displays everyone's scores
        foreach (Player p in playerList)
        {
            //Create panel for final score
            var score = Instantiate(endScorePrefab, endScorePanel.transform);
            score.GetComponent<Text>().text = p.name + ": \n" + p.GetScore();

            
        }
        if (MasterManager.Instance.globalPlayerList.Count == 0)
        {//pass scores over
            MasterManager.Instance.GetPlayers();
        }
        else
        {//add scores
            foreach (Player p in playerList)
            {
                var playerMatch = MasterManager.Instance.globalPlayerList.Find(player => player == p);
                playerMatch.score += p.GetScore();
            }                
        }

        GameState = GameState.End;
    }

    public bool CheckForShow()
    {
        foreach (Player p in playerList)
        {
            if (p.discard.Count < minDiscard) return false;
        }
        return true;
    }


    private void Update()
    {
        
    }

    //Public button functions
    public void SetPlayerNext()
    {
        setCurrentPlayer(getNextPlayer(currentPlayer));
    }
    public void PickStartingPlayer()
    {
        Player starting = PickRandom(_instance.playerList);
        setCurrentPlayer(starting);
        starting.DeckToHand();
        buttons.transform.GetChild(2).gameObject.SetActive(false);
        buttons.transform.GetChild(3).gameObject.SetActive(true);
        GameState = GameState.Action;
    }
    public void ShuffleDeck()
    {
        Shuffle(_instance.neutralCardList);
        buttons.transform.GetChild(0).gameObject.SetActive(false);
        buttons.transform.GetChild(1).gameObject.SetActive(true);
    }
    public void RestartGame()
    {
        MasterManager.Instance.GetPlayers();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void DisplayTotalScores()
    {
        if (showingTotal)
        {
            //clear panel
            foreach (Transform score in totalScorePanel.transform)
            {
                GameObject.Destroy(score.gameObject);
            }
            //Deactivate panel
            totalScorePanel.SetActive(false);
            showingTotal = false;
        }
        else
        {
            if (MasterManager.Instance.globalPlayerList.Count == 0)
            {
                MasterManager.Instance.GetPlayers();
            }
            //Activate panel
            totalScorePanel.SetActive(true);
            //create everyone's scores
            foreach (Player p in MasterManager.Instance.globalPlayerList)
            {
                var score = Instantiate(totalScorePrefab, totalScorePanel.transform);
                score.GetComponent<Text>().text = p.name + ": \n" + p.GetScore();
            }
            showingTotal = true;
        }
    }

    //ContextMenus

    [ContextMenu("Print players")]
    void PrintPlayers()
    {
        print(_instance.playerList.Count);
    }

    //MenuItems
    //[MenuItem("GameObject/Game Manager/Card Game", false, 10)]
    static void CardManager_new()
    {
        if (GameObject.Find("CardManager"))
        {
            Debug.LogWarning("CardManager already exists");
            return;
        }
        new GameObject("CardManager");
    }
    //[MenuItem("Create/Deck/Standard Playing Deck")]
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
        buttons.transform.GetChild(1).gameObject.SetActive(false);
        buttons.transform.GetChild(2).gameObject.SetActive(true);
    }

    /// <summary>
    /// Set current player and flip cards accordingly
    /// </summary>
    /// <param name="current">The new current player</param>
    void setCurrentPlayer(Player current)
    {
        _instance.currentPlayer = current;
        current.state = PlayerState.Draw;

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

        t_currentPlayer.text = "Current player: \n" + currentPlayer.name;
        t_currentScore.text = "Score: \n" + currentPlayer.GetScore();
        t_currentDiscard.text = "Discarded: \n " + currentPlayer.discard.Count;
        //Ping current player in editor
        //EditorGUIUtility.PingObject(currentPlayer);
    }

    public Player getNextPlayer(Player player)
    {
        Player nextPlayer;
        int current_index = playerList.IndexOf(player);
        if (current_index == playerList.Count - 1)
        {
            nextPlayer = playerList[0];
        }
        else
        {
            nextPlayer = playerList[current_index + 1];
        }

        return nextPlayer;
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
