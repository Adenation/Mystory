using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy
{
    public const int ENERGY_OFFENSE = 0;
    public const int ENERGY_DEFENSIVE = 1;
    public const int ENERGY_SPIRIT = 2;
    public const int ENERGY_EARTH = 3;
    public const int ENERGY_AIR = 4;
    public const int ENERGY_FIRE = 5;
    public const int ENERGY_WATER = 6;
    public const int ENERGY_LIGHT = 7;
    public const int ENERGY_SHADOW = 8;

    // Multi-Fusions

    public const int ENERGY_NEUTRAL = 9; // OFF/DEF/SPR
    public const int ENERGY_MALE = 10; // Air/Fire/Shadow
    public const int ENERGY_FEMALE = 11; // Earth/Water/Light
    public const int ENERGY_ELEMENTAL = 12; // Male + Female
    public const int ENERGY_OMNI = 13; // Neutral + Elemental

    // Dual Elements

    public const int ENERGY_LIGHTNING = 14; // Earth + Air
    public const int ENERGY_SWAMP = 15; // Earth + Water
    public const int ENERGY_MAGMA = 16; // Earth + Fire
    public const int ENERGY_LIFE = 17; // Earth + Light
    public const int ENERGY_POISON = 18; // Earth + Shadow
    public const int ENERGY_ICE = 19; // Air + Water
    public const int ENERGY_SUN = 20; // Air + Fire
    public const int ENERGY_NEO = 21; // Air + Light
    public const int ENERGY_SULFUR = 22; // Air + Shadow
    public const int ENERGY_GEYSER = 23; // Water + Fire
    public const int ENERGY_REDEMPTION = 24; // Light + FIre
    public const int ENERGY_INFERNO = 25; // Fire + Shadow
    public const int ENERGY_SACRED = 26; // Water + Light
    public const int ENERGY_SLUDGE = 27; // Water + Shadow
    public const int ENERGY_ARCANE = 28; // Light + Shadow


    // Strings

    public const string ENERGY_STRING_OFFENSIVE = "Offensive Physical Energy";
    public const string ENERGY_STRING_DEFENSIVE = "Defensive Physical Energy";
    public const string ENERGY_STRING_SPIRIT = "Spiritual Energy";
    public const string ENERGY_STRING_EARTH = "Earth Energy";
    public const string ENERGY_STRING_AIR = "Air Energy";
    public const string ENERGY_STRING_FIRE = "Fire Energy";
    public const string ENERGY_STRING_WATER = "Water Energy";
    public const string ENERGY_STRING_LIGHT = "Light Energy";
    public const string ENERGY_STRING_SHADOW = "Shadow Energy";
    public const string ENERGY_STRING_NEUTRAL = "Pure Energy";
    public const string ENERGY_STRING_MALE = "Offensive Elemental Energy";
    public const string ENERGY_STRING_FEMALE = "Defensive Elemental Energy Energy";
    public const string ENERGY_STRING_ELEMENTAL = "Elemental Energy";
    public const string ENERGY_STRING_OMNI = "Omni-Energy";

    private int element;
    
    public Energy(int elementId)
    {
        bool isValid = SetElement(elementId);
        if (!isValid) { element = ENERGY_OFFENSE; } // Change to error value later?
    }

    public bool SetElement(int element)
    {
        if (element < ENERGY_OFFENSE || element > ENERGY_ARCANE)
        {
            return false;
        }
        this.element = element;
        return true;
    }

    public int GetElement() { return element; }
    
}
