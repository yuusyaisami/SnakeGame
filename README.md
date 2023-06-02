
# introduction  
The game of snake has a long history, and its original game appeared in Blockade in 1976.  
I first played this game on the Family Computer and was fascinated by its simplicity.  
It is an overhead viewpoint and the player controls a snake.  
The player must maneuver the snake so that it does not hit the walls and the body of the snake. The snake's body grows as it eats its food, so the more the Score grows, the more difficult the game becomes.  
こちらは日本語のReadMeです<a href="ReadMe_JPN">Click here to read in Japanese.</a> 

# Game Operation and Code
Movement is done with WASDkey, and in MenuScene, pressing EnterKey starts the Game.  
You can't change the direction of movement 180 degrees, because behind the snake is the body.  
If you want to change the direction of movement 180 degrees, you must change it by 90 degrees.  
<p align="left">  
  <img src="https://user-images.githubusercontent.com/110176625/242879581-6d58800d-8d45-4906-b2ef-c9180abca3df.gif" / >  
</p>  
The main code of the game is written in program.cs, and the game consists of  
The game consists of four functions: init() update() Controller() draw().  
The init function initializes the initial values of the game.  
The update function performs the general processing of the game.  
The controller function reads the keyboard input status.  
The draw function displays the game screen on the console.  
  
If you want to modify the game code, rewrite it in visual studio and build it.  
# end  
Games are more fun to make than to play.  
