
//custom library
#include <WString.h>   
#include "NetworkingData.h"

//arduino libraries
#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <WiFiClient.h>


// important variables
const uint16_t port = 5005;
const char *host = "192.168.0.103";

WiFiClient client;


void setup()
{
    Serial.begin(115200);
    Serial.print("Connecting...\n");
    WiFi.mode(WIFI_STA);
    WiFi.begin(NetworkData :: GetNetworkName(),NetworkData :: GetNetworkPass());
    while (WiFi.status() != WL_CONNECTED)
    {
        delay(500);
        Serial.print(".");
    }
    Serial.println("\n Successful Connected to the internet.");
}

void loop()
{
    //Connect to netowrk if not connected
    if (!client.connect(host, port))
    {
        Serial.println("Connection to host failed");
        delay(100);
        return;
    }
   // Serial.println("Connected to server successful!");
    client.print("Hello From ESP8266");

    Serial.print('\n');
   
    client.stop();
    delay(10);
}
