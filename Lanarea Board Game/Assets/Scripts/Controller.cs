using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject cardBase;
    [SerializeField] GameObject playerBoard;
    [SerializeField] GameObject fieldBoard;
    [SerializeField] GameObject energyBase;
    Dictionary<Manager, int> managers; //int represents playerId
    // When multiplayer is available change single to list
    // Start is called before the first frame update
    void Start()
    {
        managers = new Dictionary<Manager, int>();
        //GameObject board = Instantiate(playerBoard);
        EnergyManager cm = playerBoard.GetComponent<EnergyManager>();
        managers.Add(cm, 0);
        List<Energy> l = new List<Energy>() { Instantiate(energyBase).GetComponent<Energy>() };
        for (int i = 5; i > 0; i--)
        {
            l.Add(Instantiate(energyBase).GetComponent<Energy>());
            
        }
        cm.LoadDeck(l);
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
