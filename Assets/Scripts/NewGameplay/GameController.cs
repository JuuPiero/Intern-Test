using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NewGameplay
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        [SerializeField] private BoardManager _boardManager;
        [SerializeField] private LevelDataSO _levelData;

        private GameManager.eStateGame _state;
        public GameManager.eStateGame State
        {
            get => _state;
            set
            {
                _state = value;
                OnGameStateChanged?.Invoke(_state);
            }
        }
        public event Action<GameManager.eStateGame> OnGameStateChanged;

        [SerializeField] private Camera mainCamera;
        public float timeRemaining;

        public bool isTimeAttackMode = false;


        private void Awake()
        {
            mainCamera = Camera.main;
            Instance = this;
            State = GameManager.eStateGame.SETUP;
        }


        public void LoadLevel()
        {
            if (_levelData == null) return;
            _boardManager.InitBoard(_levelData);
            isTimeAttackMode = false;
        }

        public void PlayTimeAttackMode()
        {
            LoadLevel();
            timeRemaining = _levelData.timeLimit;
            StartCoroutine(Countdown());
            isTimeAttackMode = true;
        }


        public void Update()
        {
            if (State == GameManager.eStateGame.GAME_OVER) return;
            if (State == GameManager.eStateGame.WIN) return;


            if (Input.GetMouseButtonDown(0))
            {
                var hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    Collider2D collider = hit.collider;
                    Cell cell = collider.GetComponentInParent<Cell>();
                    _boardManager.PickCell(cell);
                }
            }
        }

        private IEnumerator Countdown()
        {
            while (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                yield return null;
            }

            State = GameManager.eStateGame.GAME_OVER;
        }
        public void AutoWin()
        {
            StartCoroutine(AutoWinCoroutine());
        }

        public void AutoLose()
        {
            StartCoroutine(AutoLoseCoroutine());
        }

        public IEnumerator AutoWinCoroutine()
        {
            while (true)
            {
                var allCells = _boardManager.transform.GetChildren<Cell>();
                if (allCells.Count == 0)
                {
                    yield break;
                }

                Cell temp = allCells[0];
                var type = (temp.Item as NormalItem).ItemType;

                int count = 0;
                List<Cell> sameTypeCells = new List<Cell>();

                // Tìm 3 cell cùng loại
                foreach (var cell in allCells)
                {
                    var cellType = (cell.Item as NormalItem).ItemType;
                    if (cellType.Equals(type))
                    {
                        sameTypeCells.Add(cell);
                        count++;
                        if (count == 3)
                            break;
                    }
                }

                if (count < 3)
                {
                    // // Không tìm đủ 3 cell cùng loại -> khó win
                    // Debug.LogWarning("Không tìm đủ 3 cell cùng loại để ăn tiếp. Dừng auto.");
                    yield break;
                }

                // Pick 3 cell cùng loại
                foreach (var cell in sameTypeCells)
                {
                    _boardManager.PickCell(cell);
                    yield return new WaitForSeconds(0.5f); // Đợi animation move + clear
                }

                yield return null;
            }
        }

        public IEnumerator AutoLoseCoroutine()
        {
            // Lấy tất cả cell hiện còn trên board (trừ đã pick)
            List<Cell> cellsToPick = new List<Cell>();
            foreach (var layerDict in _boardManager.Layers)
            {
                foreach (var cell in layerDict.Values)
                {
                    if (!_boardManager.PickedCells.Contains(cell))
                        cellsToPick.Add(cell);
                }
            }

            // Chọn liên tục cho đến khi bottom đầy (MAX_CELL_PICKED)
            foreach (var cell in cellsToPick)
            {
                if (_boardManager.PickedCells.Count >= _levelData.maxPickedCell)
                {
                    yield break;
                }

                _boardManager.PickCell(cell);
                yield return new WaitForSeconds(0.6f);
            }
        }

     
    }
}