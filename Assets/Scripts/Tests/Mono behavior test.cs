using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Monobehaviortest
    {
        [UnityTest]
        public IEnumerator MonoBehaviourTest_Works()
        {
            yield return new MonoBehaviourTest<MyMonoBehaviourTest>();
        }

        public class MyMonoBehaviourTest : MonoBehaviour, IMonoBehaviourTest
        {
            private int frameCount;
            public bool IsTestFinished
            {
                get { return frameCount > 10; }
            }

            void Update()
            {
                frameCount++;
            }
        }
    }
}