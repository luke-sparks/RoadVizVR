using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendVehicleBehavior : MonoBehaviour, ISceneUIMenu
{
    private readonly string[] VEHICLE_OPTIONS = { "Bike", "Car", "Truck", "Bus" };
    private const int MPH_CHANGE_INCREMENT = 5;
    private BasicLane selectedLane;
    private Text speedField;
    private sendVehicle vehicleSender;
    private Dropdown vehicleSelectDd;

    public void init(GameObject[] basicLaneRef)
    {
        selectedLane = basicLaneRef[0].GetComponent<BasicLane>();

        speedField = transform.Find("SpeedControls/SpeedBackground/SpeedField").GetComponent<Text>();
        speedField.text = "15";

        vehicleSender = GameObject.Find("VehicleSender").GetComponent<sendVehicle>();

        vehicleSelectDd = transform.Find("VehicleSelectControls/VehicleType").GetComponent<Dropdown>();
        vehicleSelectDd.ClearOptions();
        vehicleSelectDd.AddOptions(new List<string>(VEHICLE_OPTIONS));
    }

    private void shiftSpeedField(int speedChange)
    {
        speedField.text = (int.Parse(speedField.text) + speedChange).ToString();
    }

    public void handleSpeedDecrease()
    {
        shiftSpeedField(-MPH_CHANGE_INCREMENT);

        if(int.Parse(speedField.text) <= 0)
        {
            speedField.text = "5";
        }
    }

    public void handleSpeedIncrease()
    {
        shiftSpeedField(MPH_CHANGE_INCREMENT);
    }

    public void handleSendSelect()
    {
        vehicleSender.updateCurrentVehicle(VEHICLE_OPTIONS[vehicleSelectDd.value].ToLower());
        vehicleSender.sendVehicleDownLane(int.Parse(speedField.text), selectedLane.gameObject);
    }

    public void handleVehicleTypeChange()
    {

    }

    public void closeUI()
    {
        Destroy(gameObject);
    }


}
