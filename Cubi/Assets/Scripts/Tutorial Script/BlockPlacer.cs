using UnityEngine;
using System.Collections;

public enum BlockPlacerCondition
{
    None,
    AboveGround,
    BelowGround,
    GroundLevel
}


[System.Serializable]
public class BlockPlacer {
    public string name = "UnSet BlockPlayer";
    public Blocks block;
    public float weight = 1;
    public BlockPlacerCondition[] conditions;

    public virtual float Bid(int y, float mountainValue, float blobValue)
    {
        float bid = 0;

        for (int a = 0; a < conditions.Length; a++)
        {
            switch (conditions[a])
            {
                case BlockPlacerCondition.None:
                    bid++;
                    break;
                case BlockPlacerCondition.AboveGround:
                    if (y > 50) bid++;
                    break;
                case BlockPlacerCondition.BelowGround:
                    if (y < 50) bid++;
                    break;
                case BlockPlacerCondition.GroundLevel:
                    if (y == 50) bid++;
                    break;

            }

        }

        if (weight == 0) return bid;
        return bid * weight;
    }
}
