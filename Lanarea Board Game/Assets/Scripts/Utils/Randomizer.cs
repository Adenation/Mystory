using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Randomizer
{
    public static float CardTwister()
    {
        return Random.Range(0.69f, 0.96f);
    }

    public static float HandSpanVarianceX()
    {
        return Random.Range(0.85f, 0.95f);
    }
    public static float HandSpanVarianceY()
    {
        return Random.Range(0.01f, 0.03f);
    }
}
