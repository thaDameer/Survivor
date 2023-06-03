using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Level
{
    public class GroundPlane : MonoBehaviour, IInitializable
    {
        [SerializeField] private Vector3 size = new Vector3(8, 8, 8);
        [SerializeField] private GameObject groundPlane;
        public Vector3 GetSize() => size;
        public TextMeshPro textMeshPro;

        public void Initialize()
        {
            groundPlane.transform.localScale = size;
        }

        public void SetText(string text)
        {
            textMeshPro.text = text;
        }
    }
}