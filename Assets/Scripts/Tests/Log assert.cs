using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Logassert
    {
        public class TestsWithPrebuildStep : IPrebuildSetup
        {
            public void Setup()
            {
                // Run this code before the tests are executed
            }

            [Test]
            //PrebuildSetupAttribute can be skipped because it's implemented in the same class
            [PrebuildSetup(typeof(TestsWithPrebuildStep))]
            public void Test()
            {
                
            }
        }
    }
}