using System.Collections.Generic;
using UnityEngine;

namespace ThangVN.LevelPikachu
{


    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;

        private PathFind pathFind;
        private List<int> listValues = new List<int>();

        [SerializeField] private DataPikachuChar dataPikachuChar;
        [SerializeField] private Block blockPrefab;
        [SerializeField] private List<Block> listBlock = new List<Block>();
        [SerializeField] private List<Block> listBlockAll = new List<Block>();
        [SerializeField] private int row, col;
        [SerializeField] private float blockSizeY = 0.9f;
        [SerializeField] private float blockSizeX = 0.73f;
        [SerializeField] private int totalDiff = 16;
        [SerializeField] private int max = 15;
        public List<int> totalValues = new List<int>();

        public LineControllerPool lineControllerPool;
        public int[,] grid;
        private void Awake()
        {
            /*ManagerEvent.RegEvent(EventCMD.EVENT_SHUFFLE, Shuffle);
            ManagerEvent.RegEvent(EventCMD.EVENT_HINT, Hint);*/
            Instance = this;
        }

        private void Start()
        {
            row = 7;
            col = 15;
            InitGrid();
            pathFind = new PathFind(grid);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        Debug.Log($"grid{i},{j}: " + grid[i, j]);
                    }

                    Debug.Log("______________________");
                }

            }

        }

        const float zOffSize = -0.01f;

        public Block GetBlock(int row, int col)
        {
            foreach (Block block in listBlockAll)
            {
                if (block.row == row && block.col == col)
                {
                    return block;
                }
            }
            return null;
        }

        private void InitGrid()
        {
            int start = 0;

            var offSetY = (((float)row - 1) / 2.0f) * blockSizeY;
            var offsetX = (((float)col - 1) / 2.0f) * blockSizeX;

            grid = new int[row, col];

            Vector3 startPosition = new Vector3(-offsetX, offSetY, 0);

            for (int x = 0; x < row; x++)
            {
                for (int y = 0; y < col; y++)
                {
                    float z = zOffSize * start;
                    start++;
                    Vector3 position = new Vector3(y * blockSizeX, -x * blockSizeY, z) + startPosition;

                    Block block = Instantiate(blockPrefab, transform);
                    block.transform.localPosition = position;
                    block.Init(x, y);
                    block.name = $"Block[{x},{y}]";

                    if (x == 0 || y == 0 || x == row - 1 || y == col - 1)
                    {
                        grid[x, y] = 0;
                        block.gameObject.SetActive(false);
                        listBlockAll.Add(block);
                    }
                    else
                    {
                        block.gameObject.SetActive(true);
                        listBlock.Add(block);
                        listBlockAll.Add(block);
                    }
                }
            }

            SpawnValueBlockUI();
        }

        private void SpawnValueBlockUI()
        {
            int[] arr = new int[totalDiff];

            List<Block> listSpawnBlock = new List<Block>();

            for (int i = 0; i < listBlock.Count; i++)
            {
                listSpawnBlock.Add(listBlock[i]);
            }

            while (listSpawnBlock.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, totalDiff);

                if (arr[index] < max)
                {
                    arr[index] += 2;

                    for (int y = 0; y < 2; y++)
                    {
                        int r = UnityEngine.Random.Range(0, listSpawnBlock.Count);
                        listValues.Add(index + 1);
                        listSpawnBlock.RemoveAt(r);
                    }
                }
            }

            Shuffle(listValues);
            Show();
            OnlyCheckSpawnBug();
        }

        private void Shuffle(List<int> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                int newValue = list[k];
                list[k] = list[n];
                list[n] = newValue;
            }
        }

        public void Show()
        {
            for (int i = 0; i < listBlock.Count; i++)
            {
                int x = listBlock[i].row;
                int y = listBlock[i].col;
                int value = listValues[i];
                Sprite sprite = dataPikachuChar.data[value - 1].sprite;
                grid[x, y] = value;
                totalValues.Add(value);
                listBlock[i].InitValue(value, sprite);
            }
        }


        public void OnlyCheckSpawnBug()
        {
            Dictionary<int, int> listCountDictionary = new Dictionary<int, int>();

            for (int i = 0; i < totalDiff; i++)
            {
                for (int j = 0; j < listValues.Count; j++)
                {
                    if (listValues[j] != i) continue;

                    if (listCountDictionary.ContainsKey(i))
                    {
                        listCountDictionary[i]++;
                    }
                    else
                    {
                        listCountDictionary.Add(i, 1);
                    }
                }
            }

            foreach (var obj in listCountDictionary)
            {
                if (obj.Value % 2 != 0)
                {
                    Debug.LogError("Fuck Spawn Loi!!!");
                }
            }
        }

        public bool CheckTwoBlocks(Block p1, Block p2)
        {
            List<Block> path = pathFind.FindPath(p1, p2);
            if (path != null)
            {
                DrawPath(path);
                return true;
            }
            return false;
        }

        public void ClearBlock(Block p)
        {
            grid[p.row, p.col] = 0;

            totalValues.Remove(p.id);
            if (totalValues.Count <= 0)/* ManagerEvent.RaiseEvent(EventCMD.EVENT_WIN)*/  Debug.Log("WIn");
        }

        public void DrawPath(List<Block> path)
        {
            List<Vector3> positions = new List<Vector3>();
            foreach (Block block in path)
            {
                Debug.Log(block.name);
                Vector3 position = block.transform.position;
                positions.Add(position);
            }

            LineController lineController = lineControllerPool.GetLineController();
            lineController.DrawPath(positions);
        }

        public void Shuffle(object e)
        {
            if (totalValues.Count <= 2) return;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (grid[i, j] == 0) continue;

                    int randomIndex = UnityEngine.Random.Range(0, totalValues.Count);
                    grid[i, j] = totalValues[randomIndex];
                    totalValues.RemoveAt(randomIndex);
                }
            }

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (grid[i, j] != 0)
                    {
                        totalValues.Add(grid[i, j]);
                    }
                }
            }

            int current = totalValues[0];

            if (totalValues[1] != current)
            {
                int indexToSwap = -1;

                for (int i = 2; i < totalValues.Count; i++)
                {
                    if (totalValues[i] == current)
                    {
                        indexToSwap = i;
                        break;
                    }
                }

                if (indexToSwap != -1)
                {
                    int temp = totalValues[1];
                    totalValues[1] = totalValues[indexToSwap];
                    totalValues[indexToSwap] = temp;
                }
            }

            int index = 0;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (grid[i, j] == 0) continue;

                    int value = totalValues[index];
                    grid[i, j] = value;

                    for (int k = 0; k < listBlock.Count; k++)
                    {
                        if (listBlock[k].row == i && listBlock[k].col == j)
                        {
                            Sprite sprite = dataPikachuChar.data[value - 1].sprite;
                            listBlock[k].InitValue(grid[i, j], sprite);
                        }
                    }

                    index++;
                }
            }
        }

        [SerializeField] private List<Block> listHint = new List<Block>();

        public void RefreshListHint()
        {
            foreach (Block block in listHint)
            {
                block.StopHint();
            }
            listHint.Clear();
        }

        public void Hint(object e)
        {
            (Block p1, Block p2) = GetHint();

            if (p1 != null && p2 != null)
            {
                Debug.Log($"{p1.name} + {p2.name}");

                listHint.Add(p1);
                listHint.Add(p2);

                p1.SetHint();
                p2.SetHint();
            }
            else
            {
                Debug.Log("Không tìm thấy cặp ô nào thỏa mãn");
            }
        }

        public (Block, Block) GetHint()
        {
            if (listHint.Count > 0) return (null, null);

            List<(Block, Block)> validPairs = new List<(Block, Block)>();

            for (int i = 1; i < row - 1; i++)
            {
                for (int j = 1; j < col - 1; j++)
                {
                    for (int k = i; k < row - 1; k++)
                    {
                        for (int l = j; l < col - 1; l++)
                        {
                            if (i == k && j == l) continue;

                            Block p1 = GetBlock(i, j);
                            Block p2 = GetBlock(k, l);

                            if (p1 != null && p2 != null && p1.gameObject.activeSelf && p2.gameObject.activeSelf && pathFind.FindPath(p1, p2) != null)
                            {
                                validPairs.Add((p1, p2));
                            }
                        }
                    }
                }
            }

            if (validPairs.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, validPairs.Count);
                return validPairs[randomIndex];
            }

            return (null, null);
        }
    }
}
