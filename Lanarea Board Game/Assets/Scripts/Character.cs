using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, ISelectable
{
    [SerializeField] GameObject equip_left;
    [SerializeField] GameObject equip_right;
    [SerializeField] List<GameObject> energy_pile;
    bool isSelected;
    List<Energy> energies;
    const int MAX_ENERGY = 6;
    Card item;

    private Attributes stats;

    // Character created before  hand
    public Character(Attributes stats)
    {
        this.stats = stats;
    }

    private void Start()
    {
        energies = new List<Energy>();
    }

    public GameObject GetEquipLeft() { return equip_left; }
    public Card GetItem() { return item; }
    public void SetItem(Card card) { item = card; }
    public List<Energy> GetEnergy() { return energies; }
    public void SetEnergy(Energy energy)
    {
        energies.Add(energy);
    }
    // Also code logic for gameobject

    private void OnMouseDown()
    {
        GetComponentInParent<CharacterManager>().OnCharacterSelected(this);
        Debug.Log("Character has been clicked ");
    }

    public void ToggleSelected()
    {
        isSelected = !isSelected;
        if(isSelected) { Selected(); }
        else { Deselected(); }
        // Turns halos/particle effects etc. on and off
    }

    public void Selected()
    {

    }
    public void Deselected()
    {

    }
}
