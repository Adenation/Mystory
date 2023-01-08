using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] CharacterManager cm;
    [SerializeField] EnergyManager em;
    [SerializeField] UIManager um;
    Manager lastSelected;
    List<Energy> deck;
    // Default Deck
    public static readonly List<Energy> defaultDeck =
        new List<Energy>()
        {
            new Energy(0), new Energy(0), new Energy(0), new Energy(0),
            new Energy(1), new Energy(1), new Energy(1), new Energy(1),
            new Energy(2), new Energy(2), new Energy(2), new Energy(2),
            new Energy(3), new Energy(4), new Energy(5), new Energy(6),
        };

    // Whatever object you clicked on, there is a manager in charge of it
    
    // Start is called before the first frame update
    void Start()
    {
        // Check if deck = null?
        // Will be clearer when deck building is possible

        //SetManagers(cm, em, um);
        if (deck == null || deck.Count == 0)
        {
            deck = defaultDeck;
        }
        em.LoadDeck(deck);
    }

    

    public List<Energy> GetDeck()
    {
        return deck;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetManagers(CharacterManager cm, EnergyManager em, UIManager um)
    {
        this.cm = cm; this.em = em; this.um = um;
    }
    public CharacterManager GetCM() { return cm; }
    public EnergyManager GetEM() { return em; }
    public UIManager GetUM() { return um; }
    // For multi-targeting abilities will need to rethink, skill manager
    // Should allow multi-selecting
    public void OnCharacterSelected(CharacterManager cm, Character cha)
    {
        if(lastSelected == null)
        {
            lastSelected = cm;
        }
        else if (lastSelected.GetType() == typeof(EnergyManager))
        {
            // If character has energy slots available then play energy
            if(cha.CheckEnergyCount())
            {
                EnergyBody eb = em.GetCurrentlySelected();
                Energy e = em.GetEnergyFromBody(eb);
                // Energy can only be selected when in hand
                em.Play(eb, cha.transform.position);
                cha.AbsorbEnergy(e);
                // Later maybe change to better char comparison
                um.DisplayEnergy(cm.GetCurrentlySelectedIndex(), e.GetElement());
            }
            else
            {
                // Otherwise report to UI Manager, player has max energy
                um.DisplayMessage(Character.CHARACTER_MAX_ENERGY);
            }
            ResetSelected();
        }

    }
    // Reset after executing an action;
    public void ResetSelected() { lastSelected = null; }

    public void OnEnergySelected(EnergyManager em, EnergyBody e)
    {
        if(lastSelected == null)
        {
            lastSelected = em;
        }
        else if (lastSelected.GetType() == typeof(CharacterManager))
        {
            ResetSelected();
        }
    }
    public void OnDeckSelected()
    {
        StartCoroutine(um.DisplayMessage(em.Draw(1)));
    }
}
