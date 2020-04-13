using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class go_Card : MonoBehaviour
{
    public SpriteRenderer sr;
    public GameObject owner_player;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(owner_player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
