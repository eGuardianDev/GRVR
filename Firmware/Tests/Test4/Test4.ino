
#include "Simple_MPU6050.h"

#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <WiFiClient.h>
WiFiClient client;
const uint16_t port = 8585;
const char *host = "192.168.0.103";
#define MPU6050_ADDRESS_AD0_LOW     0x68 // address pin low (GND), default for InvenSense evaluation board
#define MPU6050_ADDRESS_AD0_HIGH    0x69 // address pin high (VCC)
#define MPU6050_DEFAULT_ADDRESS     MPU6050_ADDRESS_AD0_LOW
#define Three_Axis_Quaternions 3
#define Six_Axis_Quaternions 6  // Default
Simple_MPU6050 mpu(Six_Axis_Quaternions);

String macAdress;
#define NumberOfMPUs 2


/*             _________________________________________________________*/
//               X Accel  Y Accel  Z Accel   X Gyro   Y Gyro   Z Gyro
//#define OFFSETS  -5260,    6596,    7866,     -45,       5,      -9  // My Last offsets.
//       You will want to use your own as these are only for my specific MPU6050.
/*             _________________________________________________________*/

//***************************************************************************************
//******************                Print Functions                **********************
//***************************************************************************************

#define spamtimer(t) for (static uint32_t SpamTimer; (uint32_t)(millis() - SpamTimer) >= (t); SpamTimer = millis()) // (BLACK BOX) Ya, don't complain that I used "for(;;){}" instead of "if(){}" for my Blink Without Delay Timer macro. It works nicely!!!
//recision is the number of digits after the decimal point set to zero for intergers



void PrintEuler(int32_t *quat, uint16_t SpamDelay = 100) {
    if (!client.connect(host, port))
    {
        Serial.println("Connection to host failed");
        delay(1000);
        return;
    }
//   
//    while (client.available() > 0)
//    {
//        char c = client.read();
//        Serial.write(c);
//    }

  
  Quaternion q;
  float euler[3];         // [psi, theta, phi]    Euler angle container
  float eulerDEG[3];         // [psi, theta, phi]    Euler angle container
  int activeMPU;
  spamtimer(SpamDelay) {// non blocking delay before printing again. This skips the following code when delay time (ms) hasn't been met
    mpu.GetQuaternion(&q, quat);
    mpu.GetEuler(euler, &q);
    mpu.ConvertToDegrees(euler, eulerDEG);
    if(digitalRead(14) == HIGH) activeMPU = 0; // Serial.println("Up14");
    else if(digitalRead(12) == HIGH) activeMPU = 1; // Serial.println("Up12");
  
    String data = macAdress + " " + activeMPU + " " + String( eulerDEG[0] )+ " " +  String(eulerDEG[1] )+ " " +  String(eulerDEG[2]) ;
    client.println(data);
    client.stop();
//    Serial.printfloatx(F("euler  ")  , eulerDEG[0], 9, 4, F(",   ")); //printfloatx is a Helper Macro that works with Serial.print that I created (See #define above)
//    Serial.printfloatx(F("")       , eulerDEG[1], 9, 4, F(",   "));
//    Serial.printfloatx(F("")       , eulerDEG[2], 9, 4, F("\n"));
  }
  
//    Serial.print('\n');
    
}

//***************************************************************************************
//******************              Callback Function                **********************
//***************************************************************************************


void print_Values (int16_t *gyro, int16_t *accel, int32_t *quat) {
  uint8_t Spam_Delay = 10; // Built in Blink without delay timer preventing Serial.print SPAM
 
   PrintEuler(quat, Spam_Delay);
}

void printBoardInformation(){
  if(!Serial) {return;}
  Serial.print("ESP Board MAC Address:  ");
  Serial.println(macAdress);
  Serial.print("Active MPUs: ");
  Serial.println(NumberOfMPUs);
}
void setup() {
    macAdress = WiFi.macAddress();
    Serial.begin(115200);
    printBoardInformation();
    Serial.print("Connecting\n");
    WiFi.mode(WIFI_STA);
    WiFi.begin("", ""); // change it to your ussid and password
    while (WiFi.status() != WL_CONNECTED)
    {
        delay(500);
        Serial.print(".");
    }


  
  // initialize serial communication
  while (!Serial); // wait for Leonardo enumeration, others continue immediately
  Serial.println(F("Start:"));

  // mpu.begin(); <-- Upload error 
  
  // Seting up MPU settings
  mpu.SetAddress(MPU6050_DEFAULT_ADDRESS);
    mpu.Set_DMP_Output_Rate_Hz(5);           // Set the DMP output rate from 200Hz to 5 Minutes.

 //Setting Pin Modes
  pinMode(14, OUTPUT);
  pinMode(12, OUTPUT);
 // Calibration
  digitalWrite(14, HIGH);
  digitalWrite(12, LOW);
  mpu.CalibrateMPU().load_DMP_Image();// Does it all for you with Calibration
  digitalWrite(14, LOW);
  digitalWrite(12, HIGH);
  mpu.CalibrateMPU().Enable_Reload_of_DMP(Three_Axis_Quaternions).load_DMP_Image();// Does it all for you with Calibration
  
  //What to do when information is received
  mpu.on_FIFO(print_Values);
  
}

void loop() {
  //Looping from one MPU to another MPU
  digitalWrite(14, !digitalRead(14));
  digitalWrite(12, !digitalRead(12));
  mpu.dmp_read_fifo(0);// Must be in loop
  delay(10); // this delay is very important. Otherwise there my be skippes
  
}
