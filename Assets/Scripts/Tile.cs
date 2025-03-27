using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isSelected = false;
    public static Tile firstSelected, secondSelected;
    public Vector2Int gridPosition;
    [HideInInspector] public GameObject highlightBorder;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError($"SpriteRenderer component not found on {gameObject.name}. Please attach a SpriteRenderer to this GameObject.");
            return;
        }

        highlightBorder = new GameObject("HighlightBorder");
        highlightBorder.transform.SetParent(transform);
        highlightBorder.transform.localPosition = Vector3.zero;
        SpriteRenderer borderRenderer = highlightBorder.AddComponent<SpriteRenderer>();
        borderRenderer.sprite = spriteRenderer.sprite;
        borderRenderer.color = Color.yellow;
        borderRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
        float borderScale = 1.2f;
        highlightBorder.transform.localScale = Vector3.one * borderScale;
        highlightBorder.SetActive(false);
    }

    public void SetSprite(Sprite sprite)
    {
        if (spriteRenderer == null)
        {
            Debug.LogWarning($"Cannot set sprite on {gameObject.name}: SpriteRenderer is null.");
            return;
        }
        if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
            if (highlightBorder != null)
            {
                highlightBorder.GetComponent<SpriteRenderer>().sprite = sprite;
            }
        }
    }

    void OnMouseDown()
    {
        if (isSelected || GameManager.instance == null || spriteRenderer == null) return;

        isSelected = true;
        highlightBorder.SetActive(true);

        if (firstSelected == null)
        {
            firstSelected = this;
        }
        else if (firstSelected != this)
        {
            secondSelected = this;
            StartCoroutine(CheckMatchRoutine());
        }
    }

    private IEnumerator CheckMatchRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        if (firstSelected == null || secondSelected == null ||
            firstSelected.spriteRenderer == null || secondSelected.spriteRenderer == null)
        {
            ResetSelection();
            yield break;
        }

        bool isMatch = firstSelected.spriteRenderer.sprite == secondSelected.spriteRenderer.sprite &&
                       GameManager.instance.CanConnect(firstSelected, secondSelected);

        if (GameManager.instance != null)
        {
            GameManager.instance.PlaySound(isMatch);
        }

        if (isMatch)
        {
            Destroy(firstSelected.gameObject);
            Destroy(secondSelected.gameObject);
        }
        else
        {
            if (firstSelected != null && firstSelected.highlightBorder != null)
                firstSelected.highlightBorder.SetActive(false);
            if (secondSelected != null && secondSelected.highlightBorder != null)
                secondSelected.highlightBorder.SetActive(false);
        }

        ResetSelection();
    }

    private void ResetSelection()
    {
        if (firstSelected != null)
            firstSelected.isSelected = false;
        if (secondSelected != null)
            secondSelected.isSelected = false;

        firstSelected = null;
        secondSelected = null;
    }

    void OnDestroy()
    {
        if (firstSelected == this) firstSelected = null;
        if (secondSelected == this) secondSelected = null;

        GameManager manager = GameManager.instance;
        if (manager != null)
        {
            if (manager.lastHintedTile1 == this)
                manager.lastHintedTile1 = null;
            if (manager.lastHintedTile2 == this)
                manager.lastHintedTile2 = null;
        }
    }

    public void SetHighlight(bool isActive)
    {
        if (highlightBorder != null)
        {
            highlightBorder.SetActive(isActive);
        }
    }
}