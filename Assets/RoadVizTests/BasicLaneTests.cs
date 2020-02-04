using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BasicLaneTests
    {
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        // Nathan wrote this
        // tests whether or not a lane's width actually changes
        public IEnumerator laneWidthChanges()
        {
            // Arrange
            GameObject lane = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/LanePrefab/BasicLane/BasicLane"));
            BasicLane laneScript = (BasicLane)lane.GetComponent("BasicLane");
            // Act
            float initialWidth = laneScript.getLaneWidth();
            laneScript.setLaneWidth(5f);
            // Assert
            Assert.IsFalse(laneScript.getLaneWidth() == initialWidth);
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
