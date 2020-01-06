using System;
using System.Collections;
using UnityEngine;

namespace Utils
{
    public static class CoroutineHelper
    {
        public static void WaitForSeconds(MonoBehaviour monoBehaviour, float seconds, Action action)
        {
            monoBehaviour.StartCoroutine(EnumerableWaitForSeconds(seconds, action));    
        }

        private static IEnumerator EnumerableWaitForSeconds(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action();
        }
    }
}