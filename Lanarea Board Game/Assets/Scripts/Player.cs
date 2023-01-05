using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterManager cm;
    EnergyManager em;
    UIManager um;
    Manager lastSelected;
    // Whatever object you clicked on, there is a manager in charge of it
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetManagers(CharacterManager cm, EnergyManager em, UIManager um)
    {
        this.cm = cm; this.em = em; this.um = um;
    }
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
            // Energy can only be selected when in hand
            em.Play(em.GetCurrentlySelected(), cha.transform.position);
        }

    }

    public void OnEnergySelected(EnergyManager em, Energy e)
    {
        if(lastSelected == null)
        {
            lastSelected = em;
        }
        else if (lastSelected.GetType() == typeof(CharacterManager))
        {

        }
    }

    public void OnDeckSelected()
    {
        em.Draw(1);
    }
}
