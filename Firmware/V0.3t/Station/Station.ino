
#include <ESP8266WiFi.h>
#include <espnow.h>

int stationMode = 0;
// 0 - connecting
// 1 - ESP_NOW initilization
// 2 - sensor data transmition for each sensor


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

void OnDataRecv(uint8_t * mac, uint8_t *incomingData, uint8_t len) {
  if(stationMode != 2 ) return;
  memcpy(&myData, incomingData, sizeof(myData));
 // memcpy(&ReceivedAdd, mac, sizeof(ReceivedAdd));
 // Serial.print("Bytes received: ");
  
}
 
void setup() {
  Serial.begin(115200);
  
  WiFi.mode(WIFI_STA);

  if (esp_now_init() != 0) {
    Serial.println("Error initializing ESP-NOW");
    return;
  }
  
  esp_now_set_self_role(ESP_NOW_ROLE_SLAVE);
  esp_now_register_recv_cb(OnDataRecv);
}

void loop() {
    //waiting for setup
  if(stationMode == 0){
    
    Serial.println("0");
    if(Serial.available() > 0){
      stationMode = 1;
      
    }


    
  }else  if(stationMode == 1){
   //setting up stuff
   
   //setuped
   stationMode = 2;
  }else if (stationMode == 2){
    //transferring information
    Serial.print(myData.code);
    Serial.print(" ");
    Serial.print (myData.x1);
    Serial.print (" ");
    Serial.print (myData.y1);
    Serial.print (" ");
    Serial.print (myData.z1);
    Serial.print (" ");
    Serial.print (myData.x2);
    Serial.print (" ");
    Serial.print (myData.y2);
    Serial.print (" ");
    Serial.println (myData.z2);
  }
  
}
