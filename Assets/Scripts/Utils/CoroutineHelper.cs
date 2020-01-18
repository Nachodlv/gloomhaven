using System;
using System.Collections;
using UnityEngine;

namespace Utils
{
    public static class CoroutineHelper
    {
        /// <summary>
        /// Wait for the quantity of seconds passed as parameter and then executes the action.
        /// </summary>
        /// <param name="monoBehaviour">The MonoBehaviour where the coroutine will take place</param>
        /// <param name="seconds">The amount of seconds that the coroutine will wait to execute the action</param>
        /// <param name="action">The function that will be executed</param>
        public static void WaitForSeconds(MonoBehaviour monoBehaviour, float seconds, Action action)
        {
            monoBehaviour.StartCoroutine(EnumerableWaitForSeconds(seconds, action));    
        }

        private static IEnumerator EnumerableWaitForSeconds(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action();
            yield return null;
        }
    }
}