//
//            ,----,                 
//          ,/   .`|                 
//        ,`   .'  :    ,---,        
//      ;    ;     /   '  .' \       
//    .'___,/    ,'   /  ;    '.     
//    |    :     |   :  :       \    
//    ;    |.';  ;   :  |   /\   \   
//    `----'  |  |   |  :  ' ;.   :  
//        '   :  ;   |  |  ;/  \   \ 
//        |   |  '   '  :  | \  \ ,' 
//        '   :  |   |  |  '  '--'   
//        ;   |.'    |  :  :         
//        '---'      |  | ,'         
//                   `--''           
//                               

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.IO.Ports;


public class HandController : MonoBehaviour
{
    private SerialPort _serialPort;
    private Thread _serialReader;

    private bool windows = false;
    private string _serialPortPath = "";
    private const int BaudRate = 9600;
    private const int ValueAmount = 11;
    private const char ValueSeparator = '|';

    private bool isRunning;
    private const int ThreadSleepInMillis = 10;

    private float[] _values;

    public static bool Grabbing = false;

    // Fingers
    // Thumb Finger
    public Transform hand;
    public Transform thumb1;
    public Transform thumb2;

    // Index Finger
    public Transform index1;
    public Transform index2;
    public Transform index3;

    // Middle Finger
    public Transform middle1;
    public Transform middle2;
    public Transform middle3;

    // Ring Finger
    public Transform ring1;
    public Transform ring2;
    public Transform ring3;

    // Pinky Finger
    public Transform pinky1;
    public Transform pinky2;
    public Transform pinky3;

    // Current use for each Data Value 
    //
    // 0 = Thumb Angle      Angle
    // 1 = Index Finger     Angle
    // 2 = Middle Finger    Angle
    // 3 = Ring Finger      Angle
    // 4 = Pinky Finger     Angle
    //
    // 5 = Hand Angle X
    // 6 = Hand Angle Y
    // 7 = Hand Angle Z
    //
    // 8 = Hand Force X
    // 9 = Hand Force Y
    // 10 = Hand Force Z
    //
    // Total Amount of Values: 11 

    private void Start()
    {
        // Initialize Variables
        isRunning = true;
        _values = new float[ValueAmount];

        // Creating Serial
        _serialPortPath = windows ? "COM0" : "/dev/ttyUSB0";
        _serialPort = new SerialPort(_serialPortPath, BaudRate);
        _serialPort.Open();

        //Creating Thread
        _serialReader = new Thread(ReadData);
        _serialReader.Start();
    }

    public void Update()
    {
        // Rotate Hand
        hand.transform.Rotate(-_values[5] * Time.deltaTime, -_values[6] * Time.deltaTime, -_values[7] * Time.deltaTime );
        
        // Rotate Fingers
        thumb1.transform.localRotation = Quaternion.Euler(_values[0], 0f, 0f);
        thumb2.transform.localRotation = Quaternion.Euler(2f * _values[0], 0f, 0f);

        index1.transform.localRotation = Quaternion.Euler(_values[1], 0f, 0f);
        index2.transform.localRotation = Quaternion.Euler(2f * _values[1], 0f, 0f);
        index3.transform.localRotation = Quaternion.Euler(3f * _values[1], 0f, 0f);

        middle1.transform.localRotation = Quaternion.Euler(_values[2], 0f, 0f);
        middle2.transform.localRotation = Quaternion.Euler(2f * _values[2], 0f, 0f);
        middle3.transform.localRotation = Quaternion.Euler(3f * _values[2], 0f, 0f);

        ring1.transform.localRotation = Quaternion.Euler(_values[3], 0f, 0f);
        ring2.transform.localRotation = Quaternion.Euler(2f * _values[3], 0f, 0f);
        ring3.transform.localRotation = Quaternion.Euler(3f * _values[3], 0f, 0f);

        pinky1.transform.localRotation = Quaternion.Euler(_values[4], 0f, 0f);
        pinky2.transform.localRotation = Quaternion.Euler(2f * _values[4], 0f, 0f);
        pinky3.transform.localRotation = Quaternion.Euler(3f * _values[4], 0f, 0f);
        
        if (_values[0] + _values[1] + _values[2] + _values[3] + _values[4] >= 300)
        {
            Grabbing = true;
        }
        else
        {
            Grabbing = false;
        }
        
    }

    private void ReadData()
    {
        // Destroy Thread when exiting Program
        while (isRunning)
        {
            var current = _serialPort.ReadLine();
            Console.WriteLine(current);
            string[] readValues = current.Split(ValueSeparator);

            // Check if received values match the default pattern, if not restart loop
            // This is for the case of receiving a different serial print from the Arduino
            // otherwise the code could crash
            if (readValues.Length < ValueAmount || readValues.Length > ValueAmount)
            {
                Console.WriteLine("Incorrect Data length or setup");
                continue;
            }

            // Parsing all string values of the read values into the float array 'values'
            _values = Array.ConvertAll(readValues, float.Parse);

            // Pause the thread for x milliseconds to not uselessly waste computing power
            Thread.Sleep(ThreadSleepInMillis);
        }
    }
    
    void OnApplicationQuit()
    {
        isRunning = false;
    }

    public static bool IsGrabbing()
    {
        return Grabbing;
    }
}