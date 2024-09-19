using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class ShuffleScript : MonoBehaviour
{
    public void Shuffle(int emptySpaceIndex, TileScript[] tiles, Transform emptySpace)
    {
        int invertion;

        // For Empty Space Index position in 15th Tile 

        if (emptySpaceIndex != 15)
        {
            var tileOn15LastPos = tiles[15].targetPosition;
            tiles[15].targetPosition = emptySpace.position;
            emptySpace.position = tileOn15LastPos;
            tiles[emptySpaceIndex] = tiles[15];
            tiles[15] = null;
            emptySpaceIndex = 15;
        }

        // Shuffling Tiles

        do
        {
            for (int i = 0; i <= 14; i++)
            {
                var lastPos = tiles[i].targetPosition;
                int randomIndex = Random.Range(0, 14);
                tiles[i].targetPosition = tiles[randomIndex].targetPosition;
                tiles[randomIndex].targetPosition = lastPos;
                var tile = tiles[i];
                tiles[i] = tiles[randomIndex];
                tiles[randomIndex] = tile;
            }
            invertion = GetInversions(tiles);
            Debug.Log("Puzzle Shuffled");
        } while (invertion % 2 != 0);

    }


    // finding the index of Tiles in Shuffling

     /* public int findIndex(TileScript ts)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null)
            {
                if (tiles[i] == ts)
                {
                    return i;
                }
            }
        }
        return -1;
    } */

    // Inversion Calculating and Checking

    int GetInversions(TileScript[] tile)
    {
        int inversionsSum = 0;
        for (int i = 0; i < tile.Length; i++)
        {
            int thisTileInvertion = 0;
            for (int j = i; j < tile.Length; j++)
            {
                if ((tile[j] != null))
                {
                    if (tile[i].number > tile[j].number)
                    {
                        thisTileInvertion++;
                    }
                }
            }
            inversionsSum += thisTileInvertion;
        }
        return inversionsSum;
    }
}
