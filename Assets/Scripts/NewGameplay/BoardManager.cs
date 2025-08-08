using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace NewGameplay
{
    public class BoardManager : MonoBehaviour
    {
        public Transform pickedContainer;

        private List<Dictionary<Vector2Int, Cell>> _layers = new();
        public List<Dictionary<Vector2Int, Cell>> Layers => _layers;

        public List<Cell> GetAvailableCells()
        {
            var available = new List<Cell>();
            foreach (var layer in _layers)
            {
                foreach (var cell in layer.Values)
                {
                    if (cell != null && !PickedCells.Contains(cell))
                        available.Add(cell);
                }
            }
            return available;
        }

        private List<Cell> _pickedCells = new();
        public List<Cell> PickedCells => _pickedCells;

        private LevelDataSO _levelData;

        public void PickCell(Cell cell)
        {
            if (_pickedCells.Contains(cell))
            {
                BackCellToBoard(cell, 0.5f);
              
                return;
            }
            if (_pickedCells.Count >= _levelData.maxPickedCell) return;

            MoveCellToBottom(cell, 0.5f);
            _pickedCells.Add(cell);
            CheckMatches();
        }


        public Cell GetCell(int layerIndex, Vector2Int pos)
        {
            return _layers[layerIndex][pos];
        }
        public Cell GetCell(int layerIndex, int x, int y)
        {
            return GetCell(layerIndex, new Vector2Int(x, y));
        }

        private void CheckMatches()
        {
            if (_pickedCells.Count < _levelData.matchesMin) return; // Ít hơn (3) thì bỏ qua
            foreach (NormalItem.eNormalType type in System.Enum.GetValues(typeof(NormalItem.eNormalType)))
            {
                var sameTypeCells = _pickedCells.Where(c =>
                {
                    var cellItem = c.Item as NormalItem;
                    return cellItem.ItemType == type;
                }).ToList();

                if (sameTypeCells.Count >= _levelData.matchesMin)
                {
                    // GET SCORE 

                    // Animate rồi remove
                    foreach (var cell in sameTypeCells.Take(3)) 
                    {
                        if (cell == null) return;
                        cell.transform.DOScale(Vector3.zero, 0.5f)
                            .SetEase(Ease.InBack)
                            .OnComplete(() =>
                            {
                                Destroy(cell.gameObject);
                            });
                        _pickedCells.Remove(cell);
                    }
                 
                    break;
                }
            }

            if (_pickedCells.Count >= _levelData.maxPickedCell && !GameController.Instance.isTimeAttackMode)
            {
                Debug.Log("Thua");
                GameController.Instance.State = GameManager.eStateGame.GAME_OVER;
            }

            // Kiểm tra win
            if (transform.childCount == 0)
            {
                Debug.Log("thắng!");
                GameController.Instance.State = GameManager.eStateGame.WIN;
            }
        }

        public bool IsBoardCleared()
        {
            foreach (var layerDict in _layers)
            {
                if (layerDict.Count > 0)
                    return false;
            }
            return true;
        }

        public void MoveCellToBottom(Cell cell, float duration)
        {
            if (cell == null || cell.gameObject == null) return;
            float spacing = 1.0f; // Khoảng cách giữa các cell

            // Thêm cell vào container trước 
            cell.transform.SetParent(pickedContainer);

          
            int total = pickedContainer.childCount;
            float totalWidth = (total - 1) * spacing;
            Vector3 center = pickedContainer.position;
            Vector3 startPos = center - new Vector3(totalWidth / 2f, 0, 0);

            for (int i = 0; i < total; i++)
            {
                Transform child = pickedContainer.GetChild(i);
                Vector3 targetPos = startPos + new Vector3(i * spacing, 0, 0);
                child.DOMove(targetPos, duration).SetEase(Ease.InOutQuad);
            }
        }


        public void BackCellToBoard(Cell cell, float duration)
        {
            if (cell == null || cell.gameObject == null) return;

            // Thêm cell vào container trước 
            cell.transform.SetParent(transform);
            cell.transform.DOMove(cell.prevPos, duration).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                _pickedCells.Remove(cell);
                _layers[cell.Layer][new Vector2Int(cell.BoardX, cell.BoardY)] = cell;
            });
        }

        private List<NormalItem.eNormalType> GenerateShuffledItems(int totalCells)
        {
            int maxType = System.Enum.GetValues(typeof(NormalItem.eNormalType)).Length;
            List<NormalItem.eNormalType> items = new();

            int totalTriplets = totalCells / 3;

            for (int i = 0; i < totalTriplets; i++)
            {
                var type = (NormalItem.eNormalType)Random.Range(0, maxType);
                items.Add(type);
                items.Add(type);
                items.Add(type);
            }

            items.Shuffle(); // Xáo trộn thứ tự ngẫu nhiên

            return items;
        }

        public void InitBoard(LevelDataSO levelData)
        {
            _levelData = levelData;
            Clear();

            List<(int layerIndex, int rows, int cols)> layerConfigs = new();

            // Lấy giá trị max rows và max cols để làm layer dưới cùng (layer 0)
            int baseRows = levelData.randomMaxRows;
            int baseCols = levelData.randomMaxColumns;

            int totalCells = 0;

            // Số lượng layer random
            int randomLayerCount = levelData.randomLayerCount;

            // Tạo các layer từ dưới lên trên (layer 0 là dưới cùng)
            for (int i = 0; i < randomLayerCount; i++)
            {
                // Giảm dần rows và cols theo i (tăng lên thì giảm đi)
                int rows = Mathf.Max(1, baseRows - i); // ít nhất là 1 row
                int cols = Mathf.Max(1, baseCols - i);

                layerConfigs.Add((i, rows, cols));
                totalCells += rows * cols;
            }

           
            int bottomIndex = randomLayerCount;
            int bottomRows = levelData.bottomLayer.rows;
            int bottomCols = levelData.bottomLayer.columns;

            totalCells += bottomRows * bottomCols;
            layerConfigs.Add((bottomIndex, bottomRows, bottomCols));

            // Làm tròn xuống thành bội số của 3
            totalCells = (totalCells / 3) * 3;

            // Sinh danh sách item type hợp lệ
            var itemList = GenerateShuffledItems(totalCells);

            // Tạo từng layer và rải item vào
            int offset = 0;
            foreach (var (layerIndex, rows, cols) in layerConfigs)
            {
                int count = rows * cols;
                if (offset + count > itemList.Count) count = itemList.Count - offset;

                var slice = itemList.GetRange(offset, count);
                CreateLayer(layerIndex, rows, cols, slice);
                offset += count;
            }
        }


        public void Clear()
        {
            _layers.Clear();
            transform.ClearChildren();
            _pickedCells.Clear();
            pickedContainer.ClearChildren();
        }


        private void CreateLayer(int layerIndex, int rows, int cols, List<NormalItem.eNormalType> types)
        {
            var layerDict = new Dictionary<Vector2Int, Cell>();
            float cellSize = 1f;
            Vector2 offset = new Vector2((cols - 1) * cellSize / 2f, (rows - 1) * cellSize / 2f);

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int i = x * rows + y;
                    if (i >= types.Count) return;

                    var type = types[i];
                    NormalItem normalItem = new NormalItem();
                    normalItem.SetType(type);
                    string prefabPath = normalItem.GetPrefabName();

                    GameObject prefab = Resources.Load<GameObject>(prefabPath);
                    Vector3 spawnPos = new Vector3(x * cellSize - offset.x, y * cellSize - offset.y, -layerIndex);
                    GameObject go = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
                    go.name = $"{layerIndex} - ({x}, {y})";

                    SpriteRenderer iconSR = go.GetComponent<SpriteRenderer>();
                    iconSR.sortingOrder = layerIndex * 2 + 1;

                    GameObject bg = go.transform.GetChild(0).gameObject;
                    bg.SetActive(true);
                    SpriteRenderer bgSR = bg.GetComponent<SpriteRenderer>();
                    bgSR.sortingOrder = layerIndex * 2;
                    bgSR.color = NormalItem.Colors[type];

                    Cell cell = go.GetComponent<Cell>() ?? go.AddComponent<Cell>();
                    cell.Assign(normalItem);
                    cell.Setup(layerIndex, new Vector2Int(x, y));

                    layerDict[new Vector2Int(x, y)] = cell;
                }
            }

            _layers.Add(layerDict);
        }
    }
}
