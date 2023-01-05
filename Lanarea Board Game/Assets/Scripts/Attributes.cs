using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes
{
    public const int DEFAULT_HEALTH = 10;
    public const int DEFAULT_MOVE = 5;
    public const int DEFAULT_SPEED = 3;

    private int maxHealth;
    private int currentMaxHealth;
    private int currentHealth;
    private int movesLeft;
    private int movesMax;
    private int movesModified;
    private int speed;
    private int modifiedSpeed;

    public Attributes()
    {
        maxHealth = DEFAULT_HEALTH;
        movesMax = DEFAULT_MOVE;
        speed = DEFAULT_SPEED;
    }
    public Attributes(int maxHealth, int movesMax, int currentSpeed)
    {
        this.maxHealth = maxHealth;
        this.movesMax = movesMax;
        speed = currentSpeed;
    }

    public void Initialize()
    {
        currentHealth = maxHealth;
        currentMaxHealth = maxHealth;
        movesLeft = movesMax;
        modifiedSpeed = speed;
    }
}
