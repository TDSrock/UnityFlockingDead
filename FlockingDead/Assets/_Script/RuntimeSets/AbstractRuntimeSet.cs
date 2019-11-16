using System.Collections.Generic;
using UnityEngine;

namespace SjorsGielen.CustomVariables.RuntimeSets
{
    public abstract class AbstractRuntimeSet<T> : ScriptableObject
    {
        public List<T> Items = new List<T>();

        public bool Add(T objectToAdd)
        {
            //potentialy improve code below by using something else then List
            if (!Items.Contains(objectToAdd))
            {
                Items.Add(objectToAdd);
                return true;
            }
            return false;
        }

        public bool Remove(T objectToRemove)
        {
            if (Items.Contains(objectToRemove))
            {
                Items.Remove(objectToRemove);
                return true;
            }
            return false;
        }

        /// <summary>
        /// On startup of the game disabled is called first.
        /// To ensure no mistakes were made we clear the list.
        /// If any mistakes were made we log a warning.
        /// </summary>
        private void OnDisable()
        {
#if UNITY_EDITOR
            if (Items.Count != 0)
                Debug.LogWarning(this + "Was not empty, clearing it for you. " +
                    "Make sure that the adder contains OnDisable method in which it removes the type from the runtime set!");
#endif
            Items.Clear();
        }
    }
}