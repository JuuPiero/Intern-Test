# Unity Test - Winter Wolf - IEC Games

## 1. General Information
- **Candidate:** Hoang Huu Thanh
- **Position Applied:** Intern Unity Developer

---

## 2. Task List and Work Details

### Task 1: Re-skin
- **Task Description:** Change the skin of the provided assets as required.
- **Work Done:**  
  - Duplicated the existing prefabs and replaced the `SpriteRenderer` sprites with new fish sprites.  
  - Updated file paths in the `Constants` class to point to the new prefab locations:  
    ```csharp
    public const string PREFAB_NORMAL_TYPE_ONE = "prefabs/Fish/itemNormal01";
    public const string PREFAB_NORMAL_TYPE_TWO = "prefabs/Fish/itemNormal02";
    public const string PREFAB_NORMAL_TYPE_THREE = "prefabs/Fish/itemNormal03";
    ...
    ```
- **Result:**  
  - All reskinned prefabs now display the new fish sprites correctly in the game.  
  - Code references updated to ensure the correct prefabs are loaded.

---

### Task 2: Change Gameplay
- **Task Description:** Change the current gameplay to a new mechanic where players tap items on a multi-layered board to move them down to a separate bottom area. When exactly three identical items gather in the bottom cells, they are cleared. The player wins by clearing the entire board and loses if the bottom cells become full before clearing the board.
- **Work Done:**  
  -Redesigned the board as a multi-layer structure, using a list of dictionaries (`List<Dictionary<Vector2Int, Cell>>`) to represent stacked layers.
  - Implemented `InitBoard` method to initialize multiple board layers with randomly assigned item types (`NormalItem.eNormalType`).
  - Developed cell picking logic allowing players to tap on items to move them down to a dedicated `pickedContainer` at the bottom, with smooth animations handled by DOTween.
  - Created `CheckMatches` function to detect and clear groups of exactly three identical items in the bottom cells, removing them with animations and updating the player's score.
  - Added win condition checks (`IsBoardCleared`) and lose condition checks (if bottom cells reach max capacity).
  - Fixed sprite sorting order to ensure backgrounds do not overlap icons incorrectly, setting backgrounds with lower sorting order than item icons.
  - Calculated cell spawn positions with proper offsets to center the board horizontally and vertically on screen.
  - Developed an auto-play coroutine for automated testing of picking and clearing mechanics, simulating gameplay to validate functionality.
- **Result:**  
  - The new gameplay mechanic with multi-layered board and bottom cell matching is fully functional and visually coherent.
  - The player can interact by tapping cells to move items down, clear matches of three, and either win or lose based on the game state.
  - Animations and UI layering provide smooth and clear visual feedback.
  - Code structure allows easy future expansion or modification of gameplay features.
---

### Task 3: Improve the Gameplay
- **Task Description:** 
  - Ensure the initial board contains all types of fish.  
  - Add animations for moving items from the board to bottom cells and clearing identical items (scale to zero).  
  - Add a "Time Attack" button on the Home Screen to start a separate game mode.  
  - In Time Attack mode:  
  - Player does not lose when bottom cells are filled.  
  - Player can return an item from a bottom cell back to its original position on the board by tapping it.  
  - Player loses if the board is not cleared within 1 minute.  
- **Work Done:**  
  - Modified board initialization logic to guarantee that every fish type appears at least once at the start of the game.  
  - Implemented smooth DOTween animations for:  
  - Moving items from board to bottom cells.  
  - Clearing matched identical items by scaling them down to zero before removal.  
  - Added a "Time Attack" button in the Home Screen UI to allow starting this mode.  
  - Adjusted gameplay rules for Time Attack mode:  
  - Disabled lose condition triggered by bottom cells filling up.  
  - Enabled tapping on bottom cells to return items back to their original board positions.  
  - Implemented a 60-second countdown timer that ends the game if the board is not cleared in time.  
- **Result:**  
  - Time Attack mode successfully integrated with dedicated start button on Home Screen UI.
  - Lose condition on bottom cells filling is disabled in Time Attack mode, aligning with new gameplay rules.
  - Item return functionality implemented, allowing users to revert moves by tapping bottom cells.
  - 60-second countdown timer enforces time limit, triggering loss if board is uncleared within duration.
  - Overall system stability maintained with no performance regressions observed during tests.
---

