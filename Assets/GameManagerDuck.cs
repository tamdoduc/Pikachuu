using UnityEngine;
using System.Collections.Generic;


public class GameManagerDuck : MonoBehaviour
{
    public int rows = 6;
    public int cols = 6;
    public GameObject tilePrefab;
    public Transform boardContainer;
    public Sprite[] tileSprites;

    private PikachuTileDuck[,] board;
    private PikachuTileDuck firstSelected, secondSelected;

    void Start()
    {
        GenerateBoard();
    }

    void GenerateBoard()
    {
        int originalRows = rows; // Lưu số hàng thật
        int originalCols = cols; // Lưu số cột thật

        // 🔹 Tăng kích thước bảng để thêm hàng và cột viền xung quanh
        rows = originalRows + 2;
        cols = originalCols + 2;

        board = new PikachuTileDuck[rows, cols];
        List<Sprite> spriteList = new List<Sprite>();

        // Tạo danh sách sprite (2 lần mỗi loại để có cặp)
        for (int i = 0; i < (originalRows * originalCols) / 2; i++)
        {
            spriteList.Add(tileSprites[i % tileSprites.Length]);
            spriteList.Add(tileSprites[i % tileSprites.Length]);
        }

        Shuffle(spriteList);

        // 🔹 Điều chỉnh vị trí để ô Pikachu xuất hiện giữa màn hình
        float xOffset = (cols - 1) * 0.75f;
        float yOffset = (rows - 1) * -0.75f;

        int spriteIndex = 0;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                // 🔹 Nếu là viền (hàng đầu/cuối, cột đầu/cuối) => không tạo Pikachu
                if (r == 0 || r == rows - 1 || c == 0 || c == cols - 1)
                {
                    board[r, c] = null; // Viền trống
                    continue;
                }

                // 🔹 Tạo ô Pikachu ở vị trí chính xác
                GameObject obj = Instantiate(tilePrefab, boardContainer);
                obj.transform.position = new Vector3(c * 1.5f - xOffset, -r * 1.5f - yOffset, 0);

                PikachuTileDuck tileDuck = obj.GetComponent<PikachuTileDuck>();
                tileDuck.SetSprite(spriteList[spriteIndex++]);
                tileDuck.SetPosition(r, c);
                tileDuck.SetGameManager(this);
                board[r, c] = tileDuck;
            }
        }
    }


    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randIndex = Random.Range(i, list.Count);
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }

    public void SelectTile(PikachuTileDuck tileDuck)
    {
        if (firstSelected == null)
        {
            firstSelected = tileDuck;
        }
        else if (secondSelected == null)
        {
            secondSelected = tileDuck;
            CheckMatch();
        }
    }

    void CheckMatch()
    {
        if (firstSelected.sprite == secondSelected.sprite && CanConnect(firstSelected, secondSelected))
        {
            Destroy(firstSelected.gameObject);
            Destroy(secondSelected.gameObject);
            board[firstSelected.row, firstSelected.col] = null;
            board[secondSelected.row, secondSelected.col] = null;
        }
        firstSelected = null;
        secondSelected = null;
    }


    bool CanConnect(PikachuTileDuck tile1, PikachuTileDuck tile2)
    {
        if (tile1 == tile2) return false;

        int r1 = tile1.row, c1 = tile1.col;
        int r2 = tile2.row, c2 = tile2.col;

        if (IsClearPath(r1, c1, r2, c2)) return true;

        if (IsOneCornerPath(r1, c1, r2, c2)) return true;

        if (IsTwoCornerPath(r1, c1, r2, c2)) return true;

        return false;
    }

    private bool IsClearPath(int r1, int c1, int r2, int c2)
{
    // 🔹 Kiểm tra nếu chỉ số vượt ngoài phạm vi hợp lệ
    if (r1 < 0 || r1 >= rows || c1 < 0 || c1 >= cols ||
        r2 < 0 || r2 >= rows || c2 < 0 || c2 >= cols)
    {
        return false; // Không cho phép kiểm tra ngoài mảng
    }

    if (r1 == r2) // Cùng hàng
    {
        int minC = Mathf.Min(c1, c2);
        int maxC = Mathf.Max(c1, c2);
        for (int c = minC + 1; c < maxC; c++)
        {
            if (board[r1, c] != null) return false;
        }
        return true;
    }

    if (c1 == c2) // Cùng cột
    {
        int minR = Mathf.Min(r1, r2);
        int maxR = Mathf.Max(r1, r2);
        for (int r = minR + 1; r < maxR; r++)
        {
            if (board[r, c1] != null) return false;
        }
        return true;
    }

    return false;
}

    bool IsOneCornerPath(int r1, int c1, int r2, int c2)
    {
        if (board[r1, c2] == null && IsClearPath(r1, c1, r1, c2) && IsClearPath(r1, c2, r2, c2))
        {
            return true;
        }

        if (board[r2, c1] == null && IsClearPath(r1, c1, r2, c1) && IsClearPath(r2, c1, r2, c2))
        {
            return true;
        }

        return false;
    }

    private bool IsTwoCornerPath(int r1, int c1, int r2, int c2)
    {
        // 🔹 Duyệt tất cả các cột trung gian (kể cả ngoài bảng)
        for (int c = -1; c <= cols; c++)
        {
            if (IsValidCorner(r1, c) && IsValidCorner(r2, c) &&
                IsClearPath(r1, c1, r1, c) &&
                IsClearPath(r1, c, r2, c) &&
                IsClearPath(r2, c, r2, c2))
            {
                return true;
            }
        }

        // 🔹 Duyệt tất cả các hàng trung gian (kể cả ngoài bảng)
        for (int r = -1; r <= rows; r++)
        {
            if (IsValidCorner(r, c1) && IsValidCorner(r, c2) &&
                IsClearPath(r1, c1, r, c1) &&
                IsClearPath(r, c1, r, c2) &&
                IsClearPath(r, c2, r2, c2))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsValidCorner(int r, int c)
    {
        // Nếu điểm nằm ngoài bảng => hợp lệ (coi như vùng trống)
        if (r < 0 || r >= rows || c < 0 || c >= cols)
            return true; // Không truy cập mảng, chỉ coi như vùng trống

        // Nếu điểm trong bảng nhưng không có ô nào => hợp lệ
        return board[r, c] == null;
    }




}
