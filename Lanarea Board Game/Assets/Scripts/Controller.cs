using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject cardBase;
    [SerializeField] GameObject characterBase;
    [SerializeField] GameObject playerBoard;
    [SerializeField] GameObject fieldBoard;
    [SerializeField] GameObject energyBase;
    [SerializeField] Transform energyParent;
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
        EnergyManager em = playerBoard.GetComponent<EnergyManager>();
        CharacterManager cm = playerBoard.GetComponent<CharacterManager>();
        UIManager um = playerBoard.GetComponent<UIManager>();
        players.Add(GetComponent<Player>()); players[0].SetManagers(cm, em, um);
        List<Energy> l = new List<Energy>();
        for (int i = 6; i > 0; i--)
        {
            Energy e = Instantiate(energyBase, energyParent).GetComponent<Energy>();
            l.Add(e); e.SetElement(i + 2); // Temp to get elemetns
            
        }
        CharacterSetup(players[0]);
        em.LoadDeck(l);
    }

    void CharacterSetup(Player player)
    {
        // Update to be player instead of playerboard once sorted in editor
        CharacterManager cm = playerBoard.GetComponent<CharacterManager>();
        UIManager um = playerBoard.GetComponent<UIManager>();
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
        cm.Play(cm.GetCards(Manager.HAND)[0], Vector3.zero);
    }

}
