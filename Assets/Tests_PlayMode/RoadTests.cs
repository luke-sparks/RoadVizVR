using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class RoadTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void RoadTestsSimplePasses()
        {
            //Instantiate objects to test
            GameObject testRoad = new GameObject("TestRoad");
            Road roadScript = testRoad.AddComponent(typeof(Road)) as Road;
            GameObject[] laneTypes = new GameObject[1];
            float shift = 2.75f;

            //Add one lane at position 0
            roadScript.insertLaneAtEnd(laneTypes[0], 0f);

            //Add a random number of lanes
            float randomLaneNum = Random.Range(0, 50);
            for (int i = 0; i < randomLaneNum; i++)
            {
                roadScript.insertLaneAtEnd(laneTypes[0], shift);
                shift += 2.75f;
            }



        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        /*
        [UnityTest]
        public IEnumerator isValidLaneTypeWithEnumeratorPasses()
        {
            // arrange
            GameObject testRoad = new GameObject("TestRoad");
            Road roadScript = testRoad.AddComponent(typeof(Road)) as Road;
            //GameObject basicLane = (GameObject) Resources.Load<Object>("../BasicLane");
            GameObject invalidLane = GameObject.Instantiate(Resources.Load("Assets/Tests/TestObject.prefab", typeof(GameObject))) as GameObject;
            // act
            //bool basicLaneIsValid = roadScript.isValidLaneType(basicLane);
            bool isInvalid = roadScript.isValidLaneType(invalidLane);
            // assert
            //Assert.IsTrue(basicLaneIsValid);
             Assert.IsFalse(isInvalid);
            // Use yield to skip a frame.
            yield return null;
        }
        */
    }
}
