using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager : MonoBehaviour
{
    // These consts represent locations cards can be
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


    [SerializeField] protected GameObject void_Pile;
    [SerializeField] protected GameObject deck_Pile;
    [SerializeField] protected GameObject hand_Pile;
    [SerializeField] protected GameObject board_Pile;

}
