This section provides brief instructions on how the GamingConsole program can be executed.

## To Run the Program
The system currently includes two games: Notakto and Gomoku. When entering Grid Games, you will be asked to select which game you would like to play, Notakto or Gomoku. If you have a saved game for your selection, you will be asked if you would like to continue. If you select no, or do not have a saved game you will then be asked if you are versing another human or computer.

Before the game starts, you will be provided with instructions on how to play and the winning conditions. You will then be required to confirm you are ready to continue and play the game. Once started, you will be promoted with the possible options these include, making a move, undo a move, redoing a move, saving the game or exiting the game. When you select make a move you will be prompted to enter your move, the specific input required will vary between games.

This loop will continue until one player has won, or if applicable there is no more valid moves, and the result is a draw. At this point, you will be able to exit the program or return back to the main menu to start another game.


## To Add New Games
This platform was specifically designed to be extensible, allowing for additional grid-games to be added as required. To create a new game, you will need to create a new concrete class of ‘Game’. This class will need to define all required methods, for example, ‘DispayBoard’ based on the specific board size and quantity, and ‘MakeMove’ to have the logic for a human players move. Additionally, the ‘Program’ class will need to be updated to include the new game in the selection logic.

## Design Principles and Patterns
The design principles and patterns were carefully selected when designing and creating the gaming console. The principles and patterns were chosen to ensure that the gaming console would be able to easily accommodate additional games in the future. The most relevant design principles and patterns are detailed below:

### 1. Single Responsibility Principle (SRP)
When designing the program, it was ensured that each class would have a single and clearly established responsibility. To exemplify, the ‘Game’ class is responsible for defining the generic structure of the concrete games classes, whilst ‘Gomoku’ is responsible for the specific rules, conditions and aspects of the Gomoku game. By utilising the single responsibility principle, it ensured that all classes were maintainable, and easily understandable.

### 2. Keep It Simple, Stupid (KISS)
The Keep It Simple, Stupid, or alternatively known as KISS principle was utilised to ensure that the program did not have any unnecessary complexity that made it difficult to understand. This was especially important due to the extensibility of the program, as if it was overly complicated, it would be difficult to add additional grid-based games. By making the code straightforward, it allows different developers to add their own games without confusion.

### 3. You Aren’t Gonna Need It (YAGNI)
Following on from KISS, the You Aren’t Gonna Need It principle (or YAGNI), was utilised to ensure that only code that was required to run the program and meet the specified requirements was implemented. By excluding unnecessary features, it ensures that only required functionality was added. This not only improves the quality of the code due to less polluting, but also saves time from writing unnecessary code. An example of unnecessary code for this program would be the ability to support non-grid-based games.

### 4. Template Method
Template method was utilised by the ‘Game’ class, as the majority of methods were defined there, however, the specific implementation of those methods was provided by the subclasses: ‘Gomoku’ and ‘Notakto’. This pattern was chosen to be implemented as it allowed for the gaming console to be extensible and add additional grid-based games as required. This design method can be identified in the class diagram where the specific game classes inherit the methods from the parent class ‘Game’.

### 5. Memento Pattern
The ‘GameManagement’ class implements a Memento pattern to handle undo and redo functionalities. The design pattern was essential for the core game functionalities as it allowed the ‘GameManagement’ class to store the state of the game at certain points. This allows the game to be restored to a previous state when an undo or redo action is performed.

### 6. Flyweight Pattern
The Flyweight pattern was used by the ‘Notakto’ and ‘Gomoku’ class when each game placed character ‘X’ markers on the board. Implementing this pattern minimises unnecessary memory usage for cells that have the same character. This pattern was applicable for our code as we had a large number of game markers, ‘X’. Utilising the flyweight pattern improves memory efficiency, leading to overall improvement in game performance.

These principles and patterns ensure that the system is easy to extend, efficient, and maintainable as new games are added.

## Diagrams
### Class Diagram
This diagram illustrates the relationship between the Game class and its subclasses, showing the inheritance structure and key method implementations.
![image](https://github.com/user-attachments/assets/116e397e-bbb4-401f-bda9-97debdcca00f)


### Object Diagram
Scenario: This diagram captures the Gomoku game loop on a 15x15 board. The user, Joseph (a human player), has chosen to play against a computer player. The diagram shows the state of the board after both Joseph and the computer have completed their first turn, which constitutes one loop. The diagram then reflects that it is now Joseph's turn again.
![image](https://github.com/user-attachments/assets/794e2175-bc60-488b-b087-bbea1ceff7b5)
Note: The board is represented as a 15x15 array for simplicity. The actual game board is a full 15x15 grid.

### Sequence Diagram
Scenario: This diagram captures the first game loop of Gomoku on a 15x15 board, where the user (a human player) has chosen to play against a computer player. The user initiates the game, causing the Game Management class to prompt the user to make their first move, followed by the computer making its first move. The sequence then continues as part of the game loop, with interactions between the game system and the players.
![image](https://github.com/user-attachments/assets/1e5395e2-e266-4ed7-b819-40c1576af457)
