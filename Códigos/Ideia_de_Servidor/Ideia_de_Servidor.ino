#include <SPI.h>
#include <Ethernet.h>

// Define the MAC address and IP address for your Arduino Mega
byte mac[] = {0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED};
IPAddress ip(192, 168, 1, 177); // Change this to an appropriate IP address for your network

// Define the port number for your server
unsigned int serverPort = 8888;

// Create an Ethernet server
EthernetServer server(serverPort);

void setup() {
  // Start the Ethernet connection and server
  Ethernet.begin(mac, ip);
  server.begin();
  Serial.begin(9600);
  Serial.println("Server started");
}

void loop() {
  // Check for incoming connections
  EthernetClient client = server.available();
  if (client) {
    Serial.println("New client connected");

    // Send a welcome message to the client
    client.println("Hello, Client!");

    // Close the connection
    client.stop();
    Serial.println("Client disconnected");
  }
}