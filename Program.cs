using System;
using System.Collections.Generic;
using System.Threading;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 2;
int width = Console.WindowWidth - 6;
int playerposX = 0;
int playerposY = 0;
int food = 0;
int foodposX = 0;
int foodposY = 0;
string[] states = {"<'_'>", "[:)]", "<X_X>"};
string[] foods = {"@@@@", "$$$$", "####"};
string player = states[0];
bool shouldExit = false;


InitializeGame();
while (!shouldExit) 
{
    if (TerminalResized()) 
    {
        Console.Clear();
        Console.Write("Console was resized. Program exiting.");
        shouldExit = true;
    } 
    else 
    {
        if (PlayerIsFaster()) 
        {
            Move(2, false);
        } 
        else if (PlayerIsSick()) 
        {
            FreezePlayer();
        } else 
        {
            Move(otherKeysExit: false);
        }
        if (GotFood())
        {
            ChangePlayer();
            ShowFood();
        }
    }
}

bool TerminalResized() 
{
    return height != Console.WindowHeight - 2 || width != Console.WindowWidth - 6;
}

void ShowFood() 
{
    food = random.Next(0, foods.Length);
    foodposX = random.Next(0, width - player.Length);
    foodposY = random.Next(0, height - 2);
    Console.SetCursorPosition(foodposX, foodposY);
    Console.Write(foods[food]);
}

bool GotFood() 
{
    return playerposY == foodposY && playerposX == foodposX;
}

bool PlayerIsSick() 
{
    return player.Equals(states[2]);
}

bool PlayerIsFaster() 
{
    return player.Equals(states[1]);
}

void ChangePlayer() 
{
    player = states[food];
    Console.SetCursorPosition(playerposX, playerposY);
    Console.Write(player);
}

void FreezePlayer() 
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
}

void Move(int speed = 3, bool otherKeysExit = false) 
{
    int lastX = playerposX;
    int lastY = playerposY;
    
    switch (Console.ReadKey(true).Key) {
        case ConsoleKey.UpArrow:
            playerposY--; 
            break;
		case ConsoleKey.DownArrow: 
            playerposY++; 
            break;
		case ConsoleKey.LeftArrow:  
            playerposX -= speed; 
            break;
		case ConsoleKey.RightArrow: 
            playerposX += speed; 
            break;
		case ConsoleKey.Escape:     
            shouldExit = true; 
            break;
        default:
            shouldExit = otherKeysExit;
            break;
    }

    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++) 
    {
        Console.Write(" ");
    }

    playerposX = (playerposX < 0) ? 0 : (playerposX >= width ? width : playerposX);
    playerposY = (playerposY < 0) ? 0 : (playerposY >= height ? height : playerposY);
    Console.SetCursorPosition(playerposX, playerposY);
    Console.Write(player);
}

void InitializeGame() 
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}