using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Prebuiltsetupattribute
    {
        [Test]
        public void LogAssertExample()
        {
            //Expect a regular log message
            LogAssert.Expect(LogType.Log, "Log message");
            //A log message is expected so without the following line
            //the test would fail
            Debug.Log("Log message");
            //An error log is printed
            Debug.LogError("Error message");
            //Without expecting an error log, the test would fail
            LogAssert.Expect(LogType.Error, "Error message");
        }
    }
}