using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public bool active;
    //bool _active;

    //Is the card still on the board?
    public bool inPlay = true;

    //Set the cards current color
    public void SetColor()
    {
        //If the card is selectable it gains a yellow highlight, otherwise it is shown as white
        if (active)
            { GetComponent<Image>().color = new Color(255f/255f, 218f/255f, 13f/255f); }
        else
            { GetComponent<Image>().color = Color.white; }
    }

    //Flips the cards over - the direction parameter is used to flip the card away from the centre
    public void Flip(int direction)
    {
        GetComponent<Animator>().SetTrigger("Flip");
        GetComponent<Animator>().SetInteger("Direction", direction);
        active = !active;
    }

    //The animator will call this function after the flip has completed - this then sets the color correctly
    public void FlipComplete()
    {
        SetColor();
    }

    //This draws the card from the board
    public void Draw()
    {
        if (active)
        {
            Hand.instance.CardSelected(this);
            GetComponent<Animator>().SetTrigger("Draw");
        }

        active = false;
    }

    //Once the card has been drawn it is removed from gameplay
    public void RemoveCard()
    {
        inPlay = false;
    }
}
