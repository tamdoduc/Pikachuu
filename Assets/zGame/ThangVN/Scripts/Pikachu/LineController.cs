using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThangVN.LevelPikachu
{
    public class LineController : MonoBehaviour
    {
        private LineRenderer lineRenderer;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        public void DrawPath(List<Vector3> positions)
        {
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
            StartCoroutine(ResetLine());
        }

        public void ClearPath()
        {
            if (lineRenderer != null)
            {
                lineRenderer.positionCount = 0;
            }
        }

        private IEnumerator ResetLine()
        {
            yield return new WaitForSeconds(0.5f);
            GridManager.Instance.lineControllerPool.ReturnLineController(this);
            ClearPath();
        }
    }
}