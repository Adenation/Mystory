using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckPile : MonoBehaviour
{
    private void OnMouseDown()
    {
        GetComponentInParent<Player>().OnDeckSelected();
    }
}
