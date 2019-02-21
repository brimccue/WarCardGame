using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarCardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // set title of console
            Console.Title = "War! by Brianna McCue";
            Console.WriteLine("Welcome to the card game War!\nTwo players will battle for 100 rounds or until one player runs out of cards.\n");

            // create a new game of war, play it through, and return the winner
            Game theGame = new Game();
            theGame.PlayWar();
            Player theWinner = theGame.GetTheWinner();

            // output results
            if (theWinner == null)
            {
                Console.WriteLine("Tie game!");
            }
            else
            {
                Console.WriteLine("The winner is " + theWinner.GetName() + "!");
            }
            Console.Read();
        }
    }
    public class Card
    {
        int suit;    // numerical representation of the suit of the card
        int value;   // numerical representation of the value of the card

        public Card(int s, int v)
        {
            // populate the card with a passed in suit and value
            suit = s;
            value = v;
        }

        public void Display()
        {
            // output the face value of the card
            if (value == 11)
            {
                Console.Write("Jack");
            }
            else if (value == 12)
            {
                Console.Write("Queen");
            }
            else if (value == 13)
            {
                Console.Write("King");
            }
            else if (value == 14)
            {
                Console.Write("Ace");
            }
            else
            {
                // output the numerical value of the card
                Console.Write(value.ToString());
            }

            Console.Write(" of ");

            // output the suit of the card
            if (suit == 1)
            {
                Console.WriteLine("Hearts");
            }
            else if (suit == 2)
            {
                Console.WriteLine("Spades");
            }
            else if (suit == 3)
            {
                Console.WriteLine("Diamonds");
            }
            else if (suit == 4)
            {
                Console.WriteLine("Clubs");
            }
        }

        public int GetSuit()
        {
            // return the suit of the card
            return suit;
        }

        public int GetValue()
        {
            // return the value of the card
            return value;
        }

    }

    public class Deck
    {
        Card[] theDeck = new Card[52];  // create an array of 52 cards for the deck
        int numberOfCards;   // the total number of cards in deck

        public Deck()
        {
            // fill the deck with cards
            Fill();
        }

        public Card Deal()
        {
            // if there are no cards left in the deck, return nothing
            if (numberOfCards == 0)
            {
                return null;
            }
            else
            {
                // remove 1 from total number of cards
                numberOfCards--;

                // return the card sitting on top of the deck
                return theDeck[numberOfCards];
            }
        }

        public void Fill()
        {
            // populate the deck with 52 unique cards
            int index = 0;
            for (int v = 2; v <= 14; v++)
            {
                for (int s = 1; s <= 4; s++)
                {
                    theDeck[index] = new Card(s, v);
                    index++;
                }
            }

            // set total number of cards in the deck to 52, a full deck
            numberOfCards = 52;
        }

        public void Shuffle(Random rnd)
        {
            // randomly shuffle the deck of cards
            for (int next = numberOfCards - 1; next > 0; --next)
            {
                int someRandomNumber = rnd.Next(next + 1);
                Card tempCard = theDeck[next];
                theDeck[next] = theDeck[someRandomNumber];
                theDeck[someRandomNumber] = tempCard;
            }
        }

        public int GetNumberOfCards()
        {
            // return total number of cards remaining in the deck
            return numberOfCards;
        }
    }

    public class Pile
    {
        int bottom;  // integer value for the bottom of the pile
        int top;  // integer value for the top of the pile
        Card[] thePile = new Card[52];  // create a pile of cards that can potentially hold 52 cards

        public Pile()
        {
            // reset the pile upon construction
            Reset();
        }

        public void Reset()
        {
            // reset bottom and top values
            bottom = 0;
            top = 0;
        }

        public Card GetNextCard()
        {
            // check for errors
            if (bottom == top)
            {
                return null;
            }

            // return the card at the bottom of the pile, then increase the bottom value by 1
            Card theCard = thePile[bottom];
            bottom++;

            // return the card that was originally at the bottom of the pile
            return theCard;
        }

        public void AddCard(Card theCard)
        {
            // set the top of the pile to the passed in card, then increase the end value by 1
            thePile[top] = theCard;
            top++;
        }

        public void AddMultipleCards(Pile p)
        {
            // while there are still cards in the passed in pile, add them to main pile
            while (p.GetSize() > 0)
            {
                AddCard(p.GetNextCard());
            }
        }

        public int GetSize()
        {
            // return size of the pile
            return (top - bottom);
        }
    }

    public class Player
    {
        Pile currentPile = new Pile();  // pile for the cards currently in the player's hand
        Pile wonPile = new Pile();  // pile for the cards received from winning turns
        string name;    // string value for the name of the player

        public Player(string theName)
        {
            // set the name of the player
            name = theName;
        }

        public void CollectCard(Card theCard)
        {
            // collect a card by adding a card to the won pile
            wonPile.AddCard(theCard);
        }

        public void CollectMultipleCards(Pile thePile)
        {
            // collect multiple cards by adding them to the won pile
            wonPile.AddMultipleCards(thePile);
        }

        public void ReplaceCurrentPile()
        {
            // replace the current pile being played with the won pile
            currentPile.Reset();
            currentPile.AddMultipleCards(wonPile);
            wonPile.Reset();
        }

        public Card PlayNextCard()
        {
            // if the player has run out of cards in their current deck, replace their pile with the pile of won cards
            if (currentPile.GetSize() == 0)
            {
                ReplaceCurrentPile();
            }

            // if the player has not run out of cards, return the next card in the current pile
            if (currentPile.GetSize() > 0)
            {
                return currentPile.GetNextCard();
            }

            return null;
        }

        public string GetName()
        {
            // return string value for the player's name
            return name;
        }

        public int GetTotalNumberOfCards()
        {
            // return the total size of the play pile and the won pile
            return (currentPile.GetSize() + wonPile.GetSize());
        }
    }

    public class Game
    {
        Player player1 = new Player("Player 1"); // first player and their name
        Player player2 = new Player("Player 2"); // second player and their name
        Random rand = new Random(); // value for randomness

        public void PlayWar()
        {
            // create a pile for a war
            Pile warPile = new Pile();

            // create a deck of cards and shuffle
            Deck theDeck = new Deck();
            theDeck.Shuffle(rand);

            // while there is at least 2 cards in the deck
            while (theDeck.GetNumberOfCards() >= 2)
            {
                // deal the entire deck to the players one card at a time
                player1.CollectCard(theDeck.Deal());
                player2.CollectCard(theDeck.Deal());
            }

            // replace the current piles being played with the collected piles
            player1.ReplaceCurrentPile();
            player2.ReplaceCurrentPile();

            // the game will continue for 100 rounds or until a player runs out of cards
            for (int round = 1; round <= 100; round++)
            {
                // check if players have at least 1 card each
                if (!HasEnoughCards(1))
                {
                    // exit for loop
                    break;
                }

                // assign the current card values
                Card card1 = player1.PlayNextCard();
                Card card2 = player2.PlayNextCard();

                // output what round it is and the players' cards
                Console.WriteLine("Round " + round + ": ");
                Console.Write(player1.GetName() + " put down "); card1.Display();
                Console.Write(player2.GetName() + " put down "); card2.Display();

                if (CompareCards(card1, card2) > 0)
                {
                    // player 1 wins this hand
                    // put the played cards in player 1's win pile and output results
                    player1.CollectCard(card1);
                    player1.CollectCard(card2);
                    Console.WriteLine(player1.GetName() + " won this hand!\n");
                }
                else if (CompareCards(card1, card2) < 0)
                {
                    // player 2 wins this hand
                    // put the played cards in player 2's win pile and output results
                    player2.CollectCard(card1);
                    player2.CollectCard(card2);
                    Console.WriteLine(player2.GetName() + " won this hand!\n");
                }
                else
                {
                    // the cards have the same value, so a war has begun

                    // reset the war pile and add the cards that were just played
                    warPile.Reset();
                    warPile.AddCard(card1);
                    warPile.AddCard(card2);
                    Boolean warIsOver = false;

                    // while the war has not ended,
                    do
                    {
                        // check if the players have at least 4 cards each
                        if (!HasEnoughCards(4))
                        {
                            // exit the do while
                            break;
                        }

                        Console.WriteLine("\nWar! Players, put down 4 cards!");

                        // each player adds 4 cards to the war pile
                        for (int j = 1; j <= 4; j++)
                        {
                            card1 = player1.PlayNextCard();
                            card2 = player2.PlayNextCard();
                            warPile.AddCard(card1);
                            warPile.AddCard(card2);
                        }

                        // output players' fourth card values
                        Console.Write(player1.GetName() + "'s Fourth Card: "); card1.Display();
                        Console.Write(player2.GetName() + "'s Fourth Card: "); card2.Display();

                        if (CompareCards(card1, card2) > 0)
                        {
                            // player 1's fourth card is higher than player 2's fourth card
                            // player 1 wins the war
                            player1.CollectMultipleCards(warPile);
                            warIsOver = true;
                            Console.WriteLine(player1.GetName() + " won this war!\n");
                        }
                        else if (CompareCards(card1, card2) < 0)
                        {
                            // player 2's fourth card is higher than player 1's fourth card
                            // player 2 wins the war
                            player2.CollectMultipleCards(warPile);
                            warIsOver = true;
                            Console.WriteLine(player2.GetName() + " won this war!\n");
                        }
                    } while (!warIsOver);
                }

                // output player card totals
                Console.WriteLine(player1.GetName() + "'s total cards: " + player1.GetTotalNumberOfCards());
                Console.WriteLine(player2.GetName() + "'s total cards: " + player2.GetTotalNumberOfCards());
                Console.WriteLine("");
            }
        }

        public Boolean HasEnoughCards(int amt)
        {
            // if either player has less than the required amount of cards, return false
            if (player1.GetTotalNumberOfCards() < amt || player2.GetTotalNumberOfCards() < amt)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int CompareCards(Card card1, Card card2)
        {
            // return card1's value minus card2's value to compare values
            return (card1.GetValue() - card2.GetValue());
        }

        public Player GetTheWinner()
        {
            if (player1.GetTotalNumberOfCards() > player2.GetTotalNumberOfCards())
            {
                // if player1 has more cards than player2, they won!
                return player1;
            }
            else if (player1.GetTotalNumberOfCards() < player2.GetTotalNumberOfCards())
            {
                // if player2 has more cards than player1, they won!
                return player2;
            }
            else
            {
                // tie
                return null;
            }
        }
    }

}
