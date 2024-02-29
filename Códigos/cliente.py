import socket

# Set the IP address and port number of the Arduino Mega server
arduino_ip = "192.168.2.20"  # Change this to the Arduino's IP address
arduino_port = 8888          # Change this to the Arduino's port number

# Create a socket object
client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# Connect to the Arduino Mega server
client_socket.connect((arduino_ip, arduino_port))
print(f"Connected to Arduino Mega on {arduino_ip}:{arduino_port}")

# Receive and print the welcome message from the server

while(True):
    message = client_socket.recv(1024).decode('utf-8')
    print(f"Server says: {message}")
    for i in range(7, -1, -1):
        # Use bitwise AND to check the value of the current bit
        bit = (byte_value & (1 << i)) >> i

        # Print the bit
        print(bit, end=" ")
