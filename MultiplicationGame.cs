using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplicationGame
{
    //This Class establishes and defines player stats
    //and contains methods that directly effect player stats
    class Player
    {
        public int Health { get; set; }
        public int Armour { get; set; }
        public bool HasDivineShield { get; set; }
        public int Experience { get; set; }
        public int Points { get; set; }
        public int Skips { get; set; }
        public int TurnsCorrect { get; set; }
        public int CorrectStreak { get; set; }
        public double ExperienceGained { get; set; }
        public double PointsGained { get; set; }
        public int Turn { get; set; }
        public double LostHealth { get; set; }
        public int LostArmour { get; set; }
        public double LostPoints { get; set; }
        public string DisplayStats { get; set; }
        public int maxRetries = 3;
        public int retryAttemps = 0;

        public string input;
        public int maxHealth = 100;

        //Establishes basic stats for the player
        public Player()
        {
            Turn = 0;
            Health = 100;
            Armour = 0;
            HasDivineShield = false;
            TurnsCorrect = 0;
            CorrectStreak = 0;
            Skips = 0;
            Experience = 0;
            Points = 0;
            TurnsCorrect = 0;
            retryAttemps = 0;
            CorrectStreak = 0;
        }

        //Display turn number, armour, health, points, experience, and whether or not the player has the power up DivineShield all on new lines
        public string UserStats(int turn, int armour, int health, int points, int experience, bool hasDivineShield)
        {
            if (armour > 0)
            {
                DisplayStats = String.Join(Environment.NewLine, $"Turn: {turn}", $"Armour: {armour}", $"Health: {health}", $"Points: {points}", $"Experience: {experience}", $"Divine Shield: {hasDivineShield}");
            }
            else
            {
                DisplayStats = String.Join(Environment.NewLine, $"Turn: {turn}", $"Health: {health}", $"Points: {points}", $"Experience: {experience}", $"Divine Shield: {hasDivineShield}");
            }
            return DisplayStats;
        }

        //Called when powerup is chosen. Increasing the maximum health and taking away experience.
        public void AddToMaxHealth(int ammoutToAdd, int experienceCost)
        {
            maxHealth += ammoutToAdd;
            Experience -= experienceCost;
        }

        //Called when powerup is chosen or after answering correctly. Gives player health, takes away experience.
        public void AddToHealth(int amountToAdd, int experienceCost)
        {
            Health += amountToAdd;
            //Makes sure health doesn't go over max health
            if (Health > maxHealth)
            {
                //Adds armour for each point over max health is healed
                Armour += Health - maxHealth;
                Health = maxHealth;
            }
            Experience -= experienceCost;
        }

        //Called when powerup is chosen. Gives player divine shield by setting the bool to true and then subtracting the experience.
        public void ActivateDivineShield(int experienceCost)
        {
            HasDivineShield = true;
            Experience -= experienceCost;
        }

        //Called when powerup is chosen. Gives player the skip powerup that allows them to skip a question then subtracts experience.
        public void AddSkips(int amountToAdd, int expierienceCost)
        {
            Skips += amountToAdd;
            Experience -= expierienceCost;
        }

        //Called when player gets a question wrong with divine shield. Prevents them from losing health, armour, and points.
        public void UseDivineShield(double difference)
        {
            string reply;
            HasDivineShield = false;
            reply = $"Oh, your answer was {difference} off. But, you had Divine Shield so you lost no health and no points.";
            ++TurnsCorrect;
            ++CorrectStreak;
            Console.WriteLine(reply);
        }

        public void AddPointsGained(int Answer, int correctStreak)
        {
            PointsGained = Math.Round(Convert.ToDouble(Answer) / 25 + (correctStreak * 1.25));
            Points += Convert.ToInt32(PointsGained);
        }

        //Calculates how much experience a correct answer earns.
        public void AddExperienceGained(int Answer, double PointsGained)
        {
            //Size of the answer / | Points gained that turn - curent player health | Plus 8 Plus (CorrectStreak times 1.5)
            ExperienceGained = Math.Round(Convert.ToDouble(Answer) / Math.Abs(PointsGained - Health) + 8 + (CorrectStreak * 1.5));
            Experience += Convert.ToInt32(ExperienceGained);
        }

        //Ends players streak if they get and answer wrong
        public void EndStreak()
        {
            CorrectStreak = 0;
        }

        //Adds to turns per question asked
        public void AddTurn(int ammountToAdd)
        {
            Turn += ammountToAdd;
        }

        //Method to calculate and subtract loss.
        public void SubtractHealthLost(double difference, int armour, int health)
        {
            LostArmour = 0;
            //Divide how far their answer was from being correct by 4 until it is less then their max health to prevent instant 1 shot.
            LostHealth = Convert.ToInt64(difference / 4);
            while (LostHealth >= maxHealth)
            {
                //Breaks out of loop if answer is obsurdly off.
                if (difference >= 2000)
                {
                    break;
                }
                LostHealth = Math.Round(LostHealth / 4);
            }

            //Makes sure is doesnt subtract more than current health.
            if (LostHealth > health)
            {
                LostHealth = health;
            }

            //If player has no armour subract normally.
            if (armour == 0)
            {
                Health -= Convert.ToInt16(Math.Round(LostHealth));
            }
            //If player has armour subract health taking into acount that armour = 5 points of health.
            else if (armour > 0)
            {
                //If lost health is less than 5, player still loses 1 armour.
                if (LostHealth < 5)
                {
                    LostHealth = Math.Round(LostHealth / 2);
                    LostArmour = 1;
                }
                //Subtracts so that armour is equal to 5 health.
                else
                {
                    if (LostHealth > (armour * 5))
                    {
                        LostHealth -= (armour * 5);
                        LostArmour = armour;
                    }
                    else if (LostHealth <= (armour * 5))
                    {
                        LostHealth = Math.Round(LostHealth / 5) * 5;
                        LostArmour = Convert.ToInt16(LostHealth) / 5;
                        LostHealth = 0;
                    }
                }
                Health -= Convert.ToInt16(LostHealth);
                Armour -= LostArmour;
            }
        }

        //Calculates how many points a player loses.
        public void SubtractPointsLost(int points, double difference, int correctStreak)
        {
            if (points > 0)
            {
                LostPoints = Convert.ToInt64(Math.Round((points * 0.15f) - correctStreak) + (difference * 0.05f));
                Points = Convert.ToInt32(Math.Round(Points - LostPoints));

                if (LostPoints > points)
                {
                    LostPoints = points - 1;
                }
            }
            else
            {
                LostPoints = 0;
            }

            if (LostPoints > points)
            {
                LostPoints = points - 1;
            }

            if (points < 1)
            {
                Points = 0;
            }
        }

        //Adds to turn correct.
        public void AddToTurnsCorrect(int ammountToAdd)
        {
            TurnsCorrect += ammountToAdd;
        }
    }

    //This Class contains all the methods and variables to calculate the problem, the difference of answer and player input, display powerup options, etc.
    //Things that don't neccessarily directly relate to the player
    class GameBackEnd : Player
    {
        public string ProblemQuestion { get; set; }
        public int Answer { get; private set; }
        public double Difference { get; set; }
        public bool ItsPowerUpTime { get; set; }
        public string reply;
        public int minNumber1 = 1;
        public int minNumber2 = 1;
        public int maxNumber1 = 11;
        public int maxNumber2 = 100;
        public bool validResponse = true;
        public string[] alphabet = new string[26] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        public string[] alphaYN = new string[24] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "z" };
        public string[] specialCharacters = new string[33] { "+", "-", "*", "/", "!", "@", "#", "$", "%", "^", "&", "(", ")", "_", "=", "{", "}", "[", "]", "\n", "|", ";", ":", "\"", "'", "<", ">", ",", ".", "?", " ", "`", "~" };
        public string[] numbers = new string[9] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        //This method is what generates and determains the multiplication problem
        public string GenerateMultiplicationProblem(int minNumber1, int maxNumber1, int minNumber2, int maxNumber2)
        {
            //Generate a random number
            Random numberGenerator = new Random();
            //Range given variables so its easy to increase the difficulty of the multiplication.
            int num01 = numberGenerator.Next(minNumber1, maxNumber1);
            int num02 = numberGenerator.Next(minNumber2, maxNumber2);
            Answer = num01 * num02;
            //If squaring numbers
            if (num01 == num02)
            {
                ProblemQuestion = $"What is {num01} squared?";
            }
            else
            {
                ProblemQuestion = $"What is {num01} multiplied by {num02}?";
            }
            //Returns what muliplication problem will be asked.
            return ProblemQuestion;
        }

        //Takes player input and translates it into what powerup they chose.
        public void PowerUpChoice(string input, bool hasDivineShield)
        {
            if (input == "1" && Experience >= 100)
            {
                AddSkips(1, 100);
                ItsPowerUpTime = false;
                reply = $"Skips: {Skips}";
            }
            else if (input == "2" && Experience >= 100)
            {
                AddToHealth(4, 100);
                ItsPowerUpTime = false;
                reply = "Increased health by 4";
            }
            else if (input == "3" && Experience >= 100)
            {
                if (!hasDivineShield)
                {
                    ActivateDivineShield(100);
                    ItsPowerUpTime = false;
                    reply = "Divine Shield Activated";
                }
                else
                {
                    reply = "Divine Shield is already active";
                }
            }
            else if (input == "4" && Experience >= 200)
            {
                AddToMaxHealth(25, 200);
                ItsPowerUpTime = false;
                reply = $"Max Health is now {maxHealth}";
            }
            else if (input == "5" && Experience >= 300)
            {
                AddSkips(5, 300);
                ItsPowerUpTime = false;
                reply = $"Skips: {Skips}";
            }
            else if (input == "6" && Experience >= 500)
            {
                AddToHealth(50, 500);
                ItsPowerUpTime = false;
                reply = $"Health: {Health} Armour: {Armour}";
            }
            else if (input == "7")
            {
                ItsPowerUpTime = false;
                reply = "";
            }
            else if (input == "4" && Experience < 200)
            {
                reply = "You do not have enough experience.";
            }
            else if (input == "5" && Experience < 300)
            {
                reply = "You do not have enough experience.";
            }
            else if (input == "6" && Experience < 500)
            {
                reply = "You do not have enough experience.";
            }
            else if (input != "1" && input != "2" && input != "3" && input != "4" && input != "5" && input != "6" && input != "7")
            {
                reply = "Please enter a valid choice.";
            }
            Console.WriteLine(reply);
        }

        //Calculates how far the player was from the answer.
        public void CalculateDifference(int Answer, string input)
        {
            //Makes sure it doesn't crash/it can properly convert the input to an interger (is a problem if a character is input)
            bool success = Int64.TryParse(input, out _);

            if (success)
            {
                Difference = Math.Abs(Convert.ToInt64(input) - Answer);
            }
            //If it cannot convert the input to an interger, retry input.
            else
            {
                Console.WriteLine();
                Console.WriteLine("Please enter a valid response");
                Console.WriteLine(ProblemQuestion);
                Console.WriteLine();
                validResponse = false;
            }
        }

        //Called to increase difficulty every X turns by increasing the range of possible numbers
        public void IncreaseDifficulty(int turnInterval)
        {
            if (Turn % turnInterval == 0)
            {
                ++minNumber1;
                ++minNumber2;
                maxNumber1 += 3;
                maxNumber2 += 3;
            }
        }

        //Lists the possible powerups.
        public string PowerUps(int experience, int turn)
        {
            string powerUps;
            if (experience >= 100 && turn % 10 == 0)
            {
                ItsPowerUpTime = true;
                powerUps = String.Join(Environment.NewLine,
                    $"Experience: {experience}",
                    "",
                    "Choose:",
                    "(1) +1 Skip (skips the question and counts as a correct answer. To use: type \"skip\"). Cost: 100",
                    "(2) +4 Health. Cost: 100",
                    "(3) Divine Shield (allows you to get a question wrong with no negative effects. Does not stack). Cost 100",
                    "(4) +25 Max Health. Cost: 200",
                    "(5) +5 Skips. Cost: 300",
                    "(6) +50 Health. Cost: 500",
                    "(7) Skip this upgrade.",
                    "");
                return powerUps;
            }
            else
            {
                ItsPowerUpTime = false;
                return null;
            }
        }

        //Randomly chooses a message to congratulate user for correct response and inform them of point/experience changes. 
        public string Congrats(double experienceGain, double pointGain)
        {
            Random numberGenerator = new Random();
            string reply;
            int responsindex = numberGenerator.Next(1, 4);
            switch (responsindex)
            {
                case 1:
                    {
                        if (pointGain == 1f)
                        {
                            reply = String.Join(Environment.NewLine, $@"Congrats, your answer was correct. Plus {pointGain} point and you gain 1 health.",
                            $"You gain {experienceGain} experience.");
                        }
                        else
                        {
                            reply = String.Join(Environment.NewLine, $@"Congrats, your answer was correct. Plus {pointGain} points and you gain 1 health.",
                            $"You gain {experienceGain} experience.");
                        }
                        return reply;
                    }
                case 2:
                    {
                        if (pointGain == 1f)
                        {
                            reply = String.Join(Environment.NewLine, $@"Correct! Plus 1 Point and 1 health.",
                            $"You gain {experienceGain} experience.");
                        }
                        else
                        {
                            reply = String.Join(Environment.NewLine, $@"Correct! Plus {pointGain} Points and 1 health.",
                            $"You gain {experienceGain} experience.");
                            Console.WriteLine();
                        }
                        return reply;
                    }
                case 3:
                    {
                        if (pointGain == 1f)
                        {
                            reply = String.Join(Environment.NewLine, $"Spot on! Plus 1 Point and 1 health.",
                            $"You gain {experienceGain} experience.");
                        }
                        else
                        {
                            reply = String.Join(Environment.NewLine, $@"Spot on! Plus {pointGain} Points and 1 health.",
                            $"You gain {experienceGain} experience.");
                        }
                        return reply;
                    }

                default:
                    {
                        throw new Exception("Unexspected Case");
                    }
            }
        }

        //Randomly chooses response for when the player gets the question wrong
        public string Failed(double difference, double lostPoints, double lostHealth, int lostArmour)
        {
            Random numberGenerator = new Random();
            string reply;
            int responseIndex2 = numberGenerator.Next(1, 4);
            switch (responseIndex2)
            {
                case 1:
                    {
                        if (lostPoints == 1)
                        {
                            reply = $"Nope! You were {difference} off. Minus 1 point! Minus {lostHealth} health and {lostArmour} armour!";
                        }
                        else
                        {
                            reply = $"Nope! You were {difference} off. Minus {lostPoints} points! Minus {lostHealth} health and {lostArmour} armour!";
                        }
                        return reply;
                    }
                case 2:
                    {
                        if (lostPoints == 1)
                        {
                            reply = $"Wrong! You were {difference} off. Minus 1 point, and you lost {lostHealth} health and {lostArmour} armour!";
                        }
                        else
                        {
                            reply = $"Wrong! You were {difference} off. Say goodbye to {lostPoints} points, and you lost {lostHealth} health and {lostArmour} armour";
                        }
                        return reply;
                    }
                case 3:
                    {
                        if (lostPoints == 1)
                        {
                            reply = $"You were {difference} off. You lose 1 point, {lostHealth} health, and {lostArmour} armour :(";
                        }
                        else
                        {
                            reply = $"You were {difference} off. You lose {lostPoints} points, {lostHealth} health, and {lostArmour} armour :(";
                        }
                        return reply;
                    }

                default:
                    {
                        throw new Exception("Unexpected Case");
                    }
            }
        }

        //Randomly choses response for when the player gets the question wrong after theyve input an incorrect character
        public string FailedRetry(double lostPoints, double lostHealth, int lostArmour)
        {
            Random numberGenerator = new Random();
            string reply;
            int responseIndex2 = numberGenerator.Next(1, 4);
            switch (responseIndex2)
            {
                case 1:
                    {
                        if (LostPoints == 1)
                        {
                            reply = $"Invalid response 3 times in a row. Minus 1 point! Minus {lostHealth} health and {lostArmour} armour!";
                        }
                        else
                        {
                            reply = $"Invalid response 3 times in a row. Minus {lostPoints} points! Minus {lostHealth} health and {lostArmour} armour!";
                        }
                        return reply;
                    }
                case 2:
                    {
                        if (LostPoints == 1)
                        {
                            reply = $"Nope, invalid. Minus 1 point, and you lost {lostHealth} health and {lostArmour} armour!";
                        }
                        else
                        {
                            reply = $"Nope, invalid. Say goodbye to {lostPoints} points, and you lost {lostHealth} health and {lostArmour} armour";
                        }
                        return reply;
                    }
                case 3:
                    {
                        if (LostPoints == 1)
                        {
                            reply = $"Wasted. You lose 1 point, {LostHealth} health, and {LostArmour} armour :(";
                        }
                        else
                        {
                            reply = $"Wasted. You lose {LostPoints} points, {LostHealth} health, and {LostArmour} armour :(";
                        }
                        return reply;
                    }

                default:
                    {
                        throw new Exception("Unexpected Case");
                    }
            }
        }
    }

    //Main Program
    class Program
    {
        public static void Main()
        {
            bool replayGame = false;
            //Do loop that allows the player to replay the game after death
            do
            {
                replayGame = false;

                Player player = new Player();
                GameBackEnd GBE = new GameBackEnd();
                //Do loop that allows the program to move on to the next question after an answered properly
                do
                {
                    //Resets retryAttemps to 0
                    player.retryAttemps = 0;
                    //Increase turn
                    player.AddTurn(1);
                    //Increases difficulty every 5 turns
                    GBE.IncreaseDifficulty(5);
                    //Displays the users stats every turn
                    Console.WriteLine(player.UserStats(player.Turn, player.Armour, player.Health, player.Points, player.Experience, player.HasDivineShield));
                    Console.WriteLine();

                    //Generates and prints problem to console
                    GBE.GenerateMultiplicationProblem(GBE.minNumber1, GBE.maxNumber1, GBE.minNumber2, GBE.maxNumber2);
                    //Prints question generated
                    Console.WriteLine(GBE.ProblemQuestion);
                    Console.WriteLine(GBE.Answer);
                    Console.WriteLine();

                    bool validResponse = true;
                    //Do loop that allows the player to input 3 invalid answers before their answer is registered as wrong.
                    //This prevents errors and loops when and invalid input is entered and gives players a chance
                    do
                    {
                        //Waits for players input
                        //If they answer correctly
                        player.input = Console.ReadLine();
                        if (player.input == Convert.ToString(GBE.Answer))
                        {
                            validResponse = true;
                            player.AddToTurnsCorrect(1);
                            player.AddPointsGained(GBE.Answer, GBE.CorrectStreak);
                            player.AddExperienceGained(GBE.Answer, GBE.PointsGained);
                            player.AddToHealth(1, 0);
                            Console.WriteLine();
                            Console.WriteLine(GBE.Congrats(player.ExperienceGained, player.PointsGained));
                            Console.WriteLine();
                            //if they have over 100 experience and every 10 turns then askes if the player wants to buy a power up.
                            GBE.PowerUps(player.Experience, player.Turn);
                            if (GBE.ItsPowerUpTime)
                            {
                                //If user doesnt put in an input that correlates to a powerup, ask again until they skip or chose a valid powerup.
                                while (GBE.ItsPowerUpTime)
                                {
                                    Console.WriteLine(GBE.PowerUps(player.Experience, player.Turn));
                                    string powerUpChoice = Console.ReadLine();
                                    Console.WriteLine();
                                    GBE.PowerUpChoice(powerUpChoice, player.HasDivineShield);
                                    Console.WriteLine();
                                }
                            }
                        }
                        //If they choose to use a skip they aquired from powerup.
                        else if (player.input.ToLower() == "skip")
                        {
                            //Makes sure they have skips to use
                            if (player.Skips >= 1)
                            {
                                player.AddToHealth(1, 0);
                                player.AddPointsGained(GBE.Answer, player.TurnsCorrect);
                                player.AddExperienceGained(GBE.Answer, player.PointsGained);
                                player.AddSkips(-1, 0);
                                break;
                            }
                            //If they dont have skips then it is seen as an invalid response and they get 2 more retries.
                            else
                            {
                                ++player.retryAttemps;
                                validResponse = false;

                                Console.WriteLine();
                                Console.WriteLine("You have no skips availible");
                                Console.WriteLine(GBE.ProblemQuestion);
                                Console.WriteLine();
                                continue;
                            }
                        }
                        //If answer is wrong and doesn't contain any invalid characters
                        else if (player.input != Convert.ToString(GBE.Answer) && !GBE.alphabet.Any(Convert.ToString(player.input).ToLower().Contains) && !GBE.specialCharacters.Any(Convert.ToString(player.input).ToLower().Contains) && player.input != "" && player.input.ToLower() != "skip")
                        {
                            //Reset retry attempts to 0
                            player.retryAttemps = 0;
                            //Names it a valid response to leave the loop
                            validResponse = true;

                            //Checks if they have divine shield
                            if (!player.HasDivineShield)
                            {
                                player.EndStreak();
                                GBE.CalculateDifference(GBE.Answer, player.input);
                                player.SubtractHealthLost(GBE.Difference, player.Armour, player.Health);
                                player.SubtractPointsLost(player.Points, GBE.Difference, player.CorrectStreak);
                                Console.WriteLine();
                                Console.WriteLine(GBE.Failed(GBE.Difference, player.LostPoints, player.LostHealth, player.LostArmour));
                                Console.WriteLine();
                            }
                            else if (player.HasDivineShield)
                            {
                                GBE.CalculateDifference(GBE.Answer, player.input);
                                Console.WriteLine();
                                player.UseDivineShield(GBE.Difference);
                                Console.WriteLine();
                            }
                            break;
                        }
                        //If invalid response
                        else
                        {
                            ++player.retryAttemps;
                            validResponse = false;

                            //And they havent reached maximum amount of retries
                            if (player.retryAttemps < player.maxRetries)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Please enter a valid response");
                                Console.WriteLine(GBE.ProblemQuestion);
                                Console.WriteLine();
                            }
                            //If they've reached the maximum amount of retries
                            else if (player.retryAttemps == player.maxRetries)
                            {
                                //reset validResponse
                                validResponse = true;
                                player.retryAttemps = 0;

                                //Checks if they have divine shield and deals with it accordingly
                                if (!player.HasDivineShield)
                                {
                                    GBE.Difference = 500;
                                    player.EndStreak();
                                    player.SubtractHealthLost(GBE.Difference, player.Armour, player.Health);
                                    player.SubtractPointsLost(player.Points, GBE.Difference, player.CorrectStreak);
                                    Console.WriteLine();
                                    Console.WriteLine(GBE.FailedRetry(player.LostPoints, player.LostHealth, player.LostArmour));
                                    Console.WriteLine();
                                }
                                else if (player.HasDivineShield)
                                {
                                    Console.WriteLine();
                                    player.UseDivineShield(GBE.Difference);
                                    Console.WriteLine();
                                }
                                break;
                            }
                        }
                    }
                    //Repeats the question if they did not enter a valid response and have not reached the maximum amount of retries
                    while (!validResponse && player.retryAttemps <= player.maxRetries);
                }
                //Checks each turn if they have enough health to continue
                while (player.Health > 0);

                player.Health = 0;
                //Displays game over message
                Console.WriteLine();
                Console.WriteLine("Game Over :(");
                Console.WriteLine("You finished with " + player.Points + " points.");
                Console.WriteLine("Press any key to continue.");
                Console.WriteLine();
                //After key press clear console
                Console.ReadKey();
                Console.Clear();
                //Ask if they want to replay
                Console.WriteLine("Would you like to replay the game from the beginning? (Y)es/(N)o");
                Console.WriteLine();

                bool validResponseYN = true;
                //Loops if they have dont put in y/yes or n/no
                do
                {
                    string replayInput = Console.ReadLine();
                    //If not a valid response ask for them to put in a valid response
                    if (!validResponseYN)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Invalid response. Please enter either 'Y' or 'N' only.");
                        Console.WriteLine();
                    }
                    //If they input y or yes then clear console and restart
                    if (replayInput.ToLower() == "y" || replayInput.ToLower() == "yes")
                    {
                        replayGame = true;
                        Console.Clear();
                        break;
                    }
                    //If they input n or no then close the game entirely
                    else if (replayInput.ToLower() == "n" || replayInput.ToLower() == "no")
                    {
                        Environment.Exit(0);
                    }
                    //If they put in an invalid input, changes variable to loop until they input a valid response
                    else
                    {
                        validResponseYN = false;
                    }
                }
                while (!validResponseYN);
            }
            while (replayGame == true);
        }
    }
}