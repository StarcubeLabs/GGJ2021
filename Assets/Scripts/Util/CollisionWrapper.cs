﻿namespace GGJ2021
{
    using UnityEngine;

    /// <summary>
    /// This class allows us to decouple other classes from their collision gameobjects and functions.
    /// This means that we can store classes and colliders on different gameobjects, with the class referencing the colliders through CollisionWrapper.
    /// By doing this, a single class can reference multiple colliders.
    /// When a collision is detected on this gameobject, the pertinent delegates will be called.
    /// It is up to other classes to set up and manage these callbacks to perform collision logic.
    /// </summary>
    public class CollisionWrapper : MonoBehaviour
    {
        public Collider2D col;
        public bool ignoreCollisionsWithOtherTriggers = true;
        public bool useLayerMask = true;
        public LayerMask mask;

        public GameObject[] objectsToIgnore;
        public string[] namesToIgnore;

        private bool isTrigger;
        public bool isActive = true;

        public delegate void OnTriggerEnterDelegate(Collider2D other);
        OnTriggerEnterDelegate onTriggerEnterDelegate;
        public delegate void OnTriggerStayDelegate(Collider2D other);
        OnTriggerStayDelegate onTriggerStayDelegate;
        public delegate void OnTriggerExitDelegate(Collider2D other);
        OnTriggerExitDelegate onTriggerExitDelegate;

        public delegate void OnCollisionEnterDelegate(Collision2D other);
        OnCollisionEnterDelegate onCollisionEnterDelegate;
        public delegate void OnCollisionStayDelegate(Collision2D other);
        OnCollisionStayDelegate onCollisionStayDelegate;
        public delegate void OnCollisionExitDelegate(Collision2D other);
        OnCollisionExitDelegate onCollisionExitDelegate;

        // Start is called before the first frame update
        private void Awake()
        {
            if (col == null)
            {
                col = GetComponent<Collider2D>();
            }
            isTrigger = col.isTrigger;
        }

        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
        }

        private bool ShouldIgnoreObject(GameObject obj)
        {
            foreach (GameObject objectToIgnore in objectsToIgnore)
            {
                if (objectToIgnore == obj)
                {
                    return true;
                }
            }
            foreach (string nameToIgnore in namesToIgnore)
            {
                if (nameToIgnore == obj.name)
                {
                    return true;
                }
            }
            return false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isActive && isTrigger && ShouldIgnoreObject(other.gameObject) == false)
            {
                if (ignoreCollisionsWithOtherTriggers == true && other.isTrigger == true)
                {
                    return;
                }
                if (useLayerMask == false || (mask == (mask | 1 << other.gameObject.layer)))
                {
                    if (onTriggerEnterDelegate != null)
                    {
                        onTriggerEnterDelegate(other);
                    }
                }
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (isActive && isTrigger && ShouldIgnoreObject(other.gameObject) == false)
            {
                if (ignoreCollisionsWithOtherTriggers == true && other.isTrigger == true)
                {
                    return;
                }
                if (useLayerMask == false || (mask == (mask | 1 << other.gameObject.layer)))
                {
                    if (onTriggerStayDelegate != null)
                    {
                        onTriggerStayDelegate(other);
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (isActive && isTrigger && ShouldIgnoreObject(other.gameObject) == false)
            {
                if (ignoreCollisionsWithOtherTriggers == true && other.isTrigger == true)
                {
                    return;
                }
                if (useLayerMask == false || (mask == (mask | 1 << other.gameObject.layer)))
                {
                    if (onTriggerExitDelegate != null)
                    {
                        onTriggerExitDelegate(other);
                    }
                }
            }
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (isActive && isTrigger == false && ShouldIgnoreObject(other.gameObject) == false)
            {
                if (ignoreCollisionsWithOtherTriggers == true && other.collider.isTrigger == true)
                {
                    return;
                }
                if (useLayerMask == false || (mask == (mask | 1 << other.gameObject.layer)))
                {
                    if (onCollisionEnterDelegate != null)
                    {
                        onCollisionEnterDelegate(other);
                    }
                }
            }
        }

        void OnCollisionStay2D(Collision2D other)
        {
            if (isActive && isTrigger == false && ShouldIgnoreObject(other.gameObject) == false)
            {
                if (ignoreCollisionsWithOtherTriggers == true && other.collider.isTrigger == true)
                {
                    return;
                }
                if (useLayerMask == false || (mask == (mask | 1 << other.gameObject.layer)))
                {
                    if (onCollisionStayDelegate != null)
                    {
                        onCollisionStayDelegate(other);
                    }
                }
            }
        }

        void OnCollisionExit2D(Collision2D other)
        {
            if (isActive && isTrigger == false && ShouldIgnoreObject(other.gameObject) == false)
            {
                if (ignoreCollisionsWithOtherTriggers == true && other.collider.isTrigger == true)
                {
                    return;
                }
                if (useLayerMask == false || (mask == (mask | 1 << other.gameObject.layer)))
                {
                    if (onCollisionExitDelegate != null)
                    {
                        onCollisionExitDelegate(other);
                    }
                }
            }
        }

        // **********************
        //   Delegate functions
        // **********************

        //onTriggerEnterDelegate functions
        public void AssignFunctionToTriggerEnterDelegate(OnTriggerEnterDelegate func)
        {
            onTriggerEnterDelegate += func;
        }

        public void RemoveFunctionFromTriggerEnterDelegate(OnTriggerEnterDelegate func)
        {
            onTriggerEnterDelegate -= func;
        }

        public void ClearTriggerEnterDelegate()
        {
            onTriggerEnterDelegate = null;
        }

        //onTriggerStayDelegate functions
        public void AssignFunctionToTriggerStayDelegate(OnTriggerStayDelegate func)
        {
            onTriggerStayDelegate += func;
        }

        public void RemoveFunctionFromTriggerStayDelegate(OnTriggerStayDelegate func)
        {
            onTriggerStayDelegate -= func;
        }

        public void ClearTriggerStayDelegate()
        {
            onTriggerStayDelegate = null;
        }

        //onTriggerExitDelegate functions
        public void AssignFunctionToTriggerExitDelegate(OnTriggerExitDelegate func)
        {
            onTriggerExitDelegate += func;
        }

        public void RemoveFunctionFromTriggerExitDelegate(OnTriggerExitDelegate func)
        {
            onTriggerExitDelegate -= func;
        }

        public void ClearTriggerExitDelegate()
        {
            onTriggerExitDelegate = null;
        }

        //onCollisionEnterDelegate functions
        public void AssignFunctionToCollisionEnterDelegate(OnCollisionEnterDelegate func)
        {
            onCollisionEnterDelegate += func;
        }

        public void RemoveFunctionFromCollisionEnterDelegate(OnCollisionEnterDelegate func)
        {
            onCollisionEnterDelegate -= func;
        }

        public void ClearCollisionEnterDelegate()
        {
            onCollisionEnterDelegate = null;
        }

        //onCollisionStayDelegate functions
        public void AssignFunctionToCollisionStayDelegate(OnCollisionStayDelegate func)
        {
            onCollisionStayDelegate += func;
        }

        public void RemoveFunctionFromCollisionStayDelegate(OnCollisionStayDelegate func)
        {
            onCollisionStayDelegate -= func;
        }

        public void ClearCollisionStayDelegate()
        {
            onCollisionStayDelegate = null;
        }

        //onCollisionExitDelegate functions
        public void AssignFunctionToCollisionExitDelegate(OnCollisionExitDelegate func)
        {
            onCollisionExitDelegate += func;
        }

        public void RemoveFunctionFromCollisionExitDelegate(OnCollisionExitDelegate func)
        {
            onCollisionExitDelegate -= func;
        }

        public void ClearCollisionExitDelegate()
        {
            onCollisionExitDelegate = null;
        }

        //General delegate functions
        public void ClearAllDelegates()
        {
            ClearTriggerEnterDelegate();
            ClearTriggerStayDelegate();
            ClearTriggerExitDelegate();
            ClearCollisionEnterDelegate();
            ClearCollisionStayDelegate();
            ClearCollisionExitDelegate();
        }
    }
}