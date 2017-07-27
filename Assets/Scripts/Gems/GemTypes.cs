using UnityEngine;
using System.Collections;

public enum GemType : int
{
    BLUE = 1,
    GREEN,
    HEAVENLY,
    PINK,
    PURPLE,
    RED,
    WHITE,
    YELLOW
}

public class GemTypes : MonoBehaviour
{

    public Transform gemBlue;
    public Transform gemGreen;
    public Transform gemHeavenily;
    public Transform gemPink;
    public Transform gemPurple;
    public Transform gemRed;
    public Transform gemWhite;
    public Transform gemYellow;

    public Transform getGem(GemType type)
    {
        switch (type)
        {
            case GemType.BLUE:
                return gemBlue;
            case GemType.GREEN:
                return gemGreen;
            case GemType.HEAVENLY:
                return gemHeavenily;
            case GemType.PINK:
                return gemPink;
            case GemType.PURPLE:
                return gemPurple;
            case GemType.RED:
                return gemRed;
            case GemType.WHITE:
                return gemWhite;
            case GemType.YELLOW:
                return gemYellow;
        }
        return null;
    }

    public Transform randomGem() {
        int gem = Random.Range((int)GemType.BLUE, (int)GemType.YELLOW+1);
        switch (gem) { 
            case (int)GemType.BLUE:
                return gemBlue;
            case (int)GemType.GREEN:
                return gemGreen;
            case (int)GemType.HEAVENLY:
                return gemHeavenily;
            case (int)GemType.PINK:
                return gemPink;
            case (int)GemType.PURPLE:
                return gemPurple;
            case (int)GemType.RED:
                return gemRed;
            case (int)GemType.WHITE:
                return gemWhite;
            case (int)GemType.YELLOW:
                return gemYellow;
        }
        return null;
    }
}