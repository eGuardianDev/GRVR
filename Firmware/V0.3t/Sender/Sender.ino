
//custom library
#include <WString.h>   
#include <Arduino.h>

#include <ESP8266WiFi.h>
#include <espnow.h>

uint8_t broadcastAddress[] = {0xC8, 0xC9, 0xA3, 0x09, 0xE6, 0xB6};




#include "Simple_MPU6050.h"

//arduino libraries

//Gyroscope variables
#define MPU6050_ADDRESS_AD0_LOW     0x68
#define MPU6050_ADDRESS_AD0_HIGH    0x69
#define MPU6050_DEFAULT_ADDRESS     MPU6050_ADDRESS_AD0_LOW
#define Three_Axis_Quaternions 3
#define Six_Axis_Quaternions 6  // Default
Simple_MPU6050 mpu(Six_Axis_Quaternions);


String macAdress;
#define NumberOfMPUs 2





#define spamtimer(t) for (static uint32_t SpamTimer; (uint32_t)(millis() - SpamTimer) >= (t); SpamTimer = millis()) 



typedef struct struct_message {
    float x1;
    float y1;
    float z1;
    float x2;
    float y2;
    float z2;
    int code;
} struct_message;

struct_message myData;

unsigned long lastTime = 0;  
unsigned long timerDelay = 100;  // send readings timer

// Callback when data is sent
void OnDataSent(uint8_t *mac_addr, uint8_t sendStatus) {
  Serial.print("Last Packet Send Status: ");
  if (sendStatus == 0){
    Serial.println("Delivery success");
  }
  else{
    Serial.println("Delivery fail");
  }
}

void SendEuler(int32_t *quat, uint16_t SpamDelay = 100) {
    
 


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
   
    myData.x1 =  xyz[0];
    myData.y1 =  xyz[1] ;
    myData.z1 =  xyz[2] ;
   }
  else if(digitalRead(12) == HIGH) {
      activeMPU = 1; 
    
    
    myData.x2 =  xyz[0];
    myData.y2 =  xyz[1] ;
    myData.z2 =  xyz[2] ;
    myData.code = 10000;

    esp_now_send(broadcastAddress, (uint8_t *) &myData, sizeof(myData));

    }
    
  }
}
  

//some board information
void printBoardInformation(){
  if(!Serial) {return;}
  Serial.print("ESP Board MAC Address:  ");
  Serial.println(macAdress);
  Serial.print("Active MPUs: ");
  Serial.println(NumberOfMPUs);
}

void print_Values (int16_t *gyro, int16_t *accel, int32_t *quat) {
  uint8_t Spam_Delay = 10; 
   SendEuler(quat, Spam_Delay);
}


 
void setup() {
  
  Serial.begin(115200);
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

  WiFi.mode(WIFI_STA);

  if (esp_now_init() != 0) {
    Serial.println("Error initializing ESP-NOW");
    return;
  }

  esp_now_set_self_role(ESP_NOW_ROLE_CONTROLLER);
  esp_now_register_send_cb(OnDataSent);
  
  esp_now_add_peer(broadcastAddress, ESP_NOW_ROLE_SLAVE, 1, NULL, 0);
}
 
void loop() {
  
  mpu.dmp_read_fifo(0);// Must be in loop
  
  delay(10); 
    
}
