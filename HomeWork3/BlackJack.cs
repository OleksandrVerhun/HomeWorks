using System;

namespace HomeWork3
{
    public class BlackJack
    {
        private int wins = 0, loses = 0, draws = 0;

        private Deck deck;
        private List<Card> originalDeck;
        private List<Card> playerCards;
        private List<Card> computerCards;
        private List<Card> dealerCards;

        public BlackJack(Deck deck)
        {
            this.deck = deck;
            originalDeck = deck.GetCards();
            playerCards = new List<Card>();
            computerCards = new List<Card>();
            dealerCards = deck.GetCards();
        }

        private void DealInitialCards()
        {
            playerCards.Add(dealerCards[0]);
            dealerCards.RemoveAt(0);
            playerCards.Add(dealerCards[0]);
            dealerCards.RemoveAt(0);

            computerCards.Add(dealerCards[0]);
            dealerCards.RemoveAt(0);
            computerCards.Add(dealerCards[0]);
            dealerCards.RemoveAt(0);
        }

        public void StartGame()
        {
            bool flag = true;
            while (flag)
            {
                playerCards.Clear();
                computerCards.Clear();
                dealerCards = new List<Card>(originalDeck);
                deck.Shuffle(dealerCards);
                DealInitialCards();

                bool gameOver = false;
                while (!gameOver)
                {
                    Console.Clear();

                    if (!IsGameEnded())
                    {
                        if (!PlayerTurn())
                            gameOver = true;

                        ComputerTurn();
                    }
                    else
                        gameOver = true;
                }

                GameResults();

                int choice;
                do
                {
                    Console.WriteLine("\n1) Continue playing 2) Exit");
                } while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2));
                switch (choice)
                {
                    case 1:
                        break;
                    case 2:
                        return;
                }
            }
        }

        private bool PlayerTurn()
        {
            Console.WriteLine("Your Cards:");
            DisplayHand(playerCards);
            int option;
            do
            {
                Console.WriteLine("\n1) Take 2) Stop");
            } while (!int.TryParse(Console.ReadLine(), out option) || (option != 1 && option != 2));

            bool continuePlaying = true;

            switch (option)
            {
                case 1:
                    playerCards.Add(dealerCards[0]);
                    dealerCards.RemoveAt(0);
                    break;
                case 2:
                    continuePlaying = false;
                    break;
            }

            return continuePlaying;
        }

        private void ComputerTurn()
        {
            if (GetHandValue(computerCards) < 12)
            {
                computerCards.Add(dealerCards[0]);
                dealerCards.RemoveAt(0);
            }    
        }

        private bool IsGameEnded()
        {
            bool flag;

            if (GetHandValue(computerCards) >= 21 || GetHandValue(playerCards) >= 21 || HasTwoAces(computerCards) || HasTwoAces(playerCards))
                flag = true;
            else
                flag = false;

            return flag;
        }

        private void GameResults()
        {
            Console.Clear();
            Console.WriteLine("Your Cards:");
            DisplayHand(playerCards);
            Console.WriteLine("\nComputer Cards:");
            DisplayHand(computerCards);

            int playerValue = GetHandValue(playerCards);
            int computerValue = GetHandValue(computerCards);

            if (playerValue == 21 || HasTwoAces(playerCards))
            {
                Console.WriteLine("\n - = You Win = -\n");
                wins++;
            }
            else if (computerValue == 21 || HasTwoAces(computerCards))
            {
                Console.WriteLine("\n - = Computer Wins = -\n");
                loses++;
            }
            else if (playerValue > 21)
            {
                Console.WriteLine("\n - = Computer Wins = -\n");
                loses++;
            }
            else if (computerValue > 21)
            {
                Console.WriteLine("\n - = You Win = -\n");
                wins++;
            }
            else if (playerValue > computerValue)
            {
                Console.WriteLine("\n - = You Win = -\n");
                wins++;
            }
            else if (playerValue < computerValue)
            {
                Console.WriteLine("\n - = Computer Wins = -\n");
                loses++;
            }
            else
            {
                Console.WriteLine("\n - = Draw = -\n");
                draws++;
            }

            Console.WriteLine($"Wins: {wins} | Loses: {loses} | Draws: {draws}");
        }

        private int GetHandValue(List<Card> hand)
        {
            int value = 0;

            foreach (var item in hand)
            {
                if (item.Value == Value.Six)
                    value += 6;
                else if (item.Value == Value.Seven)
                    value += 7;
                else if (item.Value == Value.Eight)
                    value += 8;
                else if (item.Value == Value.Nine)
                    value += 9;
                else if (item.Value == Value.Ten)
                    value += 10;
                else if (item.Value == Value.Jack)
                    value += 2;
                else if (item.Value == Value.Queen)
                    value += 3;
                else if (item.Value == Value.King)
                    value += 4;
                else if (item.Value == Value.Ace)
                    value += 11;
            }

            return value;
        }

        private void DisplayHand(List<Card> hand)
        {
            foreach (var item in hand)
                Console.WriteLine($"{item.Value} {item.Suit}");
            Console.WriteLine($"\nTotal hand value: {GetHandValue(hand)}");
        }

        private bool HasTwoAces(List<Card> hand)
        {
            int numAce = 0;
            bool flag = false;

            foreach (var item in hand)
            {
                if (item.Value == Value.Ace)
                    numAce++;
            }

            if (hand.Count == 2 && numAce == 2)
                flag = true;

            return flag;
        }
    }
}
