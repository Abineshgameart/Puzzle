using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Private
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform PiecePrefab;

    private int emptyLocation;
    private int size;

    // Public


    // Createn the Game Setup with size x size Pieces.
    private void CreateGamePieces(float gapThickness)
    {
        // This is the width of Each Tile
        float width = 1 / (float)size;
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Transform piece = Instantiate(PiecePrefab, gameTransform);

                // Pieces will be in a game board going from -1 to +1
                piece.localPosition = new Vector3(-1 + (2 * width * col) + width, +1 - (2 * width * row) - width, 0);
                piece.name = $"{(row * size) + col}";

                // We want an empty Space in the bottom right
                if ((row == size -  1) && (col == size - 1))
                {
                    emptyLocation = (size * size) - 1;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    // we wanat to map the uv coordinates appropriately, they are 0 -> 1.
                    float gap = gapThickness / 2;
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];

                    // UV coord order: (0, 1), (1, 1), (0, 0), (1, 0)
                    uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
                    uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
                    uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));

                    // Assign our new UVs to the Mesh
                    mesh.uv = uv;
                }
            }
        }
    }


    


    // Start is called before the first frame update
    void Start()
    {
        size = 3;
        CreateGamePieces(0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
