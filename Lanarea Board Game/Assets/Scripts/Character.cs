using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] GameObject equip_left;
    [SerializeField] GameObject equip_right;
    [SerializeField] List<GameObject> energy_pile;

    List<Card> energy;
    Card item;

    public GameObject GetEquipLeft() { return equip_left; }
    public Card GetItem() { return item; }
    public void SetItem(Card card) { item = card; }
    public List<Card> GetEnergy() { return energy; }
    public Card SetEnergy(Card card, int energy_slot)
    {
        Card current_energy = energy[energy_slot];
        if(energy_slot > 0 && energy_slot < energy_pile.Count)
        energy[energy_slot] = card;
        return current_energy;
    }
    // Also code logic for gameobject
}
