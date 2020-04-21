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

		public void placeObstacles(List<Position> obstacles)
		{
			///<summary>
			/// Set up and draw the obstacles at random position.
			/// </summary>
			foreach (Position obstacle in obstacles)
			{
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.SetCursorPosition(obstacle.col, obstacle.row);
				Console.Write("="); //A method to display value, in this case "="
			}
		}

		public void drawFood(Position food)
		{
			Console.SetCursorPosition(food.col, food.row);
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("@");
		}

		public void addSnakeElements(Queue<Position> snakeElements)
		{
			for (int i = 0; i <= 3; i++)
			{
				snakeElements.Enqueue(new Position(0, i)); //add item to the list
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

		public void gameOver(Queue<Position> snakeElements, int negativePoints, int oriUserPoints)
		{
			playGameOverSound();
			Console.SetCursorPosition(55, 8);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Game over!");
			int userPoints = (snakeElements.Count - 6) * 100 - negativePoints;

			if (oriUserPoints != userPoints)
			{
				oriUserPoints = userPoints;
			}

			Console.SetCursorPosition(50, 0);
			Console.Write("Points: " + oriUserPoints);

			addToScoreboard(negativePoints, userPoints);

			//if (userPoints < 0) userPoints = 0;
			userPoints = Math.Max(userPoints, 0); //Compare userPoints to 0 & return whichever is larger
			Console.SetCursorPosition(50, 9);
			Console.WriteLine("Your points are: {0}", userPoints);

			Console.SetCursorPosition(45, 10);
			Console.WriteLine("Press enter to exit the game.");
			Console.SetCursorPosition(60, 11);
		}

		public void addToScoreboard(int negativePoints, int userPoints)
		{
			using (StreamWriter writetext = new StreamWriter("score.txt"))
			{
				writetext.WriteLine("Points: " + userPoints);
				writetext.WriteLine("Negative Points: " + negativePoints);
			}
		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		static void Main(string[] args)
		{
			///<summary>
			/// Create variables.
			/// </summary>
			byte right = 0;
			byte left = 1;
			byte down = 2;
			byte up = 3;
			int lastFoodTime = 0;
			int foodDissapearTime = 8000;
			int negativePoints = 0;

			/* Create an array of structures named directions
			and pre-define the positions that follow the 
			format of the structure named Position*/
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
			lastFoodTime = Environment.TickCount;

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
			program.playBackgroundSound();

			Console.SetCursorPosition(50, 0);
			int oriUserPoints = 0;
			Console.Write("Points: " + oriUserPoints);
			Console.SetCursorPosition(70, 0);
			Console.Write("Negative Points: " + negativePoints);

			program.placeObstacles(obstacles);

			//Create a Quesue to store elements in FIFO (first-in, first out) style
			Queue<Position> snakeElements = new Queue<Position>();
			for (int i = 0; i <= 3; i++)
			{
				snakeElements.Enqueue(new Position(0, i)); //add item to the list
			}

			///<summary>
			/// Set up and draw the food of the snake at random position.
			/// </summary>
			Position food;
			do
			{
				food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
					randomNumbersGenerator.Next(0, Console.WindowWidth));
			}
			while (snakeElements.Contains(food) || obstacles.Contains(food));

			program.drawFood(food);
			program.drawSnakeBody(snakeElements);

			///<summary>
			/// A while loop which allow the user to control the snake by using the keyboard to create input.
			/// </summary>
			while (true)
			{
				negativePoints++;

				if (Console.KeyAvailable)
				{
					ConsoleKeyInfo userInput = Console.ReadKey();
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
				}

				///<summary>
				/// Track the last position of the head of the snake.
				/// </summary>
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

				///<summary>
				/// End the game and print the score(s) of the user when the snake hit the obstacles.
				/// </summary>
				if (snakeElements.Contains(snakeNewHead) || obstacles.Contains(snakeNewHead))
				{
					program.gameOver(snakeElements, negativePoints, oriUserPoints);

					string action = Console.ReadLine();
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
				Console.SetCursorPosition(snakeNewHead.col, snakeNewHead.row);
				Console.ForegroundColor = ConsoleColor.Gray;
				program.writeDirection(direction, right, left, up, down);

				///<summary>
				/// Creation of the new food after the snake ate the previous food.
				/// </summary>
				if (snakeNewHead.col == food.col && snakeNewHead.row == food.row)
				{
					// feeding the snake
					do
					{
						food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
							randomNumbersGenerator.Next(0, Console.WindowWidth));
					}
					while (snakeElements.Contains(food) || obstacles.Contains(food));
					lastFoodTime = Environment.TickCount;
					Console.SetCursorPosition(food.col, food.row);
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write("@");
					sleepTime--;

					///<summary>
					/// Create new obstacle in new position.
					/// </summary>
					Position obstacle = new Position();
					do
					{
						obstacle = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
							randomNumbersGenerator.Next(0, Console.WindowWidth));
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
					Console.Write("=");
				}
				else
				{
					// moving...
					Position last = snakeElements.Dequeue();
					Console.SetCursorPosition(last.col, last.row);
					Console.Write(" ");
				}

				///<summary>
				/// Calculation of negative points when the snake miss the food.
				/// </summary>
				if (Environment.TickCount - lastFoodTime >= foodDissapearTime)
				{
					negativePoints = negativePoints + 50;
					Console.SetCursorPosition(70, 0);
					Console.Write("Negative Points: " + negativePoints);

					Console.SetCursorPosition(food.col, food.row);
					Console.Write(" ");
					do
					{
						food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
							randomNumbersGenerator.Next(0, Console.WindowWidth));
					}
					while (snakeElements.Contains(food) || obstacles.Contains(food));
					lastFoodTime = Environment.TickCount;
				}

				program.drawFood(food);

				sleepTime -= 0.01;

				Thread.Sleep((int)sleepTime);
			}
		}
	}
}
