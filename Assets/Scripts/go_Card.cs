﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class go_Card : MonoBehaviour
{
    public Card card;

    public SpriteRenderer sr;
    public GameObject go_owner_player;
    public GameObject back;
    // Start is called before the first frame update
    void Start()
    {
        if (this == null) return;
        back = gameObject.transform.GetChild(1).gameObject;

        Debug.Log(back);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        Player owner_player = go_owner_player.GetComponent<Player>();
        Player nextPlayer = GameManager.Instance.getNextPlayer(owner_player);
        GameObject currentPile = transform.parent.gameObject;

        //owner_player.Discard(this.GetComponent<Card>());
        if (GameManager.Instance.GameState != GameState.Action)
        {
            StartCoroutine(WarningPanel.Instance.ShowMessage("Game has not started yet", 2f));
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (owner_player.state == PlayerState.Discard)
            {
                if (currentPile == owner_player.go_hand)
                {//if card is in hand
                    owner_player.Discard(this);
                }
            }
            else if (nextPlayer == GameManager.Instance.currentPlayer && GameManager.Instance.currentPlayer.state == PlayerState.Draw)
            {
                if (currentPile == owner_player.go_discard)
                {//if card is in discard pile

                    //if (owner_player.discard.Count )
                    if (owner_player.discard.Count > 1)
                    {
                        //spawn cards below
                        GameObject go_card = Instantiate(owner_player.prefab_card, currentPile.transform);
                        go_Card anana = go_card.GetComponent<go_Card>();
                        
                        anana.sr.sprite = owner_player.discard[owner_player.discard.Count -2]._art;
                        anana.card = owner_player.discard[owner_player.discard.Count- 2];
                        owner_player.setOwner(anana, currentPile);
                        anana.transform.parent = owner_player.go_discard.transform;
                        anana.transform.position = owner_player.go_discard.transform.position;
                    }
                    nextPlayer.setOwner(gameObject.GetComponent<go_Card>(), nextPlayer.go_hand);//set this card's parent to next player's hand

                    nextPlayer.state = PlayerState.Discard;
                    GameManager.Instance.t_currentPlayer.text = "Current player: \n" + nextPlayer.name + "\n" + nextPlayer.state + "\nPhase";
                    //owner_player.DiscardToHand(nextPlayer, GameManager.Instance.currentPlayer);
                    //GameObject.Destroy(this);
                    return;
                }
            }
            else
            {
                StartCoroutine(WarningPanel.Instance.ShowMessage(GameManager.Instance.currentPlayer.name + " already drew a card", 2f));

                print(GameManager.Instance.currentPlayer + " already drew a card");
            }

            //else if (transform.parent.gameObject == owner_player.go_discard)
            //{//if card is in discard pile
            //    Debug.Log("Index: " + this.transform.GetSiblingIndex());
            //}
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (currentPile != owner_player.go_hand)
            {
                StartCoroutine(WarningPanel.Instance.ShowMessage("End the game with a card from your hand", 2f));
                print("End the game with a card from your hand");
                return;
            }
            if (owner_player.state != PlayerState.Discard)
            {
                StartCoroutine(WarningPanel.Instance.ShowMessage("You have to be in your discard phase to end the game.", 2f));
                print("You have to be in your discard phase to end the game.");
                return;
            }
            //check then trigger show
            if (!GameManager.Instance.CheckForShow())
            {
                StartCoroutine(WarningPanel.Instance.ShowMessage("Not everyone has 5 discarded cards, you cannot show", 2f));

                print("Not everyone has 5 discarded cards, you cannot show");
                return;
            }
            else
            {
                //discard current card to the middle
                //trigger show event
                GameManager.Instance.EndGame();

            }
        }
    }

    [ContextMenu("Flip")]
    public void Flip()
    {
        if (back.activeSelf) back.SetActive(false);
        else back.SetActive(true);
    }

    public void FaceDown()
    {
        back.SetActive(true);
    }
    public void FaceUp()
    {
        back.SetActive(false);
    }
}
