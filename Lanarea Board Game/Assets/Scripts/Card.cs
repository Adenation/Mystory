using System.Collections;
using System.Collections.Generic;

/* The Card is a container for all the cards
 * These represents all the cards that can be
 * placed into the users deck, it is a parent
 * class for the different types of cards
 */ 

public class Card
{
    public const string DEFAULT_NAME = "Unnamed";
    public const string DEFAULT_DESCRIPTION = "Undescribed";
    public const int MIN_CATEGORY = 0;
    public const int MAX_CATEGORY = 1; // To be updated as required
    public const int ERR_CATEGORY = -1;

    // Categories
    public const int CATEGORY_ENERGY = 0;
    public const int CATEGORY_ITEM = 1;


    private string cardName;
    private string description;
    private int id;
    private int category;

    public Card(string cardName, string description, int id, int category)
    {
        SetCardName(cardName); SetCardDescription(description);
        this.id = id; SetCardCategory(category);
    }

    public override bool Equals(object obj)
    {
        return obj is Card ? ((Card)obj).GetId() == GetId() : false;
    }

    // Getters
    public string GetCardName() { return cardName; }
    public string GetDescription() { return description; }
    public int GetId() { return id; }
    public int GetCardCategory() { return category; }
    public void SetCardName(string name)
    {
        if (name == null) { cardName = "Name was Null plz fix."; }
        else if (RegexManager.NameValidator(name)) { cardName = name; }
        else { cardName = DEFAULT_NAME; }
    }

    public void SetCardDescription(string description)
    {
        if (description == null) { this.description = "Description was null..."; }
        else if (RegexManager.DescriptionValidator(description)) { this.description = description; }
        else { this.description = DEFAULT_DESCRIPTION; }
    }
    public void SetCardCategory(int category)
    {
        if (category < MIN_CATEGORY || category > MAX_CATEGORY)
        { this.category = ERR_CATEGORY; }
    }
}
