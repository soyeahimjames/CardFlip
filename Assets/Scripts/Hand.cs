using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    //A reference to the current Hand
    public static Hand instance;

    public List<Card> cards;

    public Transform Board;

    public int HandSize = 4;
    public Card card;

    public int CompletedCards;

    //Praise text is shown after the player completes a board
    public TextMeshProUGUI praiseText;
    string[] praise = {"Good Job!", "Nice Work", "Great Going!", "Excellent!", "Keep Going!", "Your Doing Great!", "Aces Charles!" };
    
    //Set the Instance reference, then deal a hand
    void Start()
    {
        instance = this;

        Deal();
    }

    //Deal a new hand across the board
    void Deal()
    {
        //Calculate the screensize to scale the cards correctly
        int screensize = Screen.width / 11;

        //Calculate where to start dealing cards from
        float startPos = -((HandSize / 2) * screensize);
        if (HandSize % 2 == 0)
            {startPos = startPos + (screensize / 2);}

        //Deal cards to match the required handsize
        for (int i = 0; i < HandSize; i++)
        {
            //Create a new card
            Card newCard = Instantiate(card);
            //Add the card to the cards list
            cards.Add(newCard);
            //Set the cards position on the board and ensure its the correct size
            newCard.transform.position = new Vector2(startPos + (screensize * i),0);
            newCard.transform.SetParent(Board,false);
            newCard.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 12, 300f);
        }

            Shuffle();
    }

    //Assign some cards as odd and shuffle their position on the board
    void Shuffle()
    {
            List<Card> ShuffledCards = new List<Card>(cards);

            ShuffledCards.Shuffle();

            //The number of Odd Cards allowed to ensure the game remains completable
            int Num = GetOdd();

            for (int i = 0; i < Num; i++)
            {
                ShuffledCards[i].active = true;
                ShuffledCards[i].SetColor();
            }

        StartCoroutine(StartRound());
    }

    //Show some praise, then start a round
    IEnumerator StartRound()
    {

        float percent = 0;

        //Show the player some praise
        while (percent <= 1)
        {
            percent += Time.deltaTime/1.2f;
            praiseText.alpha = percent;
            yield return null;
        }
        //Keep that praise going a little while...but not to long :)
        yield return new WaitForSeconds( .5f);
        percent = 0;
        //Hide the praise now its time to start playing
        while (percent <= 1)
        {
            percent += Time.deltaTime / .5f;
            praiseText.alpha = 1-percent;
            yield return null;
        }

         Board.GetComponent<Animator>().SetTrigger("Fade");
        //Start the timer
        Timer.instance.pause = false;

        yield return null;
    }

    //Clear the board and the list of cards - Then deal a new hand
    public void ResetBoard()
    {
        cards.Clear();
        CompletedCards = 0;
        foreach (Transform child in Board)
        {
            Destroy(child.gameObject);
        }

        Deal();
    }

    //Each new round increases the hand size to a maximum of 11
    void NewRound()
    {
        if (HandSize != 11)
            HandSize++;

        //select a new piece of random praise
        praiseText.text = praise[Random.Range(0, praise.Length)];
        //Clear the board ready for a new round
        ResetBoard();
    }

    //Set the handsize back to 4 then reset the board
    public void ResetHandsize()
    {
        HandSize = 4;
        praiseText.text = "Good Luck!";
        Board.GetComponent<Animator>().SetTrigger("Hide");

        Invoke("ResetBoard", 1f); ;
    }

    //When the player selected a card - flip its neighbours
    public void CardSelected(Card card)
    {
        int x = cards.IndexOf(card);

        //Flip the card to its left
        if ((x - 1) > -1)
        {
            if (cards[x - 1].inPlay)
            {
                cards[x - 1].Flip(-1);
            }
        }
        //Flip the card to the right
        if ((x + 1) < cards.Count)
        {
            if (cards[x + 1].inPlay)
            {
                cards[x + 1].Flip(1);
            }
        }
        
        //Remove the selected card
        card.RemoveCard();

        //Tell the board a card has been removed
        CompletedCards++;

        //IF the removed cards matches the handsize the player has successfully completed the board
        if (CompletedCards == HandSize)
        {
            //The timer is paused whilst praise is shown
            Timer.instance.pause = true;
            Board.GetComponent<Animator>().SetTrigger("Hide");
            //A new round will begin shortly
            Invoke("NewRound", 1f);
        }
    }

    //Checks whether there are still moves available on the board
    public bool MovesAvailable()
    {
        foreach (Card card in cards)
        {
            if (card.active)
                return true;
        }

        return false;
    }
    
    //Calculates the number of cards which need to start highlighted in order to make the game winable 
    int GetOdd()
    {
        int Num = Random.Range(1, cards.Count+1);

        if (Num%2 ==0)
        {
            Num--;
        }
        return Num;
    }

}


public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}