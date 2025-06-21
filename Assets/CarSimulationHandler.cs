using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarSimulationHandler : MonoBehaviour
{
    public ButtonBehavior respawnCarButton;
    private bool respawnHandled = false;

    public ButtonBehavior startCarButton;
    public float velocity = 2;

    public GameObject car;

    private Vector3 resetPos = Vector3.zero;
    private Quaternion resetRot = new Quaternion(0, 0, 0, 0);
    private Vector3 carPos;
    private Rigidbody rb;


    //graph stuff
    private static Vector3 graphOrigin = new Vector3(-13.85f, 1.29f, -20.18f);
    private static Vector3 graphXMax = new Vector3(-13.9f, 4.32f, -20.2f);
    private static Vector3 graphTMax = new Vector3(-13.9f, 1.22f, -13.97f);

    private Vector3 graphCurrentX = graphOrigin;
    private Vector3 graphCurrentT = graphOrigin;

    private float xComponent;
    private static float xMaxDistance = 14.3f;
    public float timeElapsed = 0f;
    private float timeDuration = 5f;

    //graph import
    public GameObject graph;

    private Transform currentPoint;
    private Transform XMarker;
    private TextMeshProUGUI xTMP;
    private Transform TMarker;
    private TextMeshProUGUI tTMP;
    public GameObject Point;

    //bounds
    private float xMax = graphXMax.y - graphOrigin.y;
    private float timeMax = graphTMax.z - graphOrigin.z;

    void Start()
    {
        rb = car.GetComponent<Rigidbody>();
        carPos = car.transform.position;
        graphCurrentX = graphOrigin;
        graphCurrentT = graphOrigin;
        currentPoint = graph.transform.Find("CurrentPoint");
        XMarker = graph.transform.Find("XMarker");
        TMarker = graph.transform.Find("TimeMarker");

        xTMP = XMarker.GetComponentInChildren<TextMeshProUGUI>();
        tTMP = TMarker.GetComponentInChildren<TextMeshProUGUI>();

    }

    void Update()
    {
        //update car position vector
        carPos = car.transform.position;
        //calculating graph point position
        xComponent = carPos.z;

        if (respawnCarButton.pressed && !respawnHandled)
        {
            car.transform.position = resetPos;
            car.transform.rotation = resetRot;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            timeElapsed = 0;
            respawnHandled = true;
        }
        else if (!respawnCarButton.pressed)
        {
            respawnHandled = false;
        }

        //when button pressed, start car w set velocity
        if (startCarButton.pressed)
        {
            rb.velocity = new Vector3(0, 0, velocity);
        }

        //increase timer
        if (xComponent < xMaxDistance && rb.velocity.z > 0)
        {
            timeElapsed += Time.deltaTime;
        }

        if (xComponent > xMaxDistance)
        {
            rb.velocity = Vector3.zero;
        }



        //dont allow to rotate
        rb.angularVelocity = Vector3.zero;

        DetermineGraphCoord();
        UpdateText();

    }

    void DetermineGraphCoord()
    {

        graphCurrentX = new Vector3(0, xMax / xMaxDistance * xComponent, 0);
        float actualMaxTime = xMaxDistance / velocity;
        graphCurrentT = new Vector3(0, 0, timeMax / actualMaxTime * timeElapsed);

        currentPoint.transform.position = graphOrigin + graphCurrentX + graphCurrentT;

        XMarker.transform.position = graphOrigin + graphCurrentX;
        TMarker.transform.position = graphOrigin + graphCurrentT;
    }

    void UpdateText()
    {
        float actualMaxTime = xMaxDistance / velocity;
        xTMP.text = RoundToTenths(xMax / xMaxDistance * xComponent).ToString();
        tTMP.text = RoundToTenths(timeMax / actualMaxTime * timeElapsed).ToString();
    }
    float RoundToTenths(float value)
{
    return Mathf.Round(value * 10f) / 10f;
}




}
