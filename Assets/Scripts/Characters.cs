using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour {

    [Header("Board Variables")]
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;
    private GameObject otherChararcter;
    private Board board;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    public float swipeAngel = 0f;

	// Use this for initialization
	void Start () {
        board = FindObjectOfType<Board>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;
        previousColumn = column;
        previousRow = row;

    }
	
	// Update is called once per frame
	void Update () {
        targetX = column;
        targetY = row;
        FindMatches();
        if(isMatched == true)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, .2f);
        }
        if(Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //Moves towards the target.
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        } else
        {
            // Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allCharacters[column, row] = this.gameObject;
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //Moves towards the target.
            tempPosition = new Vector2(transform.position.x ,targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            // Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allCharacters[column, row] = this.gameObject;
        }
    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);
        if(otherChararcter != null)
        {
            if(!isMatched && !otherChararcter.GetComponent<Characters>().isMatched)
            {
                otherChararcter.GetComponent<Characters>().row = row;
                otherChararcter.GetComponent<Characters>().column = column;
                row = previousRow;
                column = previousColumn;
            }
            otherChararcter = null;
        }
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(firstTouchPosition);
    }

    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        swipeAngel = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x)* 180/Mathf.PI;
        Debug.Log(swipeAngel);
        MovePieces();
    }

    void MovePieces()
    {
        if (swipeAngel > -45 && swipeAngel <= 45 && column < board.width -1)
        {
            // Right swipe.
            otherChararcter = board.allCharacters[column + 1, row];
            otherChararcter.GetComponent<Characters>().column -= 1;
            column += 1;

        } else if (swipeAngel > 45 && swipeAngel <= 135 && row < board.height -1)
        {
            // Up swipe.
            otherChararcter = board.allCharacters[column, row + 1];
            otherChararcter.GetComponent<Characters>().row -= 1;
            row += 1;

        } else if ((swipeAngel > 135 || swipeAngel <= -135) && column > 0)
        {
            // Left swipe.
            otherChararcter = board.allCharacters[column - 1, row];
            otherChararcter.GetComponent<Characters>().column += 1;
            column -= 1;

        } else if (swipeAngel < -45 && swipeAngel >= -135 && row > 0)
        {
            // Down swipe.
            otherChararcter = board.allCharacters[column, row -1];
            otherChararcter.GetComponent<Characters>().row += 1;
            row -= 1;
        }

        StartCoroutine(CheckMoveCo());
    }

    void FindMatches()
    {
        if(column > 0 && column < board.width -1)
        {
            GameObject leftCharacter1 = board.allCharacters[column - 1, row];
            GameObject rightCharacter1 = board.allCharacters[column + 1, row];
            if(leftCharacter1.tag == this.gameObject.tag && rightCharacter1.tag == this.gameObject.tag)
            {
                leftCharacter1.GetComponent<Characters>().isMatched = true;
                rightCharacter1.GetComponent<Characters>().isMatched = true;
                isMatched = true;
            }
        }

        if (row > 0 && row < board.height - 1)
        {
            GameObject upCharacter1 = board.allCharacters[column, row +1];
            GameObject downCharacter1 = board.allCharacters[column, row -1];
            if (upCharacter1.tag == this.gameObject.tag && downCharacter1.tag == this.gameObject.tag)
            {
                upCharacter1.GetComponent<Characters>().isMatched = true;
                downCharacter1.GetComponent<Characters>().isMatched = true;
                isMatched = true;
            }
        }
    }
}
