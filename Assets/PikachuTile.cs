using UnityEngine;

public class PikachuTile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite sprite { get; private set; }
    public int row, col;
    private GameManager gameManager;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void SetSprite(Sprite newSprite)
    {
        sprite = newSprite;
        spriteRenderer.sprite = newSprite;
    }

    public void SetPosition(int r, int c)
    {
        row = r;
        col = c;
    }

    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }

    void OnMouseDown()
    {
        gameManager.SelectTile(this);
    }
}