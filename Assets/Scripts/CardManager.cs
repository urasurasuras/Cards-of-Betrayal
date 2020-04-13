using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //this.transform.parent = GameManager.Instance.transform;

        //_instance.neutralCardList = new List<Card>();
        //Debug.Log(_instance.neutralCardList.Count);

        //CreateStandardDeck();
        //Debug.Log(_instance.neutralCardList.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //Debug.Log(_instance.neutralCardList[_instance.neutralCardList.Count - 1]._rank);
        }
    }
}
