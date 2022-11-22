
//custom library
#include <WString.h>   
#include "NetworkingData.h"



#include "Simple_MPU6050.h"

//arduino libraries
#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <WiFiClient.h>


// important variables
const uint16_t port = 5005;
const char *host = "192.168.0.103";
//Gyroscope variables
#define MPU6050_ADDRESS_AD0_LOW     0x68
#define MPU6050_ADDRESS_AD0_HIGH    0x69
#define MPU6050_DEFAULT_ADDRESS     MPU6050_ADDRESS_AD0_LOW
#define Three_Axis_Quaternions 3
#define Six_Axis_Quaternions 6  // Default
Simple_MPU6050 mpu(Six_Axis_Quaternions);

String macAdress;
#define NumberOfMPUs 2

WiFiClient client;



#define spamtimer(t) for (static uint32_t SpamTimer; (uint32_t)(millis() - SpamTimer) >= (t); SpamTimer = millis()) 




String data;
void SendEuler(int32_t *quat, uint16_t SpamDelay = 100) {
    
   //check connections
    if (!client.connect(host, port))
    {
        Serial.println("Connection to host failed");
        delay(100);
        return;
    }


  //caculate angles
  Quaternion q;
  VectorFloat gravity;
  float ypr[3] = { 0, 0, 0 };
  float xyz[3] = { 0, 0, 0 };  int activeMPU;
  spamtimer(SpamDelay) {
    mpu.GetQuaternion(&q, quat);
    mpu.GetGravity(&gravity, &q);
    mpu.GetYawPitchRoll(ypr, &q, &gravity);
    mpu.ConvertToDegrees(ypr, xyz);
   

  digitalWrite(14, !digitalRead(14));
  digitalWrite(12, !digitalRead(12)); 
   if(digitalRead(14) == HIGH){
    activeMPU = 0; 
    data = macAdress + " " + activeMPU + " " + String( xyz[0 ] )+ " " +  String(xyz[1] )+ " " +  String(xyz[2]) ;
   
   }
  else if(digitalRead(12) == HIGH) {
      activeMPU = 1; 
    
    data.concat( " " + String(activeMPU)+ " " + String( xyz[0 ] )+ " " +  String(xyz[1] )+ " " +  String(xyz[2])) ;
     client.println(data);

    }
    
  }
}
  

void print_Values (int16_t *gyro, int16_t *accel, int32_t *quat) {
  uint8_t Spam_Delay = 10; 
   SendEuler(quat, Spam_Delay);
}


//some board information
void printBoardInformation(){
  if(!Serial) {return;}
  Serial.print("ESP Board MAC Address:  ");
  Serial.println(macAdress);
  Serial.print("Active MPUs: ");
  Serial.println(NumberOfMPUs);
}

void setup()
{

  
  Serial.begin(115200);

  while (!Serial); 

  Serial.print("Connecting...\n");
  WiFi.mode(WIFI_STA);
  WiFi.begin(NetworkData :: GetNetworkName(),NetworkData :: GetNetworkPass());
  while (WiFi.status() != WL_CONNECTED)
  {
      delay(500);
      Serial.print(".");
  }
  Serial.println("\n Successful Connected to the internet.");

  Serial.println("Board information:");
  printBoardInformation();

  //Netowork is ready to be used;



  Serial.println(F("Starting calibrations:"));
  mpu.SetAddress(MPU6050_DEFAULT_ADDRESS);
  mpu.Set_DMP_Output_Rate_Hz(25);           // Set the DMP output rate from 200Hz to 5 Minutes.

 //Setting Pin Modes
  pinMode(14, OUTPUT);
  pinMode(12, OUTPUT);

 // Calibration gyro 1
  digitalWrite(14, HIGH);
  digitalWrite(12, LOW);
  mpu.CalibrateMPU().load_DMP_Image();// Does it all for you with Calibration

 // Calibration gyro 2
  digitalWrite(14, LOW);
  digitalWrite(12, HIGH);
  mpu.CalibrateMPU().Enable_Reload_of_DMP(Three_Axis_Quaternions).load_DMP_Image();// Does it all for you with Calibration
  
  //What to do when information is received
  mpu.on_FIFO(print_Values);

}

void loop()
{


  //Looping from one MPU to another MPU
  
  mpu.dmp_read_fifo(0);// Must be in loop
  
  delay(10); 
    
}
