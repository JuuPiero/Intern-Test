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

