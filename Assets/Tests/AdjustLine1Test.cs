using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class adjustLine1Test
    {
        public GameObject lanePrefab = GameObject.Find("Lane_Solid_Lines_Basic");
        public GameObject asphalt    = GameObject.Find("PrimaryAsphalt");
        public GameObject line1      = GameObject.Find("Line1");
        public GameObject line2      = GameObject.Find("Line2");

        // A Test behaves as an ordinary method
        [Test]
        public void adjustSize_Test()
        {
            // Generate a random Z Position
            bool testPassed = false;
            float randomZPos = Random.Range(1, 500);

            // Act on moving the asphalt
            lanePrefab.transform.Translate(new Vector3(0, 0, randomZPos));

            //Retrieve values for the Z position and the expectation.
            var line1ZPos = line1.transform.position.z;
            var asphaltZPos = asphalt.transform.position.z;
            var expectedLine1ZPos = asphaltZPos + 1.575;

            // Verify that the position of the line is correct
            // Along with the asphalt
            if (!(line1ZPos > expectedLine1ZPos + 0.01) && !(line1ZPos < expectedLine1ZPos - 0.01))
            {
                testPassed = true;
            }

            Assert.IsTrue(testPassed);


        }
    }
}
