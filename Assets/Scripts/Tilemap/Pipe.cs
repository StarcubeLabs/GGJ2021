using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GGJ2021
{
#if UNITY_EDITOR
    [CustomEditor(typeof(Pipe))]
    public class PipeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            string instructions = "1. Set up the pipe of your dreams."
                + "\n2. Positon camera straight on. (I'm so sorry)"
                + "\n3. Press Bake Pipe.";
            GUILayout.Label(new GUIContent(instructions));

            Pipe pipe = (Pipe)target;
            if (GUILayout.Button("Bake Trail"))
            {
                pipe.CalculatePointsFromContainer();
                pipe.BakeTrail();
            }

            if (Application.isPlaying)
            {
                if (GUILayout.Button("Transport Debug Object") && pipe.DebugObjectThroughPipe)
                    pipe.MoveObjectThroughPipe(pipe.DebugObjectThroughPipe, pipe.DebugSpeedThroughPipe, delegate
                    {
                        Debug.Log("Object Reached Destination Through Pipe");
                    });
            }
            else
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.magenta;
                GUILayout.Label(new GUIContent("'Transport Debug Object' unavailable outside of play mode"), style);
            }

            base.OnInspectorGUI();
        }
    }
#endif

    [RequireComponent(typeof(Ara.AraTrail))]
    [ExecuteInEditMode]
    public class Pipe : MonoBehaviour
    {
        public Ara.AraTrail AraTrail;
        public Transform PointsContainer;

        public GameObject StartPipeCap;
        public GameObject EndPipeCap;
        public float PipeCapOffset = 1f;
        public bool UpdatePipeCapRotations;

        public List<Vector3> PointPositions = new List<Vector3>();

        public GameObject BakedObject;

        [Header("Debug Variables")]
        public Transform DebugObjectThroughPipe;
        public float DebugSpeedThroughPipe = 1f;

        private float _previousPipeCapOffset = 0f;

        public List<Vector3> GetPositionsFromContainer()
        {
            List<Vector3> positions = new List<Vector3>();
            for (int i = 0; i < PointsContainer.childCount; i++)
            {
                Vector3 pos = PointsContainer.GetChild(i).position;
                positions.Add(pos);
            }
            return positions;
        }

        public void CalculatePointsFromContainer()
        {
            CalculatePoints(GetPositionsFromContainer());
        }

        public void CalculatePoints(List<Vector3> positions)
        {
            AraTrail.Clear();
            foreach (Vector3 pos in positions)
            {
                Ara.AraTrail.Point point = new Ara.AraTrail.Point(
                    position: pos,
                    velocity: default,
                    tangent: default,
                    normal: default,
                    color: Color.white,
                    thickness: 1f,
                    texcoord: 0,
                    lifetime: Mathf.Infinity);
                AraTrail.points.Add(point);
            }

            PointPositions = positions;

            UpdatePipeCaps();
        }

        private void UpdatePipeCaps()
        {
            if (StartPipeCap)
            {
                Vector3 startPos = PointPositions.First();

                if (UpdatePipeCapRotations && PointPositions.Count > 1)
                {
                    Vector3 direction = startPos - PointPositions[1];
                    StartPipeCap.transform.forward = direction;
                }

                StartPipeCap.transform.position = startPos + (StartPipeCap.transform.forward * PipeCapOffset);
            }

            if (EndPipeCap)
            {
                Vector3 endPos = PointPositions.Last();
                EndPipeCap.transform.position = endPos;

                if (UpdatePipeCapRotations && PointPositions.Count > 1)
                {
                    Vector3 direction = endPos - PointPositions[PointPositions.Count - 2];
                    EndPipeCap.transform.forward = direction;
                }

                EndPipeCap.transform.position = endPos + (EndPipeCap.transform.forward * PipeCapOffset);
            }
        }

        public void BakeTrail()
        {
            GameObject go = new GameObject($"Baked {gameObject.name} trail");
            go.transform.parent = transform;

            // Create a filter/renderer combo. Note this will only work IF THE OBJECT DOES NOT HAVE THEM ALREADY!
            MeshFilter filter = go.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();

            if (filter != null && meshRenderer != null)
            {
                // Feed a copy of the mesh
                filter.mesh = Instantiate(AraTrail.mesh);

                // Feed the material list.
                meshRenderer.materials = AraTrail.materials;
            }
            else
                Debug.LogError("[BakeTrail]: Could not bake the trail because the object already had a MeshRenderer.");

            //Copy pipe caps as well
            if (StartPipeCap)
            {
                GameObject pipeStart = GameObject.Instantiate(StartPipeCap, go.transform);
                pipeStart.transform.position = StartPipeCap.transform.position;
            }
            if (EndPipeCap)
            {
                GameObject pipeEnd = GameObject.Instantiate(EndPipeCap, go.transform);
                pipeEnd.transform.position = EndPipeCap.transform.position;
            }

            if (BakedObject)
                DestroyImmediate(BakedObject);
            BakedObject = go;

#if UNITY_EDITOR
            Undo.RegisterCreatedObjectUndo(go, "Baked Pipe Body");
#endif
        }

        private void Awake()
        {
            if (!AraTrail)
                AraTrail = GetComponent<Ara.AraTrail>();

            AraTrail.emit = false;

            //we don't need to generate a pipe at all if we baked it
            if (BakedObject)
            {
                AraTrail.enabled = false;
                EndPipeCap?.SetActive(false);
                StartPipeCap?.SetActive(false);
            }
            else
            {
                if (PointsContainer)
                {
                    CalculatePointsFromContainer();
                }
            }
        }

        private bool PositionsHasChanged()
        {
            List<Vector3> positions = GetPositionsFromContainer();

            if (positions.Count != PointPositions.Count)
                return true;

            if (AraTrail.points.Count != positions.Count)
                return true;

            for (int i = 0; i < positions.Count; i++)
            {
                if (positions[i] != PointPositions[i])
                {
                    return true;
                }
            }
            return false;
        }

        public void MoveObjectThroughPipe(Transform trans, float speed, System.Action onFinish, StartingPoint startingPointEnum = StartingPoint.Auto)
        {
            StartCoroutine(MoveObjectThroughPipeRoutine(trans, speed, onFinish, startingPointEnum));
        }

        private IEnumerator MoveObjectThroughPipeRoutine(Transform trans, float speed, System.Action onFinish, StartingPoint startingPointEnum)
        {
            //figure out which direction through the pipes we're moving
            int direction = 1;
            switch (startingPointEnum)
            {
                case (StartingPoint.EndPipe):
                    direction = -1;
                    break;
                case (StartingPoint.StartPipe):
                    direction = 1;
                    break;
                default:
                    float distToStart = Vector3.Distance(trans.position, PointPositions.First());
                    float distToEnd = Vector3.Distance(trans.position, PointPositions.Last());
                    if (distToStart < distToEnd)
                        direction = 1;
                    else
                        direction = -1;
                    break;
            }

            //set starting variables
            int currentIndex = direction > 0 ? 0 : PointPositions.Count - 1;
            Vector3 previousPosition = trans.position;
            Vector3 currentdestination = PointPositions[currentIndex];
            float currentDist = 0;
            float goalDist = Vector3.Distance(currentdestination, trans.position);
            Vector3 goalDirection = (currentdestination - trans.position).normalized;
            while (0 <= currentIndex && currentIndex < PointPositions.Count)
            {
                Vector3 position = trans.position;

                currentDist += speed * Time.deltaTime;

                //we've reached/passed our destination
                if (currentDist >= goalDist)
                {
                    currentIndex += direction;

                    //we've passed our end goal
                    if ((0 > currentIndex || currentIndex >= PointPositions.Count))
                    {
                        trans.position = currentdestination;
                        break;
                    }

                    previousPosition = currentdestination;
                    currentdestination = PointPositions[currentIndex];

                    currentDist = currentDist - goalDist;
                    goalDist = Vector3.Distance(previousPosition, currentdestination);
                    goalDirection = (currentdestination - previousPosition).normalized;
                }

                position = previousPosition + (goalDirection * currentDist);

                trans.position = position;

                yield return null;
            }

            onFinish?.Invoke();
        }

#if UNITY_EDITOR
        private void Update()
        {
            //If our points list has changed, update pipe data
            if (PositionsHasChanged())
            {
                CalculatePointsFromContainer();
                Debug.Log("Updating Pipe Points");
            }

            //If our offset has been changed, update cap positions
            if (_previousPipeCapOffset != PipeCapOffset)
                UpdatePipeCaps();
        }
#endif

        public enum StartingPoint
        {
            Auto,
            StartPipe,
            EndPipe,
        }
    }
}
