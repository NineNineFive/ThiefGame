using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Unityplatformattribute
    {
        [Test]
        [UnityPlatform (RuntimePlatform.WindowsPlayer)]
        public void TestMethod1()
        {
            Assert.AreEqual(Application.platform, RuntimePlatform.WindowsPlayer);
        }

        [Test]
        [UnityPlatform(exclude = new[] {RuntimePlatform.WindowsEditor })]
        public void TestMethod2()
        {
            Assert.AreNotEqual(Application.platform, RuntimePlatform.WindowsEditor);
        }
    }
}