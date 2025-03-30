using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DevDuck;
using DG.Tweening;
using Unity.VisualScripting.FullSerializer;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum GAME_STATE
{
    EASY,NORMAL,HARD
    
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject tilePrefab;
    public Sprite[] pikachuSprites;
    public int rows, cols;
    private GameObject[,] grid;
    public LineRenderer lineRenderer;
    [Header("Sound Effects")] 
    public AudioClip matchSound;
    public AudioClip mismatchSound;
    private AudioSource audioSource;
    
    [Header("Hint Settings")]
    public float hintHighlightDuration = 1.5f;
    public float blinkInterval = 0.3f;

    float spacingX, spacingY;

    [HideInInspector] public Tile lastHintedTile1, lastHintedTile2;
    private Coroutine hintCoroutine;
    public float offsetYAxis;
    [Header("Attribute : ")] public float currentTime, maxTime;
    public ParticleSystem smokeParticle1, smokeParticle2;
    [HideInInspector] public int totalPiece;
    public int shuffleAmount, hintAmount;
    public int coin, score;
    public GameUiManager gameUiManager;
     public bool isCanPlay, isCanSelect, isEndGame;
    public UiWinLose UiWinLose;

    public EffectGetCoinRemake  effectGetCoinRemake;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    { 
        currentTime  = maxTime;
        SetData();
        totalPiece = rows * cols;
        gameUiManager.AnimStartGame();
        DOVirtual.DelayedCall(1.5f,(() =>GenerateBoard() )) ;
    }

    private void SetData()
    {
        ManagerGame.TIME_SCALE = 1;
        coin = PlayerPrefs.GetInt(PlayerPrefsManager.Coin);
        hintAmount = PlayerPrefs.GetInt(PlayerPrefsManager.Hint);
        shuffleAmount = PlayerPrefs.GetInt(PlayerPrefsManager.Shuffle);
        gameUiManager.SetHintText(hintAmount);
        gameUiManager.SetShuffleText(shuffleAmount);
        gameUiManager.UpdaeCoinText(coin);
    }

    private void Update()
    {
        if (isCanPlay)
        {
            ControlTimer();
        }
    }

    public void GenerateBoard()
    {
        lineRenderer.positionCount = 0;
        spacingX = tilePrefab.GetComponent<BoxCollider2D>().size.x;
        spacingY = tilePrefab.GetComponent<BoxCollider2D>().size.y;
        grid = new GameObject[rows + 2, cols + 2];
        List<Sprite> usedSprites = GenerateShuffledSprites();
        StartCoroutine(SetUpTiles(usedSprites));
    }

    List<Sprite> GenerateShuffledSprites()
    {
        List<Sprite> sprites = new List<Sprite>();
        int numPairs = (rows * cols) / 2;

        for (int i = 0; i < numPairs; i++)
        {
            Sprite randomSprite = pikachuSprites[Random.Range(0, pikachuSprites.Length)];
            if (randomSprite != null)
            {
                sprites.Add(randomSprite);
                sprites.Add(randomSprite);
            }
        }

        Shuffle(sprites);
        return sprites;
    }

    IEnumerator SetUpTiles(List<Sprite> sprites)
    {
        int index = 0;
        Vector2 offset = new Vector2(-((cols - -1) * spacingX) / 2, ((rows - -1) * spacingY) / 2);

        for (int i = 1; i <= rows; i++)
        {
            for (int j = 1; j <= cols; j++)
            {
                yield return new WaitForEndOfFrame();
                Vector2 pos = new Vector2(j * spacingX + offset.x, -i * spacingY + offset.y - offsetYAxis);
                GameObject newTile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                Tile tileScript = newTile.GetComponent<Tile>();
                if (tileScript != null && index < sprites.Count)
                {
                    tileScript.SetSprite(sprites[index++]);
                    tileScript.gridPosition = new Vector2Int(i, j);
                    tileScript.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).From(0);
                    grid[i, j] = newTile;
                }
            }
        }

        NoMoreMoves();
    }

    public bool CanConnect(Tile tile1, Tile tile2)
    {
        if (tile1 == null || tile2 == null || tile1 == tile2 ||
            tile1.gameObject == null || tile2.gameObject == null ||
            tile1.GetComponent<SpriteRenderer>() == null || tile2.GetComponent<SpriteRenderer>() == null)
        {
            return false;
        }

        Vector2Int pos1 = tile1.gridPosition, pos2 = tile2.gridPosition;
        bool canConnect = CheckLine(pos1, pos2) || CheckLShape(pos1, pos2) || CheckZShape(pos1, pos2);
        if (canConnect)
        {
            DrawConnectionLine(tile1.transform.position, tile2.transform.position, pos1, pos2);
            return true;
        }

        return false;
    }

    public bool CheckCanPlay(Tile tile1, Tile tile2)
    {
        if (tile1 == null || tile2 == null || tile1 == tile2 ||
            tile1.gameObject == null || tile2.gameObject == null ||
            tile1.GetComponent<SpriteRenderer>() == null || tile2.GetComponent<SpriteRenderer>() == null)
        {
            return false;
        }

        Vector2Int pos1 = tile1.gridPosition, pos2 = tile2.gridPosition;
        bool canConnect = CheckLine(pos1, pos2) || CheckLShape(pos1, pos2) || CheckZShape(pos1, pos2);
        if (canConnect)
        {
            return true;
        }
        return false;
    }

    void DrawConnectionLine(Vector3 startPos, Vector3 endPos, Vector2Int pos1, Vector2Int pos2)
    {
        List<Vector2Int> path = GetConnectionPath(pos1, pos2);
        if (path != null && path.Count > 0)
        {
            lineRenderer.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++)
            {
                lineRenderer.SetPosition(i, GridPositionToWorldPosition(path[i]));
            }
            StartCoroutine(ClearLineAfterDelay(0.2f));
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    Vector3 GridPositionToWorldPosition(Vector2Int gridPos)
    {
        Vector2 offset = new Vector2(-((cols - -1) * spacingX) / 2, ((rows - -1) * spacingY) / 2);
        return new Vector3(gridPos.y * spacingX + offset.x, -gridPos.x * spacingY + offset.y, 0);
    }

    IEnumerator ClearLineAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        lineRenderer.positionCount = 0;
    }

    List<Vector2Int> GetConnectionPath(Vector2Int pos1, Vector2Int pos2)
    {
        if (CheckLine(pos1, pos2)) return GetLinePath(pos1, pos2);
        if (CheckLShape(pos1, pos2)) return GetLShapePath(pos1, pos2);
        if (CheckZShape(pos1, pos2)) return GetZShapePath(pos1, pos2);
        return null;
    }

    List<Vector2Int> GetLinePath(Vector2Int pos1, Vector2Int pos2)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(pos1);
        if (pos1.x == pos2.x)
        {
            for (int y = Mathf.Min(pos1.y, pos2.y) + 1; y < Mathf.Max(pos1.y, pos2.y); y++)
            {
                path.Add(new Vector2Int(pos1.x, y));
            }
        }
        else
        {
            for (int x = Mathf.Min(pos1.x, pos2.x) + 1; x < Mathf.Max(pos1.x, pos2.x); x++)
            {
                path.Add(new Vector2Int(x, pos1.y));
            }
        }

        path.Add(pos2);
        return path;
    }

    List<Vector2Int> GetLShapePath(Vector2Int pos1, Vector2Int pos2)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(pos1);
        Vector2Int corner1 = new Vector2Int(pos1.x, pos2.y);
        Vector2Int corner2 = new Vector2Int(pos2.x, pos1.y);

        if (grid[corner1.x, corner1.y] == null && CheckLine(pos1, corner1) && CheckLine(corner1, pos2))
        {
            path.Add(corner1);
        }
        else if (grid[corner2.x, corner2.y] == null && CheckLine(pos1, corner2) && CheckLine(corner2, pos2))
        {
            path.Add(corner2);
        }

        path.Add(pos2);
        return path;
    }

    List<Vector2Int> GetZShapePath(Vector2Int pos1, Vector2Int pos2)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(pos1);
        for (int i = 0; i < cols + 2; i++)
        {
            Vector2Int mid1 = new Vector2Int(pos1.x, i);
            Vector2Int mid2 = new Vector2Int(pos2.x, i);
            if (IsValidZShape(mid1, mid2, pos1, pos2))
            {
                path.Add(mid1);
                path.Add(mid2);
                path.Add(pos2);
                return path;
            }
        }

        for (int j = 0; j < rows + 2; j++)
        {
            Vector2Int mid1 = new Vector2Int(j, pos1.y);
            Vector2Int mid2 = new Vector2Int(j, pos2.y);
            if (IsValidZShape(mid1, mid2, pos1, pos2))
            {
                path.Add(mid1);
                path.Add(mid2);
                path.Add(pos2);
                return path;
            }
        }

        return null;
    }

    bool CheckLine(Vector2Int pos1, Vector2Int pos2)
    {
        if (pos1.x == pos2.x)
        {
            int minY = Mathf.Min(pos1.y, pos2.y);
            int maxY = Mathf.Max(pos1.y, pos2.y);
            for (int y = minY + 1; y < maxY; y++)
            {
                if (grid[pos1.x, y] != null) return false;
            }

            return true;
        }
        else if (pos1.y == pos2.y)
        {
            int minX = Mathf.Min(pos1.x, pos2.x);
            int maxX = Mathf.Max(pos1.x, pos2.x);
            for (int x = minX + 1; x < maxX; x++)
            {
                if (grid[x, pos1.y] != null) return false;
            }

            return true;
        }

        return false;
    }

    bool CheckLShape(Vector2Int pos1, Vector2Int pos2)
    {
        Vector2Int corner1 = new Vector2Int(pos1.x, pos2.y);
        Vector2Int corner2 = new Vector2Int(pos2.x, pos1.y);
        return (grid[corner1.x, corner1.y] == null && CheckLine(pos1, corner1) && CheckLine(corner1, pos2)) ||
               (grid[corner2.x, corner2.y] == null && CheckLine(pos1, corner2) && CheckLine(corner2, pos2));
    }

    bool CheckZShape(Vector2Int pos1, Vector2Int pos2)
    {
        for (int i = 0; i < cols + 2; i++)
        {
            Vector2Int mid1 = new Vector2Int(pos1.x, i);
            Vector2Int mid2 = new Vector2Int(pos2.x, i);
            if (IsValidZShape(mid1, mid2, pos1, pos2)) return true;
        }

        for (int j = 0; j < rows + 2; j++)
        {
            Vector2Int mid1 = new Vector2Int(j, pos1.y);
            Vector2Int mid2 = new Vector2Int(j, pos2.y);
            if (IsValidZShape(mid1, mid2, pos1, pos2)) return true;
        }

        return false;
    }

    bool IsValidZShape(Vector2Int mid1, Vector2Int mid2, Vector2Int pos1, Vector2Int pos2)
    {
        return grid[mid1.x, mid1.y] == null && grid[mid2.x, mid2.y] == null &&
               CheckLine(pos1, mid1) && CheckLine(mid1, mid2) && CheckLine(mid2, pos2);
    }

    void Shuffle(List<Sprite> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    public bool NoMoreMoves()
    {
        if (lastHintedTile1 != null && lastHintedTile1 != null && lastHintedTile1.highlightBorder != null)
            lastHintedTile1.SetHighlight(false);
        if (lastHintedTile2 != null && lastHintedTile2 != null && lastHintedTile2.highlightBorder != null)
            lastHintedTile2.SetHighlight(false);

        lastHintedTile1 = null;
        lastHintedTile2 = null;

        for (int i = 1; i <= rows; i++)
        {
            for (int j = 1; j <= cols; j++)
            {
                if (grid[i, j] == null) continue;
                Tile tile1 = grid[i, j].GetComponent<Tile>();
                if (tile1 == null || tile1.GetComponent<SpriteRenderer>() == null) continue;

                for (int x = 1; x <= rows; x++)
                {
                    for (int y = 1; y <= cols; y++)
                    {
                        if (grid[x, y] == null) continue;
                        Tile tile2 = grid[x, y].GetComponent<Tile>();
                        if (tile2 == null || tile1 == tile2 || tile2.GetComponent<SpriteRenderer>() == null) continue;

                        if (tile1.GetComponent<SpriteRenderer>().sprite ==
                            tile2.GetComponent<SpriteRenderer>().sprite &&
                            CheckCanPlay(tile1, tile2))
                        {
                            lastHintedTile1 = tile1;
                            lastHintedTile2 = tile2;
                           // hintCoroutine = StartCoroutine(BlinkHintHighlight());
                            return true;
                        }
                    }
                }
            }
        }
        ShuffleFunction();
        return false;
    }

    public void ShuffleBoard()
    {
        if (shuffleAmount <= 0 || !isCanPlay) return;
        shuffleAmount--;
        PlayerPrefs.SetInt(PlayerPrefsManager.Shuffle, shuffleAmount);
        gameUiManager.SetShuffleText(shuffleAmount);
        ShuffleFunction();

        // NoMoreMoves();
    }

    private void ShuffleFunction()
    {
        Debug.Log("Shuffle ????");

        List<Sprite> sprites = new List<Sprite>();
        for (int i = 1; i <= rows; i++)
        {
            for (int j = 1; j <= cols; j++)
            {
                if (grid[i, j] != null)
                {
                    Tile tile = grid[i, j].GetComponent<Tile>();
                    if (tile != null && tile.GetComponent<SpriteRenderer>() != null)
                    {
                        sprites.Add(tile.GetComponent<SpriteRenderer>().sprite);
                    }
                }
            }
        }

        Shuffle(sprites);
        int index = 0;
        for (int i = 1; i <= rows; i++)
        {
            for (int j = 1; j <= cols; j++)
            {
                if (grid[i, j] != null)
                {
                    Tile tile = grid[i, j].GetComponent<Tile>();
                    if (tile != null && index < sprites.Count)
                    {
                        tile.SetSprite(sprites[index++]);
                    }
                }
            }
        }
    }

    public void Hint()
    {
        Debug.Log("Hinttttt");
        if (hintAmount <= 0|| !isCanPlay) return;
        hintAmount--;
        PlayerPrefs.SetInt(PlayerPrefsManager.Hint, hintAmount);
        gameUiManager.SetHintText(hintAmount);
        if (hintCoroutine != null)
        {
            StopCoroutine(hintCoroutine);
            hintCoroutine = null;
        }

        if (lastHintedTile1 != null && lastHintedTile1 != null && lastHintedTile1.highlightBorder != null)
            lastHintedTile1.SetHighlight(false);
        if (lastHintedTile2 != null && lastHintedTile2 != null && lastHintedTile2.highlightBorder != null)
            lastHintedTile2.SetHighlight(false);

        lastHintedTile1 = null;
        lastHintedTile2 = null;

        for (int i = 1; i <= rows; i++)
        {
            for (int j = 1; j <= cols; j++)
            {
                if (grid[i, j] == null) continue;
                Tile tile1 = grid[i, j].GetComponent<Tile>();
                if (tile1 == null || tile1.GetComponent<SpriteRenderer>() == null) continue;

                for (int x = 1; x <= rows; x++)
                {
                    for (int y = 1; y <= cols; y++)
                    {
                        if (grid[x, y] == null) continue;
                        Tile tile2 = grid[x, y].GetComponent<Tile>();
                        if (tile2 == null || tile1 == tile2 || tile2.GetComponent<SpriteRenderer>() == null) continue;

                        if (tile1.GetComponent<SpriteRenderer>().sprite ==
                            tile2.GetComponent<SpriteRenderer>().sprite &&
                            CanConnect(tile1, tile2))
                        {
                            lastHintedTile1 = tile1;
                            lastHintedTile2 = tile2;
                            hintCoroutine = StartCoroutine(BlinkHintHighlight());
                            return;
                        }
                    }
                }
            }
        }

        Debug.Log("No matching pair found for hint.");
    }

    private IEnumerator BlinkHintHighlight()
    {
        float elapsedTime = 0f;
        bool isVisible = true;

        while (elapsedTime < hintHighlightDuration)
        {
            if (lastHintedTile1 == null || lastHintedTile1 == null || lastHintedTile1.highlightBorder == null ||
                lastHintedTile2 == null || lastHintedTile2 == null || lastHintedTile2.highlightBorder == null)
            {
                yield break;
            }

            lastHintedTile1.SetHighlight(isVisible);
            lastHintedTile2.SetHighlight(isVisible);

            isVisible = !isVisible;
            elapsedTime += blinkInterval;

            yield return new WaitForSeconds(blinkInterval);
        }

        if (lastHintedTile1 != null && lastHintedTile1 != null && lastHintedTile1.highlightBorder != null)
            lastHintedTile1.SetHighlight(false);
        if (lastHintedTile2 != null && lastHintedTile2 != null && lastHintedTile2.highlightBorder != null)
            lastHintedTile2.SetHighlight(false);

        hintCoroutine = null;
    }

    public void PlaySmokeParicle(GameObject tile1, GameObject tile2)
    {
        smokeParticle1.gameObject.transform.position = tile1.transform.position;
        smokeParticle2.gameObject.transform.position = tile2.transform.position;
        Duck.PlayParticle(smokeParticle1);
        Duck.PlayParticle(smokeParticle2);
        GetCoin();

       
    }

    public void GetCoin()
    {
        coin += 2;
        score += 10;
        gameUiManager.UpdaeCoinText(coin);
        gameUiManager.UpdaeScoreText(score);
    }

    public void ControlTimer()
    {
        if (currentTime >= 0)
        {
            currentTime -= Duck.TimeMod;
            float val = currentTime / maxTime;
            gameUiManager.ReduceTimer(val);
        }
        else
        {
            if (!isEndGame)
            {
                isEndGame = true;
                Debug.Log("Lose");
                UiWinLose.ShowLosePanel();
                PlayerPrefs.SetInt(PlayerPrefsManager.Coin,coin);
            }
        }
    }

    public void CheckWinLose()
    {
        totalPiece -= 2;
        if (totalPiece <= 0)
        {
            Debug.Log("Win");
            DOVirtual.DelayedCall(0.7f, (() => CheckTimerShowWin()));
            PlayerPrefs.SetInt(PlayerPrefsManager.Coin,coin);
        }
    }
    public void CheckTimerShowWin()
    {
        ManagerGame.TIME_SCALE = 0;
        float val = currentTime / maxTime;

        if (val < 0.2f)
        {
            UiWinLose.ShowWinPanel1Star();
        }
        else if (val < 0.4f)
        {
            UiWinLose.ShowWinPanel2Star();
        }
        else
        {
            UiWinLose.ShowWinPanel3Star();
        }
    }
}