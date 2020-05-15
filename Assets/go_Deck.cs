using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class go_Deck : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
    }

    void Start()
    {
        GameManager.Instance.go_deck = gameObject;

    }

    private void OnMouseDown()
    {
        Player current_player = GameManager.Instance.currentPlayer;
        current_player.DeckToHand();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
