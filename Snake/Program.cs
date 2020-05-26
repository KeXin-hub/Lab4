using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Media;
using System.IO;
/* Above are the namespace declaration used in the program.
This is used so a fully qualified name does not need to be
specified every time that a method that is contained within 
is used. */

//The Snake Namespace
namespace Snake
{
	/// <summary>
	///  Create a structure named Position with public modifier
	/// </summary>
	struct Position
	{
		/// <summary>
		/// Create variables.
		/// </summary>
		public int row;
		public int col;
		/// <summary>
		/// Pass-by-value constructor with row and col as the parameters for initialization.
		/// </summary>
		/// <param name="row">Row.</param>
		/// <param name="col">Col.</param>
		public Position(int row, int col)
		{
			this.row = row;
			this.col = col;
		}
	}

	//Declare class named Program with only one method called Main
	class Program
	{
		public void helpMenu()
		{
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Help Menu");
			Console.WriteLine("Instruction:");
			Console.WriteLine("1.Press the arrow key on the keyboard to control the direction of the snake.");
			Console.WriteLine("2.Eat the food to increase your score.");
			Console.WriteLine("3.Prevent the snake from hitting the obsatcles or its body.");
			Console.WriteLine("Press enter to start the game.");
			string startGame = Console.ReadLine();
			Console.Clear();
		}
		public void playBackgroundSound()
		{
			SoundPlayer bgSound = new SoundPlayer("../../Resources/POL-azure-waters-short.wav");
			bgSound.PlayLooping();
		}

		public void playGameOverSound()
		{
			SoundPlayer bgSound = new SoundPlayer("../../Resources/Death.wav");
			bgSound.Play();
		}

		public void playWinSound()
		{
			SoundPlayer bgSound = new SoundPlayer("../../Resources/Ta Da.wav");
			bgSound.Play();
		}

		public void placeObstacles(List<Position> obstacles)
		{
			///<summary>
			/// Set up and draw the obstacles at random position.
			/// </summary>
			foreach (Position obstacle in obstacles)
			{
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.SetCursorPosition(obstacle.col, obstacle.row);
				Console.Write("▒");
			}
		}

		public void drawFood(Position food)
		{
			Console.SetCursorPosition(food.col, food.row);
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Console.Write("♥♥");
		}

		public void drawSpecialFood(Position bonusfood, Random randomNumbersGenerator, int eatenTimes)
		{
			if (eatenTimes == 2)
			{
				Console.SetCursorPosition(bonusfood.col, bonusfood.row);
				Console.ForegroundColor = ConsoleColor.Magenta;
				Console.Write("%");
			}
		}

		public void addSnakeElements(Queue<Position> snakeElements)
		{
			for (int i = 0; i <= 3; i++)
			{
				snakeElements.Enqueue(new Position(4, i)); //add item to the list
			}
		}

		public void drawSnakeBody(Queue<Position> snakeElements)
		{
			///<summary>
			/// A foreach loop which is used to draw the body of the snake.
			/// </summary>
			foreach (Position position in snakeElements)
			{
				Console.SetCursorPosition(position.col, position.row);
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write("*");
			}
		}

		public void writeDirection(int direction, byte right, byte left, byte up, byte down)
		{
			if (direction == right) Console.Write(">");
			if (direction == left) Console.Write("<");
			if (direction == up) Console.Write("^");
			if (direction == down) Console.Write("v");
		}

		public void gameOver(Queue<Position> snakeElements, int negativePoints, int lvlNum, int foodDissapearTime, int eatenTimes, int bonus)
		{
			playGameOverSound();
			Console.SetCursorPosition(55, 8);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Game over!");
			int userPoints = (snakeElements.Count - 6) * 100 - negativePoints + bonus;

			using (FileStream fs = new FileStream("score.txt", FileMode.Append, FileAccess.Write))
			using (StreamWriter writetext = new StreamWriter(fs))
			{
				writetext.WriteLine("YOU LOSE!");
				writetext.WriteLine("Game Level: " + lvlNum);
				writetext.WriteLine("Food Eaten Times: " + eatenTimes);
				writetext.WriteLine("Points: " + userPoints);
				writetext.WriteLine("Negative Points: " + negativePoints);
				writetext.WriteLine("---------------------------------------");
			}

			//if (userPoints < 0) userPoints = 0;
			//userPoints = Math.Max(userPoints, 0); //Compare userPoints to 0 & return whichever is larger
			Console.SetCursorPosition(50, 9);
			Console.WriteLine("Your points are: {0}", userPoints);

			if (negativePoints >= 200)
			{
				Console.SetCursorPosition(43, 10);
				Console.WriteLine("Your Negative Points is too HIGH!");
			}

			Console.SetCursorPosition(43, 11);
			Console.WriteLine("Press ENTER to display ScoreBoard.");
			Console.SetCursorPosition(60, 11);

			Console.ReadLine();

			Console.Clear();
			scoreBoard();
			//Console.WriteLine("Press ENTER to quit game");

			//string action = Console.ReadLine(); //Enter key pressed, exit game
			//if (action == "")
			//{
			//	Environment.Exit(0);
			//}
		}

		public void winGame(int negativePoints, int eatenTimes, int lvlNum, Queue<Position> snakeElements, int bonus)
		{
			if (eatenTimes == 5)
			{
				playWinSound();
				Console.SetCursorPosition(55, 8);
				Console.ForegroundColor = ConsoleColor.Blue;
				Console.WriteLine("You Win!");
				Console.SetCursorPosition(35, 10);
				Console.WriteLine("Congratulation! You've reached Level 5 & food eaten time is 5");
				Console.SetCursorPosition(50, 12);
				Console.WriteLine("Press ENTER to display ScoreBoard.");
				int userPoints = (snakeElements.Count - 6) * 100 - negativePoints + bonus;

				using (FileStream fs = new FileStream("score.txt", FileMode.Append, FileAccess.Write))
				using (StreamWriter writetext = new StreamWriter(fs))
				{
					writetext.WriteLine("YOU WIN!");
					writetext.WriteLine("Game Level: " + lvlNum);
					writetext.WriteLine("Food Eaten Times: " + eatenTimes);
					writetext.WriteLine("Points: " + userPoints);
					writetext.WriteLine("Negative Points: " + negativePoints);
					writetext.WriteLine("---------------------------------------");
				}

				Console.ReadLine();

				Console.Clear();
				scoreBoard();
				//Console.WriteLine("Press ENTER to quit game");

				//string action = Console.ReadLine(); //Enter key pressed, exit game
				//if (action == "")
				//{
				//	Environment.Exit(0);
				//}

			}
		}

		public void scoreBoard()
		{
			Console.Clear();
			Console.SetCursorPosition(0, 1);
			Console.WriteLine("SCOREBOARD");
			Console.WriteLine("-----------------------------");
			using (StreamReader score = new StreamReader("score.txt"))
			{
				string line;
				while ((line = score.ReadLine()) != null)
				{
					Console.WriteLine(line);
				}
			}
			Console.WriteLine("-----------------------------");
		}

		public void displayScore(Queue<Position> snakeElements, int negativePoints, int bonus)
		{
			int userPoints = (snakeElements.Count - 6) * 100 - negativePoints + bonus;
			Console.SetCursorPosition(5, 0);
			Console.Write("User Points: " + userPoints + "  ");
		}

		public void displayLevel(int lvlNum)
		{
			Console.SetCursorPosition(50, 0);
			string lvl = "Level " + lvlNum;
			Console.Write(lvl);
		}
	
		public void createBoundary()
		{
			int width = Console.BufferWidth;
			Console.SetCursorPosition(0, 2);
			for (int i = 1; i <= width; i++)
			{
				Console.Write("-");
			}
		}

		public void Game()
		{
			///<summary>
			/// Create variables.
			/// </summary>
			byte right = 0;
			byte left = 1;
			byte down = 2;
			byte up = 3;
			int lastFoodTime = 0;
			int foodDissapearTime = 10000;
			int negativePoints = 0;
			int eatenTimes = 0;
			int lvlNum = 1;
			int bonus = 0;

			/* Create an array of structures named directions and pre-define the positions that follow the format of the structure named Position*/
			Position[] directions = new Position[]
			{
				new Position(0, 1), // right
                new Position(0, -1), // left
                new Position(1, 0), // down
                new Position(-1, 0), // up
            };
			double sleepTime = 100; //Stores numbers with decimal
			int direction = right;
			Random randomNumbersGenerator = new Random();
			Console.BufferHeight = Console.WindowHeight;
			lastFoodTime = Environment.TickCount; //Timer

			///<summary>
			/// Create a list named obstacles to store the position of the obstacles.
			/// </summary>
			List<Position> obstacles = new List<Position>()
			{
				new Position(12, 12),
				new Position(14, 20),
				new Position(7, 7),
				new Position(19, 19),
				new Position(6, 9),
			};

			Program program = new Program();
			program.helpMenu();
			program.playBackgroundSound();

			program.placeObstacles(obstacles);

			//Create a Queue to store elements in FIFO (first-in, first out) style
			Queue<Position> snakeElements = new Queue<Position>();
			program.addSnakeElements(snakeElements);

			program.displayLevel(lvlNum);
			program.displayScore(snakeElements, negativePoints, bonus);

			///<summary>
			/// Set up and draw the food of the snake at random position.
			/// </summary>
			Position food;
			do
			{
				food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
									randomNumbersGenerator.Next(4, Console.WindowWidth));
			}
			while (snakeElements.Contains(food) || obstacles.Contains(food));

			Position bonusfood;
			do
			{
				bonusfood = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
					randomNumbersGenerator.Next(4, Console.WindowWidth));
			}
			while (snakeElements.Contains(bonusfood) || obstacles.Contains(bonusfood));

			program.drawFood(food);
			program.drawSpecialFood(bonusfood, randomNumbersGenerator, eatenTimes);
			program.drawSnakeBody(snakeElements);

			///<summary>
			/// A while loop which allow the user to control the snake by using the keyboard to create input.
			/// </summary>
			while (true)
			{
				//negativePoints++;

				//if (Console.KeyAvailable)
				//{
				//	ConsoleKeyInfo userInput = Console.ReadKey();
				//	if (userInput.Key == ConsoleKey.LeftArrow)
				//	{
				//		if (direction != right) direction = left;
				//	}
				//	if (userInput.Key == ConsoleKey.RightArrow)
				//	{
				//		if (direction != left) direction = right;
				//	}
				//	if (userInput.Key == ConsoleKey.UpArrow)
				//	{
				//		if (direction != down) direction = up;
				//	}
				//	if (userInput.Key == ConsoleKey.DownArrow)
				//	{
				//		if (direction != up) direction = down;
				//	}
				//}

				//snakeHead is the last element of snakeElements
				Position snakeHead = snakeElements.Last();

				///<summary>
				/// Identify the direction of the snake.
				/// </summary>
				Position nextDirection = directions[direction];

				///<summary>
				/// Manage the length of the snake.
				/// </summary>
				Position snakeNewHead = new Position(snakeHead.row + nextDirection.row,
					snakeHead.col + nextDirection.col);

				if (snakeNewHead.col < 0) snakeNewHead.col = Console.WindowWidth - 1;
				if (snakeNewHead.row < 0) snakeNewHead.row = Console.WindowHeight - 1;
				if (snakeNewHead.row >= Console.WindowHeight) snakeNewHead.row = 0;
				if (snakeNewHead.col >= Console.WindowWidth) snakeNewHead.col = 0;

				program.createBoundary();
				if ((snakeHead.row == 4) || (snakeNewHead.row == 4))
				{
					if (Console.KeyAvailable)
					{
						ConsoleKeyInfo userInput = new ConsoleKeyInfo();
						do
						{
							if (Console.KeyAvailable == false) { }

							userInput = Console.ReadKey(true);
							if (userInput.Key == ConsoleKey.LeftArrow)
							{
								if (direction != right) direction = left;
							}
							if (userInput.Key == ConsoleKey.RightArrow)
							{
								if (direction != left) direction = right;
							}
							if (userInput.Key == ConsoleKey.DownArrow)
							{
								if (direction != up) direction = down;
							}
						} while (userInput.Key != ConsoleKey.LeftArrow && userInput.Key != ConsoleKey.RightArrow && userInput.Key != ConsoleKey.DownArrow);
					}
					else { }
				}
				else {
					if (Console.KeyAvailable)
					{
						ConsoleKeyInfo userInput = new ConsoleKeyInfo();
						do
						{
							if (Console.KeyAvailable == false) { }

							userInput = Console.ReadKey(true);
							if (userInput.Key == ConsoleKey.LeftArrow)
							{
								if (direction != right) direction = left;
							}
							if (userInput.Key == ConsoleKey.RightArrow)
							{
								if (direction != left) direction = right;
							}
							if (userInput.Key == ConsoleKey.UpArrow)
							{
								if (direction != down) direction = up;
							}
							if (userInput.Key == ConsoleKey.DownArrow)
							{
								if (direction != up) direction = down;
							}

						} while (userInput.Key != ConsoleKey.LeftArrow && userInput.Key != ConsoleKey.RightArrow &&
								userInput.Key != ConsoleKey.UpArrow && userInput.Key != ConsoleKey.DownArrow);
					}
				}
				///<summary>
				/// End the game and print the score(s) of the user when the snake hit the obstacles.
				/// </summary>
				if (snakeElements.Contains(snakeNewHead) || obstacles.Contains(snakeNewHead) || (snakeHead.row < 2) || (snakeNewHead.row < 2) || (snakeHead.row == Console.WindowHeight) || (snakeNewHead.row == Console.WindowHeight))
				{
					program.gameOver(snakeElements, negativePoints, lvlNum, foodDissapearTime, eatenTimes, bonus);

					string action = Console.ReadLine(); //Enter key pressed, exit game
					if (action == "")
					{
						return;
					}
				}

				///<summary>
				/// Draw the body of the snake.
				/// </summary>
				Console.SetCursorPosition(snakeHead.col, snakeHead.row);
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write("*");

				///<summary>
				/// Direction of the head of the snake when the user changes the direction of the snake.
				/// </summary>
				snakeElements.Enqueue(snakeNewHead);
				program.displayLevel(lvlNum);
				program.displayScore(snakeElements, negativePoints, bonus);

				Console.SetCursorPosition(snakeNewHead.col, snakeNewHead.row);
				Console.ForegroundColor = ConsoleColor.Gray;

				program.writeDirection(direction, right, left, up, down);

				///<summary>
				/// Creation of the new food after the snake ate the previous food.
				/// </summary>
				if ((snakeNewHead.col == food.col && snakeNewHead.row == food.row) || (snakeNewHead.col == food.col + 1 && snakeNewHead.row == food.row))
				{
					Console.SetCursorPosition(food.col, food.row);
					Console.Write("  ");

					eatenTimes++;

					if (eatenTimes == 2)
					{
						lvlNum++;
						program.displayLevel(lvlNum);
						foodDissapearTime = 8000;
					}

					if (eatenTimes == 3)
					{
						lvlNum++;
						program.displayLevel(lvlNum);
						foodDissapearTime = 7000;
					}

					if (eatenTimes == 4)
					{
						lvlNum++;
						program.displayLevel(lvlNum);
						foodDissapearTime = 6000;
					}

					if (eatenTimes == 5)
					{
						lvlNum++;
						program.displayLevel(lvlNum);
					}

					// feeding the snake
					//find new position for food
					do
					{
						food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
							randomNumbersGenerator.Next(4, Console.WindowWidth));
					}
					while (snakeElements.Contains(food) || obstacles.Contains(food));

					program.winGame(negativePoints, eatenTimes, lvlNum, snakeElements, bonus);

					program.drawFood(food);
					program.drawSpecialFood(bonusfood, randomNumbersGenerator, eatenTimes);
					lastFoodTime = Environment.TickCount;
					sleepTime--;

					///<summary>
					/// Create new obstacle in new position.
					/// </summary>
					Position obstacle = new Position();
					do
					{
						obstacle = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
							randomNumbersGenerator.Next(4, Console.WindowWidth));
					}

					///<summary>
					/// Draw the obstacle on the game screen in random position.
					/// </summary>
					while (snakeElements.Contains(obstacle) ||
						obstacles.Contains(obstacle) ||
						(food.row != obstacle.row && food.col != obstacle.row));
					obstacles.Add(obstacle);
					Console.SetCursorPosition(obstacle.col, obstacle.row);
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write("▒");
				}
				else if (snakeNewHead.col == bonusfood.col && snakeNewHead.row == bonusfood.row)
				{
					eatenTimes++;
					bonus += 100;

					//program.winGame(negativePoints, eatenTimes, snakeElements, bonus);

					lastFoodTime = Environment.TickCount;
					sleepTime--;

					///<summary>
					/// Create new obstacle in new position.
					/// </summary>
					Position obstacle = new Position();
					do
					{
						obstacle = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
							randomNumbersGenerator.Next(4, Console.WindowWidth));
					}

					///<summary>
					/// Draw the obstacle on the game screen in random position.
					/// </summary>
					while (snakeElements.Contains(obstacle) ||
						obstacles.Contains(obstacle) ||
						(food.row != obstacle.row && food.col != obstacle.row));
					obstacles.Add(obstacle);
					Console.SetCursorPosition(obstacle.col, obstacle.row);
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write("▒");
				}
				else
				{
					// moving...
					Position last = snakeElements.Dequeue();
					Console.SetCursorPosition(last.col, last.row);
					Console.Write(" ");
					program.displayScore(snakeElements, negativePoints, bonus);
				}

				///<summary>
				/// Calculation of negative points when the snake miss the food.
				/// </summary>
				if (Environment.TickCount - lastFoodTime >= foodDissapearTime)
				{
					negativePoints = negativePoints + 50;

					if (negativePoints >= 200)
					{
						program.gameOver(snakeElements, negativePoints, lvlNum, foodDissapearTime, eatenTimes, bonus);
					}

					Console.SetCursorPosition(food.col, food.row);
					Console.Write("  ");
					do
					{
						food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
							randomNumbersGenerator.Next(4, Console.WindowWidth));
					}
					while (snakeElements.Contains(food) || obstacles.Contains(food));
					lastFoodTime = Environment.TickCount;
				}

				program.drawFood(food);

				sleepTime -= 0.01;

				Thread.Sleep((int)sleepTime);
			}
		}

		//Defines the Main method
		static void Main(string[] args)
		{
			while (true)
			{
				Program game = new Program();

				game.Game();

				Console.WriteLine("Press any key to restart, press Esc to close.");
				var userInput = Console.ReadKey();
				if (userInput.Key == ConsoleKey.Escape)
				{
					return;
				}
				Console.Clear();
			}
		}

	}
}
