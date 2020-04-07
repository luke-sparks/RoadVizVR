// RoadVizSaveSystem.cs
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// Note: the tutorial I found for saving/loading in Unity
// used binary files to save and load data.
// Unfortunately, this makes it so we cannot store
// unity datatypes (e.g. GameObject)
public static class RoadVizSaveSystem //: MonoBehavior
{
    // Nathan wrote this
    // saves the road in a binary file
    // road is the script of the road object in the development environment
    public static void saveRoad(Road road)
    {
        // create a binary formatter for file conversion
        BinaryFormatter roadFormatter = new BinaryFormatter();
        // create a path so we know where our saved file is being stored
        string savePath = getDataPath("/road.rvvr");
        Debug.Log(savePath);
        // create a file stream to save our file
        FileStream roadStream = new FileStream(savePath, FileMode.Create);
        // obtain the road data
        RoadData roadData = new RoadData(road);
        // store the road data as a binary file
        roadFormatter.Serialize(roadStream, roadData);
        // close the file stream
        roadStream.Close();
    }

    // Nathan wrote this
    // loads the road from a saved binary file
    public static RoadData loadRoadFromMemory()
    {
        string loadPath = getDataPath("/road.rvvr");
        if(File.Exists(loadPath))
        {
            // create a binary formatter for file conversion
            BinaryFormatter roadFormatter = new BinaryFormatter();
            // create a path to the file we want to load
            FileStream roadStream = new FileStream(loadPath, FileMode.Open);
            // convert the binary file and store it as a RoadData object
            RoadData roadData = roadFormatter.Deserialize(roadStream) as RoadData;
            // close the file stream
            roadStream.Close();
            // return our loaded data
            return roadData;
        }
        else
        {
            Debug.LogError("Save file not found in: " + loadPath);
            return null;
        }
    }

    // Nathan wrote this
    // obtains a datapath
    private static string getDataPath(string endOfPath)
    {
        return Application.persistentDataPath + endOfPath;
    }
}
