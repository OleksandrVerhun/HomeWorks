//1) Згенерувати впорядковану колоду карт
//2) Перемішати колоду карт
//3) Знайти позиції всіх тузів у колоді
//4) Перемістити всі пікові карти на початок колоди
//5) Відсортувати колоду

//6) Створіть консольну програму для карткової гри «21» з простими правилами:
//a. у грі 36 карт
//b. вартість карток в балах: Туз - 11 очок, Король – 4 очки, Дама/Королева - 3 бали, Джек/Валет – 2 очки, інші карти за їх номіналом
//c. 2 гравці (ви та комп'ютер)
//d. мета гри - набрати 21 очко
//e. спочатку ви повинні ввести, хто отримує перші картки
//f. ви та комп'ютер отримуєте 2 карти відразу
//g. після цього кожен із вас вирішить, чого ви хочете? отримати ще одну карту або перестати
//   отримувати карти (залежить від того, хто першим почав гру)
//h. якщо гравець набрав 21 очко або має 2 тузи, гра закінчена і виграє цей гравенць
//i. якщо гравець набирає більше 21 очка, гра закінчується, але в кінці раунду
//j. якщо у вас обох більше 21 очка гра закінчена та виграє гравець з меншою кількістю очок
//k. має бути можливість продовжувати грати
//l. статистика за результатами всіх зіграних ігор

using System;

namespace HomeWork3;

public enum Suit
{
    Spades, // ♠
    Hearts, // ♥
    Clubs, // ♣
    Diamonds // ♦
}

public enum Value
{
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}

public class Card
{
    public Suit Suit { get; set; }
    public Value Value { get; set; }
}

public class Deck
{
    public List<Card> cards { get; set; }

    public Deck()
    {
        cards = new List<Card>();
        GenerateDeck();
    }

    private void GenerateDeck()
    {
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Value value in Enum.GetValues(typeof(Value)))
            {
                cards.Add(new Card { Suit = suit, Value = value });
            }
        }
    }

    public List<Card> GetCards() => cards;

    public void Shuffle(List<Card> cards)
    {
        Random random = new Random();
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            Card card = cards[k];
            cards[k] = cards[n];
            cards[n] = card;
        }
    }

    public void PrintDeck()
    {
        int i = 0;
        foreach (var item in cards)
        {
            Console.WriteLine($"{i}) {item.Value} {item.Suit}");
            i++;
        }
    }

    public void SortDeck()
    {
        cards.Sort((card1, card2) =>
        {
            int suitComparison = card1.Suit.CompareTo(card2.Suit);
            if (suitComparison == 0)
            {
                return card1.Value.CompareTo(card2.Value);
            }
            else
            {
                return suitComparison;
            }
        });
    }

    public List<int> GetAcePositions()
    {
        List<int> acePositions = new List<int>();

        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].Value == Value.Ace)
            {
                acePositions.Add(i);
            }
        }

        return acePositions;
    }

    public void MoveSpadesToBeginning()
    {
        List<Card> spades = new List<Card>();
        List<Card> otherCards = new List<Card>();

        foreach (var card in cards)
        {
            if (card.Suit == Suit.Spades)
                spades.Add(card);
            else
                otherCards.Add(card);
        }

        cards.Clear();
        cards.AddRange(spades);
        cards.AddRange(otherCards);
    }
}

public class Program
{
    public static void Menu()
    {
        bool flag = true;
        while (flag)
        {
            try
            {
                Thread.Sleep(1000);
                Console.Clear();
                Console.WriteLine("1) Start new game\n2) Inspection of methods\n0) Exit ");
                Console.Write("\nSelect the option: ");
                int item = Convert.ToInt32(Console.ReadLine());
                switch (item)
                {
                    case 1:
                        Deck deck = new Deck();
                        BlackJack jack = new BlackJack(deck);
                        jack.StartGame();
                        break;
                    case 2: // Зробив для цього окремий кейс, тому що НЕ всі методи використовуються у грі
                        Console.Clear();
                        Console.WriteLine("Inspection of methods");

                        Console.WriteLine("\nCreating a deck and displaying all cards");
                        Deck testDeck = new Deck();
                        testDeck.PrintDeck();

                        Console.WriteLine("\nShuffle the deck");
                        testDeck.Shuffle(testDeck.cards);
                        testDeck.PrintDeck();

                        Console.WriteLine("\nFind ace positions");
                        foreach (var acePos in testDeck.GetAcePositions())
                        {
                            Console.Write($"{acePos} ");
                        }

                        Console.WriteLine("\n\nMove all spade cards to the beginning of the deck");
                        testDeck.MoveSpadesToBeginning();
                        testDeck.PrintDeck();

                        Console.WriteLine("\nDeck sorting");
                        testDeck.SortDeck();
                        testDeck.PrintDeck();

                        Console.WriteLine("\nPress any button to continue...");
                        Console.ReadKey();
                        break;
                    case 0:
                        Console.WriteLine("\nThank you for playing!");
                        return;
                    default:
                        throw new Exception("Incorrect input! Try again...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public static void Main()
    {
        Menu();
    }
}