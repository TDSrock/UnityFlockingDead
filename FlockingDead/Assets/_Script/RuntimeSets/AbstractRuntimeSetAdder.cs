using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SjorsGielen.CustomVariables.RuntimeSets
{
    public abstract class AbstractRuntimeSetAdder<T> : MonoBehaviour
    {

        public abstract AbstractRuntimeSet<T> RuntimeSet
        {
            get;
        }

        public abstract T RuntimesetType
        { get;}

        public void OnEnable()
        {
            RuntimeSet.Add(RuntimesetType);
        }

        private void OnDisable()
        {
            RuntimeSet.Remove(RuntimesetType);
        }

    }

}