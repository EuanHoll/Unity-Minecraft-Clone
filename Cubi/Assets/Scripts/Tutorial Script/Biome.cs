using UnityEngine;
using System.Collections;

[System.Serializable]
public class Biome {
    public string name = "Unset Biome";
    [Multiline]
    public string desc = "No Description Set";

    public float mountainPower = 1;
    public float minHeight = 0;
    public float maxHeight = 30;

    public float mountainPowerBonus = 0;

    public BlockPlacer[] blockPlacers;

    public byte getBlock(int y, float mountainValue, float blobValue)
    {
        BlockPlacer bestBidder = null;
        float bestBid = 0;
        for (int a = 0; a < blockPlacers.Length; a++)
        {
            float bid = blockPlacers[a].Bid(y, mountainValue, blobValue);
            if (bid > bestBid)
            {
                bestBid = bid;
                bestBidder = blockPlacers[a];
            }
        }

        if (bestBidder == null)
            return 0;
        else
            return (byte)bestBidder.block;
    }
}
