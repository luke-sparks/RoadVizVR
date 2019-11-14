using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class RoadTests
    {
        private Road roadScript;
        // A Test behaves as an ordinary method
        [Test]
        public void RoadTestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator RoadTestsWithEnumeratorPasses()
        {
            // arrange
        
            //GameObject basicLane = (GameObject) Resources.Load<Object>("../BasicLane");
            GameObject invalidLane = (GameObject) Resources.Load("TestObject");
            // act
            //bool basicLaneIsValid = roadScript.isValidLaneType(basicLane);
            bool isInvalid = roadScript.isValidLaneType(invalidLane);
            // assert
            //Assert.IsTrue(basicLaneIsValid);
            Assert.IsFalse(isInvalid);
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
