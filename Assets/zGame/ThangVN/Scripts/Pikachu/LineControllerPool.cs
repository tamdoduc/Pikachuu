using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThangVN.LevelPikachu
{

    public class LineControllerPool : MonoBehaviour
    {
        public GameObject lineControllerPrefab;
        public int poolSize = 10;

        private List<LineController> lineControllers = new List<LineController>();

        private void Start()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject lineControllerObject = Instantiate(lineControllerPrefab, transform);
                LineController lineController = lineControllerObject.GetComponent<LineController>();
                lineControllers.Add(lineController);
                lineControllerObject.SetActive(false);
            }
        }

        public LineController GetLineController()
        {
            foreach (LineController lineController in lineControllers)
            {
                if (!lineController.gameObject.activeInHierarchy)
                {
                    lineController.gameObject.SetActive(true);
                    return lineController;
                }
            }

            GameObject lineControllerObject = Instantiate(lineControllerPrefab, transform);
            LineController newLineController = lineControllerObject.GetComponent<LineController>();
            lineControllers.Add(newLineController);
            return newLineController;
        }

        public void ReturnLineController(LineController lineController)
        {
            lineController.gameObject.SetActive(false);
        }
    }

}