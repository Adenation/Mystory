using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* The CardManager manages the cards each player has
 * The current status of them, namely where they presently are and 
 * where they should be moved to:
 * Deck, Hand, Void, Board 
 * The Card Manager should always be aware of the status and location
 * of cards on the players board
 */ 

public class CardManager : MonoBehaviour
{// These consts represent locations cards can be
    public const int DECK = 0; // Hasn't been drawn yet
    public const int HAND = 1; // Has been drawn
    public const int BOARD = 2; // In Play
                                /* Whilst on the board, the card is used by others and tracked differently
                                 * Upon being consumed the controller will have dictionaries of card
                                 * managers to cards so they are linked
                                 */
    public const int VOID = 3; // Banished

    // Orders determine the order cards are drawn from lists
    public const int ASC = 4;
    public const int DESC = 5;

    // Return Messages
    public const string NO_MORE_CARDS = "There were less cards in the " +
        "destination than requested.";
    public const string CARD_MOVED = "Card moved successfully";
    public const string CARD_NOT_FOUND = "Card does not exist in card manager";
    public const string CARD_NOT_FOUND_SOURCE = "Card not found in specified" +
        " location";
    public const string BAD_MOVE_REQUEST = "Less than 1 card was requested " +
        "to be moved, aborting method";

    public const float HAND_CARD_GAP = 0.5f;
    public const float HAND_SIZE_X = 0.64f;
    public const float HAND_SIZE_Y = 0.89f;
    public const float HAND_SIZE_Z = -0.00390625f;

    protected Dictionary<CardGO, int> cards;

    [SerializeField] GameObject item_Pile;
    [SerializeField] GameObject boon_Pile;
    [SerializeField] List<GameObject> character_Pile;
    [SerializeField] GameObject void_Pile;
    [SerializeField] GameObject deck_Pile;
    [SerializeField] GameObject hand_Pile;

    public CardManager()
    {
        cards = new Dictionary<CardGO, int>();
    }

    public GameObject GetDeckPile() { return deck_Pile; }
    // Getters
    public Dictionary<CardGO, int> GetAllCards() { return cards; }
    public List<CardGO> GetCards(int location)
    {
        List<CardGO> cardList = new List<CardGO>();
        foreach (CardGO card in cards.Keys)
        {
            if (cards[card] == location) { cardList.Add(card); }
        }
        return cardList;
    }

    /* This method is kept private so that logic is kept within the class
     * and not exposed to other classes/objects and additionally so
     * the method calls that other classes i.e. the controller should have
     * are not ambigious, e.g. Draw() Discard()
     */ 
    private string Move(int numberOfCards, int source, int destination, Vector3 coords, Quaternion rots) //, int order)
    {
        List<CardGO> tempList = new List<CardGO>();
        if (cards.ContainsValue(source))
        {
            tempList = GetCards(source);
        }
        int i = numberOfCards;
        if (i < 1) { return BAD_MOVE_REQUEST; }
        // If either no more cards are to be moved, in the source
        // location is empty, then stop method
        while (i > 0 && tempList.Count > 0)
        {
            cards[tempList[0]] = destination;
            tempList[0].Move(coords, rots);
            tempList.Remove(tempList[0]);
            i--;
            /* If card order becomes important later uncomment section
             * Such as drawing from discard pile in order of cards
             * discarded otherwise, let the dictionary sort it in the
             * most efficient manner
             
            int index = 0; // First item in list
            switch(order)
            {
                case ASC:
                    // index = 0; No need for a reassignment
                    break;
                case DESC:
                    index = tempList.Count - 1; // Last item in list
                    break;
                default:
                    int index = Random.Range(0, tempList.Count); // Random
                    break;
            }
            */
        }
        if (i > 0) { return NO_MORE_CARDS; } else { return ""; }
    }
    /* This method is for moving a specific card
     * elses probably aren't necessary but are more human readable
     */ 
    private string Move(CardGO card, int source, int destination, Vector3 coords, Quaternion rots)
    {
        if(cards.ContainsKey(card))
        {
            if(cards[card] == source)
            {
                cards[card] = destination;
                card.Move(coords, rots);
                return CARD_MOVED;
            }
            else { return CARD_NOT_FOUND_SOURCE; }
        }
        else { return CARD_NOT_FOUND; }
    }

    // Methods are explicit to restrict what external classes can do
    public string Draw(CardGO card)
    {
        string msg = Move(card, DECK, HAND,
        hand_Pile.transform.position, hand_Pile.transform.rotation);
        AdjustHand();
        return msg;
    }
    public string Discard(CardGO card)
    { // Check deck size and edit position
        string msg = Move(card, HAND, DECK,
        deck_Pile.transform.position, Quaternion.Euler(new Vector3(90f, 0f, 0f)));
        AdjustHand();
        return msg;
    }
    public string Play(CardGO card, Transform target) {
        string msg =  Move(card, HAND, BOARD, 
        target.position, target.localRotation);
        AdjustHand();
        return msg;
    }
    public string Use(CardGO card)
    {
        string msg = Move(card, HAND, VOID,
        void_Pile.transform.position, void_Pile.transform.localRotation);
        AdjustHand();
        return msg;
    }
    public string Banish(CardGO card) { return Move(card, BOARD, VOID,
        void_Pile.transform.position, void_Pile.transform.localRotation); }
    public string Consume(CardGO card) { return Move(card, BOARD, DECK,
        deck_Pile.transform.position, deck_Pile.transform.localRotation); }
    
    // Move amounts not knowing which card
    public string Draw(int amount)
    {
        string msg = Move(amount, DECK, HAND,
        hand_Pile.transform.position, hand_Pile.transform.rotation);
        AdjustHand();
        return msg;
    }
    public string Discard(int amount)
    {
        string msg = Move(amount, HAND, DECK,
        deck_Pile.transform.position, deck_Pile.transform.localRotation);
        AdjustHand();
        return msg;
    }
    public string Play(int amount, Transform target)
    {
        string msg = Move(amount, HAND, BOARD,
        target.position, target.localRotation);
        AdjustHand();
        return msg;
    }
    public string Use(int amount)
    {
        string msg = Move(amount, HAND, VOID,
        void_Pile.transform.position, void_Pile.transform.localRotation);
        AdjustHand();
        return msg;
    }
    public string Banish(int amount) { return Move(amount, BOARD, VOID,
        void_Pile.transform.position, void_Pile.transform.localRotation); }
    public string Consume(int amount) { return Move(amount, BOARD, DECK,
        deck_Pile.transform.position, deck_Pile.transform.localRotation); }
    
    public void LoadDeck(CardGO card) { cards.Add(card, DECK); }
    private void AdjustHand()
    {
        List<CardGO> cardList = GetCards(HAND);
        // Move cards to fit in hand
        //float handspan = 0.64f + (cardList.Count - 1 * 0.64f *
        // Randomizer.HandSpanVariance());
        for (int i = 0; i < cardList.Count; i++)
        {
            int c = cardList.Count;
            cardList[i].Move(
                (hand_Pile.transform.position +
                 new Vector3(
                     HAND_SIZE_X * (c / 2f - i) * // Greater variance at edges
                     Randomizer.HandSpanVarianceX(), 
                     HAND_SIZE_Y * (c - (c / 2f - i)) * // Less variance at edges
                     Randomizer.HandSpanVarianceY(),
                     HAND_SIZE_Z * i)*-1),
                     Quaternion.Euler(hand_Pile.transform.localEulerAngles.x, 0f, Randomizer.CardTwister()
                     * (cardList.Count / 2f - i)));
            // times vector by -1 for other way
        }
    }
    /*
    /// Implementation 2 - Lists  ///

    private List<Card> deck = new List<Card>();
    private List<Card> hand = new List<Card>();
    private List<Card> board = new List<Card>();
    private List<Card> _void = new List<Card>();

    // Getters
    public List<Card> GetAllCards()
    {
        // Must be a more elegant way of doing this
        List<Card> cardList = new List<Card>();
        cardList.AddRange(deck); cardList.AddRange(hand);
        cardList.AddRange(board); cardList.AddRange(_void);
        return cardList;
    }
    */
}
