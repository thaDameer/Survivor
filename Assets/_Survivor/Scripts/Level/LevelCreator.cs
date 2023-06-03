using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using UnityEngine;
using UnityEditor;

namespace Level
{
    public class LevelCreator : MonoBehaviour
    {
        [SerializeField] private int size = 12;
        [SerializeField] private GroundPlane groundPlane;
        public float debugTilePlaceDelay = 0.05f;
        private Vector3 _worldOffset;
        private List<GroundPlane> _gpList = new List<GroundPlane>();

        private bool _debugTimer;

        private void Start()
        {
            CreateFloor();
        }

        private void CreateFloor()
        {
            _debugTimer = debugTilePlaceDelay > 0;
            var s = Spiral(size);

            SetupOffset();

            var pos = Reorder(s);
            StartCoroutine(SetupGround(pos));
        }

        /// <summary>
        /// Spawns a square for each position in list;
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private IEnumerator SetupGround(IList<Vector3> pos)
        {
            groundPlane.gameObject.SetActive(true);
            var v3 = new Vector3();
            for (int i = 0; i < pos.Count; i++)
            {
                v3.x = pos[i].x - _worldOffset.x;
                v3.y = 0;
                v3.z = pos[i].z - _worldOffset.z;
                if (_debugTimer)
                    yield return new WaitForSeconds(debugTilePlaceDelay);
                _gpList.Add(SpawnSquare(v3, i + 1));
            }
            groundPlane.gameObject.SetActive(false);
            yield return null;
        }

        /// <summary>
        /// Creates an array with Spiral Pattern.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static int[,] Spiral(int n)
        {
            int[,] result = new int[n, n];

            int pos = 0;
            int count = n;
            int value = -n;
            int sum = -1;

            do
            {
                value = -1 * value / n;
                for (int i = 0; i < count; i++)
                {
                    sum += value;
                    result[sum / n, sum % n] = pos++;
                }

                value *= n;
                count--;
                for (int i = 0; i < count; i++)
                {
                    sum += value;
                    result[sum / n, sum % n] = pos++;
                }
            } while (count > 0);

            return result;
        }

        /// <summary>
        /// Puts the array into a Reversed Vector3 list based on int[,].
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private Vector3[] Reorder(int[,] array)
        {
            Vector3[] list = new Vector3[array.GetLength(0) * array.GetLength(1)];
            var offset = groundPlane.GetSize();
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    list[array[i, j]] = (new Vector3(offset.x * i, 0, offset.z * j));
                }
            }

            return FlipList(list);
        }

        /// <summary>
        /// Flips/Reverses the list
        /// </summary>
        /// <param name="listToFlip"></param>
        /// <returns></returns>
        private static Vector3[] FlipList(IReadOnlyList<Vector3> listToFlip)
        {
            var newList = new Vector3[listToFlip.Count];

            int newListPos = 0;
            for (int i = listToFlip.Count - 1; i >= 0; i--)
            {
                newList[newListPos] = listToFlip[i];
                newListPos++;
            }

            return newList;
        }

        /// <summary>
        /// Instantiates a ground plane on position with label number displayed.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="number"></param>
        private GroundPlane SpawnSquare(Vector3 pos, int number) //
        {
            var gp = Instantiate(groundPlane, transform);
            gp.SetText(number + "");
            gp.transform.localPosition = pos;
            return gp;
        }

        /// <summary>
        /// Takes the size value, divides by half and sets the <param name="_worldOffset"></param> with said values
        /// </summary>
        private void SetupOffset()
        {
            var x = size * groundPlane.GetSize().x / 2;
            var y = size * groundPlane.GetSize().y / 2;
            var z = size * groundPlane.GetSize().z / 2;
            _worldOffset = new Vector3(x, y, z);
        }

        public void Reset()
        {
            _gpList?.ForEach(Destroy);
            groundPlane.gameObject.SetActive(true);
            CreateFloor();
        }
    }
}
/*
[CustomEditor(typeof(LevelCreator))]
public class LevelCreatorEditor : Editor
{
    #region GUI_Button

    private LevelCreator creator;
    private bool init;

    private void OnEnable()
    {
        if (init) return;
        creator = FindObjectOfType<Level.LevelCreator>();
        init = creator == null;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Rect lastRect = GUILayoutUtility.GetLastRect();
        Rect buttonRect = new Rect(lastRect.x, lastRect.y + EditorGUIUtility.singleLineHeight, 150, 30);
        if (GUI.Button(buttonRect, "Generate New Ground"))
        {
            if (!init)
            {
                creator = FindObjectOfType<Level.LevelCreator>();
                init = creator == null;
            }

            creator.Reset();
        }
    }


    #endregion
}*/