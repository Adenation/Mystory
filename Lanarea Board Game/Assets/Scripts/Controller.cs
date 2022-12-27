using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject cardBase;
    [SerializeField] GameObject playerBoard;
    [SerializeField] GameObject fieldBoard;
    Dictionary<CardManager, int> managers; //int represents playerId
    // When multiplayer is available change single to list
    // Start is called before the first frame update
    void Start()
    {
        managers = new Dictionary<CardManager, int>();
        //GameObject board = Instantiate(playerBoard);
        CardManager cm = playerBoard.GetComponent<CardManager>();
        managers.Add(cm, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void DrawCard(CardManager cm)
    {
        cm.Draw(1);
    }

    public void LoadDeck(CardManager cm)
    {
        for (int i = 16; i > 0; i--)
        {
            CardGO card = Instantiate(cardBase).GetComponent<CardGO>();
            card.Move(cm.GetDeckPile().transform.position,
                Quaternion.Euler(new Vector3(90f, 0f, 0f)));
            card.SetCard(new Card("blank", "blank", 0, 0));
            cm.LoadDeck(card);
        }
    }
}
