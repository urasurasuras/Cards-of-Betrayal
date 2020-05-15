using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MasterManager : MonoBehaviour
{
    public List<Player> globalPlayerList;    //List of players that have cards in their hands and their discard piles

    private static MasterManager _instance;
    public static MasterManager Instance
    {
        get
        {
            return _instance;
        }
    }
    // Start is called before the first frame update
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    

    public void GetPlayers()
    {
        globalPlayerList = GameManager.Instance.playerList; //Get players
    }
}
