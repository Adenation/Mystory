using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : Manager
{
    Dictionary<Energy, int> energy_locations;
    Dictionary<Energy, EnergyBody> energy_bodies;
    Dictionary<EnergyBody, Energy> inverse_energy_bodies;
    EnergyBody currentlySelectedEnergy;
    [SerializeField] List<GameObject> energyBase;
    private static string HAND_FULL_STRING = "Hand limit reached, can't draw anymore.";

    private static int maxHandSize = 9;

    void Awake()
    {
        energy_locations = new Dictionary<Energy, int>();
    }

    public void LoadDeck(List<Energy> deck)
    {
        energy_bodies = new Dictionary<Energy, EnergyBody>();
        inverse_energy_bodies = new Dictionary<EnergyBody, Energy>();
        foreach (Energy e in deck)
        {
            GameObject go = Instantiate(energyBase[e.GetElement()], transform);
            // The energy manager is attached to the energy pile where all
            // energy bodies should be children
            EnergyBody eb = go.GetComponent<EnergyBody>();
            energy_bodies.Add(e, eb);
            inverse_energy_bodies.Add(eb, e);
            energy_locations.Add(e, DECK);
            eb.transform.position = deck_Pile.transform.position;
        }
    }
    public List<Energy> GetCards(int location)
    {
        List<Energy> cardList = new List<Energy>();
        foreach (Energy e in energy_locations.Keys)
        {
            if (energy_locations[e] == location) { cardList.Add(e); }
            //Debug.Log(e);
        }
        return cardList;
    }
    private string Move(int amount, int source, int destination)
    {
        List<Energy> tempList = new List<Energy>();
        if (energy_locations.ContainsValue(source))
        {
            tempList = GetCards(source);
        }
        Debug.Log("Templist size: " + tempList.Count);
        int i = amount;
        if (i < 1) { return BAD_MOVE_REQUEST; }
        // If either no more cards are to be moved, in the source
        // location is empty, then stop method
        while (i > 0 && tempList.Count > 0)
        {
            energy_locations[tempList[0]] = destination;
            TriggerMovement(tempList[0], source, destination);
            tempList.Remove(tempList[0]);
            i--;
        }
        if (i > 0) { return NO_MORE_CARDS; } else { return ""; }
    }
    private string Move(Energy energy, int source, int destination)
    {
        if (energy_locations.ContainsKey(energy))
        {
            if (energy_locations[energy] == source)
            {
                TriggerMovement(energy, source, destination);
                return CARD_MOVED;
            }
            else { return CARD_NOT_FOUND_SOURCE; }
        }
        else { return CARD_NOT_FOUND; }
    }
    private string Move(Energy energy, int source, int destination, Vector3 position)
    {
        if (energy_locations.ContainsKey(energy))
        {
            if (energy_locations[energy] == source)
            {
                TriggerMovement(energy, source, destination, position);
                return CARD_MOVED;
            }
            else { return CARD_NOT_FOUND_SOURCE; }
        }
        else { return CARD_NOT_FOUND; }
    }
    // This method calls the default location of each area in the case
    // Vector3 isn't specified when moving energy
    private void TriggerMovement(Energy energy, int source, int destination)
    {
        Vector3 position = Vector3.zero; // Default value to avoid null
        // Maybe it's better to leave this as null to catch errors?
        switch(destination)
        {
            case DECK: position = deck_Pile.transform.position; break;
            case HAND: position = hand_Pile.transform.position; break;
            case BOARD: position = board_Pile.transform.position; break;
            case VOID: position = void_Pile.transform.position; break;
        }
        TriggerMovement(energy, source, destination, position);
            
    }
    private void TriggerMovement(Energy energy, int source, int destination,
        Vector3 position)
    {
        switch (source)
        {
            case DECK:
                energy_bodies[energy].Expand();
                break;
            case HAND:
                energy_bodies[energy].Deselected();
                energy_bodies[energy].Shrink();
                break;
            case BOARD:
                energy_bodies[energy].Expand();
                break;
            case VOID:
                // Nothing escapes the void
                break;
        }
        energy_locations[energy] = destination;
        switch (destination)
        {
            case DECK:
                energy_bodies[energy].SetPatrolPoints(
                    new List<Vector3>()
                    {
                        position
                    }, false, false);
                energy_bodies[energy].Shrink();
                break;
            case HAND:
                Vector3 offset = new Vector3((HandSize() - 1) / 2f,
                    Random.Range(0f, 0.1f), Random.Range(0f, 0.01f));
                if (HandSize()%2 == 1) { offset.x *= -1; }
                position += offset;
                //Debug.Log(hand_Pile.transform.parent);
                Camera cam = hand_Pile.GetComponentInParent<Camera>();
                energy_bodies[energy].SetPatrolPoints(
                    PatrolGenerator.GenerateScreenPatrolPoints(
                        cam, position, 0, 1, 0, 0.2f, 1), false, true);
                break;
            case BOARD:
                energy_bodies[energy].SetPatrolPoints(
                    new List<Vector3>()
                    {
                        position
                    }, false, false);
                break;

            case VOID:
                energy_bodies[energy].SetPatrolPoints(
                    new List<Vector3>()
                    {
                        position
                    }, false, false);
                energy_bodies[energy].Shrink();
                break;
        }
    }

    // Would be good to make this more generic
    private int HandSize()
    {
        int count = 0;
        foreach(int number in energy_locations.Values)
        {
            if (number == HAND)
            {
                count++;
            }
        }
        return count;
    }

    public string Draw(Energy energy)
    {
        if(HandSize() == maxHandSize)
        {
            return HAND_FULL_STRING;
        }
        return Move(energy, DECK, HAND);
    }
    public string Draw(int amount)
    {
        if (HandSize() == maxHandSize)
        {
            return HAND_FULL_STRING;
        }
        return Move(amount, DECK, HAND);
    }
    public string Use(Energy energy) { return Move(energy, BOARD, DECK); }
    public string Use(int amount) { return Move(amount, BOARD, DECK); }
    public string Consume(Energy energy) { return Move(energy, BOARD, VOID); }
    public string Consume(int amount) { return Move(amount, BOARD, VOID); }
    // User will click on an energy in their hand and then click on the
    // character they wish to attach it to. The click events are managed by
    // the controller which will pass in the position of the character
    public string Play(EnergyBody energyBody, Vector3 charPos)
    {        return Move(inverse_energy_bodies[energyBody], HAND, BOARD, charPos);
    }

    public EnergyBody GetCurrentlySelected() { return currentlySelectedEnergy; }
    public void OnEnergySelected(EnergyBody energy)
    {
        if (currentlySelectedEnergy == energy)
        {
            energy.Deselected(); // Deselect Self
            currentlySelectedEnergy = null; // Reset Var
        }
        else
        {
            if (currentlySelectedEnergy != null)
            {
                currentlySelectedEnergy.Deselected(); // Deselect old
            }
            currentlySelectedEnergy = energy;
            currentlySelectedEnergy.Selected(); // Select new
            GetComponentInParent<Player>().OnEnergySelected(this, energy);
        }
    }

    public Dictionary<Energy, EnergyBody> GetEnergyBodies() { return energy_bodies; }

    public Energy GetEnergyFromBody(EnergyBody eb) { return inverse_energy_bodies[eb]; }
    public EnergyBody GetBodyFromEnergy(Energy e) { return energy_bodies[e]; }

    // Update is called once per frame
    void Update()
    {

    }
}
