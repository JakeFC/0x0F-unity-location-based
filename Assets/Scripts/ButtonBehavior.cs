using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    public Text coordinates;
    public Text distance;
    public GameObject marker;
    struct coords {
        public float latitude;
        public float longitude;
        public float altitude;
    };
    private coords _lastCoordinates;
    private GameObject _clone;

    IEnumerator Start()
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
            yield break;

        // Starts the location service.
        Input.location.Start(0.00001f, 0.1f);

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }
    }

    public void GetCurrent()
    {
        coordinates.text = String.Format("Current Longitude: {0}\nCurrent Latitude: {1}\nCurrent Altitude: {2}", Input.location.lastData.longitude, Input.location.lastData.latitude, Input.location.lastData.altitude);
    }

    public void SetDestination()
    {
        _lastCoordinates.latitude = Input.location.lastData.latitude;
        _lastCoordinates.longitude = Input.location.lastData.longitude;
        _lastCoordinates.altitude = Input.location.lastData.altitude;
    }

    public void GetDistance()
    {
        distance.text = String.Format("Distance: {0} m", Math.Abs(_lastCoordinates.latitude - Input.location.lastData.latitude +
                                                                _lastCoordinates.longitude - Input.location.lastData.longitude +
                                                                _lastCoordinates.altitude - Input.location.lastData.altitude) / 3);
    }

    public void DrawObject()
    {
        _clone = Instantiate(marker, transform);
        _clone.transform.parent = null;
        marker.GetComponent<Canvas>().GetComponent<Text>().text = Console.ReadLine();
    }
}
