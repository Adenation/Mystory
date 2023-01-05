using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Manager
{
    List<Character> characters;
    Character currentlySelectedCharacter;  

    private void Start()
    {
        characters = new List<Character>();
    }

    public void LoadCharacters(List<Character> characters)
    {
        this.characters = characters;
        int n = characters.Count;
        switch (n)
        {
            case 1:
                characters[0].transform.position = 
                    player_Board.transform.position;
                break;
            case 2:
                characters[0].transform.position = 
                    player_Board.transform.position +
                    new Vector3(-1.5f, 0f, 0f);
                characters[1].transform.position =
                    player_Board.transform.position +
                    new Vector3(1.5f, 0f, 0f);
                break;
            case 3:
                characters[0].transform.position =
                    player_Board.transform.position;
                characters[1].transform.position =
                    player_Board.transform.position +
                    new Vector3(-2f, 0f, 0f);
                characters[2].transform.position =
                    player_Board.transform.position +
                    new Vector3(2f, 0f, 0f);
                break;
        }
        /*
        for (int i = 0; i < n; i++)
        {
             Mate this is wayyyyy too complicated when there won't be more than 3 chars
             * per player - if this changes later in the future then come back to this
            if(n % 2 == 1) // if odd number of characters
            {
                Vector3 offsetVector;
                if (i == 0)
                {
                    characters[i].transform.position = 
                        player_Board.transform.position;
                }
                else if (i < 3)
                {
                    offsetVector = new Vector3((player_Board.transform.localScale.x * 10 //6f
                        / n / 2), 0f, 0f);
                    if (i % 2 == 1) { offsetVector.x = offsetVector.x * -1; }
                    characters[i].transform.position =
                        player_Board.transform.position + offsetVector;
                }
                else
                {
                    float offset = player_Board.transform.localScale.x / n * 10;
                    offsetVector = new Vector3(offset * (i - 1) / 2 + offset / 2f, 0f, 0f);
                    if (i % 2 == 1) { offsetVector.x = offsetVector.x * -1; }
                    characters[i].transform.position =
                        player_Board.transform.position + new Vector3(
                        player_Board.transform.localScale.x * 10 //6f
                        / n / 2, 0f, 0f);
                }
            }
        }
        */
    }

    public Character GetCurrentlySelected() { return currentlySelectedCharacter; }
    public void OnCharacterSelected(Character character)
    {
        if (currentlySelectedCharacter != null)
        {
            currentlySelectedCharacter.ToggleSelected(); // Deselect old
        }
        currentlySelectedCharacter = character;
        currentlySelectedCharacter.ToggleSelected(); // Select new
        GetComponentInParent<Player>().OnCharacterSelected(this, character);
    }
}
