using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThangVN.LevelPikachu
{


    public class LogicGame : MonoBehaviour
    {
        public static LogicGame Instance;
        [SerializeField] Camera cam;
        [SerializeField] private LayerMask layerBlock;

        public List<Block> selectedBlocks = new List<Block>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void SelectBlock(Block block)
        {
            if (selectedBlocks.Contains(block))
            {
                return;
            }

            selectedBlocks.Add(block);
            block.SetSelected(true);
            if (block.isHinting)
            {
                GridManager.Instance.RefreshListHint();
            }

            if (selectedBlocks.Count >= 2)
            {
                CheckBlock();
            }

        }

        public void DeselectBlock(Block block)
        {
            if (selectedBlocks.Contains(block))
            {
                selectedBlocks.Remove(block);
                block.SetSelected(false);
            }
        }

        public void ClearSelection()
        {
            foreach (Block block in selectedBlocks)
            {
                block.SetSelected(false);
            }
            selectedBlocks.Clear();
        }

        public void CheckBlock()
        {
            Block startBlock = selectedBlocks[0];
            Block endBlock = selectedBlocks[1];

            if (startBlock.id != endBlock.id)
            {
                Debug.Log("khác value ");
                foreach (var block in selectedBlocks)
                {
                    block.isDetecting = true;
                    block.SetFalse();

                    block.transform.DOShakeRotation(0.5f, 90, 10, 90, true, ShakeRandomnessMode.Full).OnComplete(() =>
                    {
                        block.transform.localEulerAngles = Vector3.zero;
                        block.SetDefault();
                        block.isDetecting = false;
                    });
                }

                ClearSelection();
                return;
            }
            else
            {
                bool check = GridManager.Instance.CheckTwoBlocks(startBlock, endBlock);

                if (check)
                {
                    Debug.Log("Đường đi tìm thấy:");
                    foreach (var block in selectedBlocks)
                    {
                        block.isDetecting = true;
                        block.SetSelected(true);
                        GridManager.Instance.ClearBlock(block);
                        block.transform.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.5f)
                            .OnComplete(() =>
                            {
                                block.isDetecting = false;
                                block.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                                block.gameObject.SetActive(false);
                            });
                    }
                    selectedBlocks.Clear();


                }
                else
                {
                    Debug.Log("không có đường đi ");
                    foreach (var block in selectedBlocks)
                    {
                        block.isDetecting = true;
                        block.SetFalse();

                        block.transform.DOShakeRotation(0.5f, 90, 10, 90, true, ShakeRandomnessMode.Full).OnComplete(() =>
                        {
                            block.SetDefault();
                            block.isDetecting = false;
                        });
                    }

                    ClearSelection();
                    return;
                }

            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("ScenePikachu");
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector2 rayOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero, 100f, layerBlock);

                if (hit.collider != null)
                {
                    Block block = hit.collider.GetComponent<Block>();
                    if (block.isDetecting) return;
                    SelectBlock(block);
                }
            }

            //if (Input.GetMouseButtonDown(1))
            //{
            //    Vector2 rayOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            //    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero, 100f, layerBlock);

            //    if (hit.collider != null)
            //    {
            //        Block block = hit.collider.GetComponent<Block>();
            //        GridManager.Instance.ClearBlock(block);
            //        block.gameObject.SetActive(false);

            //    }
            //}
        }
    }
}
