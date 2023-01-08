using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Manager
{
    [SerializeField] Transform partyPanel;
    [SerializeField] Transform character0EnergySlots;
    [SerializeField] Transform character1EnergySlots;
    [SerializeField] Transform character2EnergySlots;
    [SerializeField] TextMeshProUGUI msgBox;
    [SerializeField] List<GameObject> UIEnergies;
    // The Transform is the character, the Gameobject is the 
    // energy slot parent which contains 6 slots per character
    private Dictionary<Transform, GameObject> slots;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayEnergy(int charNumber, int elementId)
    {
        Transform t = null;
        switch (charNumber)
        {
            case 0:
                t = character0EnergySlots;
                break;
            case 1:
                t = character1EnergySlots;
                break;
            case 2:
                t = character2EnergySlots;
                break;
        }
        bool found = false; int i = 0;
        while (!found && i < t.childCount)
        {
            Transform child = t.GetChild(i);
            if (child.childCount == 0) // No energy currently occupies this slot
            {
                Instantiate(UIEnergies[elementId], child);
                found = true;
            }
            i++;
        }
    }

    public IEnumerator DisplayMessage(string message, float time = 5)
    {
        Debug.Log("Display Message method entered");
        msgBox.text = message;
        yield return new WaitForSeconds(time);
        msgBox.text = "";
    }
}
