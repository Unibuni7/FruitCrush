using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    // how wide i want my board to be.
    public int width;
    // how tall i want my board to be.
    public int height;
    // My prefab game object.
    public GameObject tilePrefab;
    // An array that can contain two variables.
    private BackgroundTile[,] allTiles;
    // An array to contain our characters.
    public GameObject[] characters;
    public GameObject[,] allCharacters;

    // Use this for initialization
    void Start () {
        // My array contains width and height.
        allTiles = new BackgroundTile[width, height];
        allCharacters = new GameObject[width, height];
        SetUp();
	}
	
	private void SetUp()
    {
        // two for loops to run through the width and height.
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // the positon for the width and height.
                Vector2 tempPosition = new Vector2(i, j);
                // what it should setup and where, and Quaternion.identity means no rotation.
                GameObject backgroundTile = Instantiate(tilePrefab,tempPosition,Quaternion.identity) as GameObject;
                // Here i make my hierarchy in unity more clean.
                backgroundTile.transform.parent = this.transform;
                // Here we name our prefab clones.
                backgroundTile.name = "(" + i + "," + j + ")";
                // a random character to use from the array.
                int charactersToUse = Random.Range(0, characters.Length);
                // position and stuff.
                GameObject character = Instantiate(characters[charactersToUse], tempPosition, Quaternion.identity) as GameObject;
                character.transform.parent = this.transform;
                // the name of the characters.
                character.name = "(" + i + "," + j + ")";
                allCharacters[i, j] = character;
            }
        }
    }
}
