using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ThangVN.LevelPikachu
{
    public class Block : MonoBehaviour
    {
        public int row, col, id;
        [SerializeField] SpriteRenderer thisSprite;
        [SerializeField] SpriteRenderer nIcon;
        [SerializeField] GameObject selected;
        [SerializeField] GameObject fail;
        [SerializeField] TextMeshPro txt;

        Color defaultColor = Color.white;
        Color hintColor = Color.green;
        private void Awake()
        {
            thisSprite = GetComponent<SpriteRenderer>();
        }

        public bool isDetecting;
        public bool isHinting;

        public void Init(int _row, int _col)
        {
            row = _row;
            col = _col;
        }

        public void InitValue(int _id, Sprite sprite)
        {
            nIcon.sprite = sprite;
            id = _id;
            txt.text = id.ToString();
        }

        public void SetSelected(bool isSelected)
        {
            selected.SetActive(isSelected);
        }

        public void SetFalse()
        {
            selected.SetActive(false);
            fail.SetActive(true);
        }

        public void SetDefault()
        {
            selected.SetActive(false);
            fail.SetActive(false);
        }

        public void StopHint()
        {
            StopCoroutine(hintCoroutine);
            thisSprite.color = defaultColor;
        }

        public void SetHint()
        {
            isHinting = true;
            hintCoroutine = StartCoroutine(ChangeColorLoop());
        }

        float duration = 0.3f;
        Coroutine hintCoroutine;
        private IEnumerator ChangeColorLoop()
        {
            while (true)
            {
                float time = 0;
                while (time < duration)
                {
                    thisSprite.color = Color.Lerp(defaultColor, hintColor, time / duration);
                    time += Time.deltaTime;
                    yield return null;
                }

                time = 0;
                while (time < duration)
                {
                    thisSprite.color = Color.Lerp(hintColor, defaultColor, time / duration);
                    time += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}
