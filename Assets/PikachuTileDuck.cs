using UnityEngine;

public class PikachuTileDuck : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite sprite { get; private set; }
    public int row, col;
    private GameManagerDuck _gameManagerDuck;

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

    public void SetGameManager(GameManagerDuck managerDuck)
    {
        _gameManagerDuck = managerDuck;
    }

    void OnMouseDown()
    {
        _gameManagerDuck.SelectTile(this);
    }
}