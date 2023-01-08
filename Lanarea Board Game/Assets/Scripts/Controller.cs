using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject cardBase;
    [SerializeField] GameObject characterBase;
    [SerializeField] GameObject playerBoard;
    [SerializeField] GameObject fieldBoard;
    [SerializeField] Transform characterParent;
    [SerializeField] Canvas canvas;
    List<Player> players;
    const int MAX_PLAYERS = 3;
    // When multiplayer is available change single to list
    // Start is called before the first frame update
    void Start()
    {
        players = new List<Player>();
        //GameObject board = Instantiate(playerBoard);

        players.Add(GetComponentInChildren<Player>());  //temp until boards are instantiated
        EnergyManager em = players[0].GetEM();
        CharacterManager cm = players[0].GetCM();
        UIManager um = players[0].GetUM();
        CharacterSetup(players[0]);
    }

    void CharacterSetup(Player player)
    {
        // Update to be player instead of playerboard once sorted in editor
        EnergyManager em = players[0].GetEM();
        CharacterManager cm = players[0].GetCM();
        UIManager um = players[0].GetUM();
        List<Character> p = new List<Character>();
        for (int i = 0; i < MAX_PLAYERS; i++)
        {
            Transform child = characterParent.GetChild(i);
            p.Add(child.GetComponent<Character>());
        }
        DisableUnusedCharacters(0); // Update when number of chars is input as parameter
        cm.LoadCharacters(p);
        
    }

    void DisableUnusedCharacters(int number)
    {
        for(int i = MAX_PLAYERS - number; i < MAX_PLAYERS; i++)
        {
            characterParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void DrawCard(EnergyManager cm)
    {
        cm.Draw(1);
    }
    public void UseCard(EnergyManager cm)
    {
        cm.Use(1);
    }
    public void ConsumeCard(EnergyManager cm)
    {
        cm.Consume(1);
    }
    public void PlayCard(EnergyManager cm)
    {
        cm.Play(cm.GetEnergyBodies()[cm.GetCards(Manager.HAND)[0]], Vector3.zero);
    }

}
