import socket

# Set the IP address and port number for the server
server_ip = "192.168.2.109"  # Change this to the server's IP address or use "localhost" for local testing
server_port = 8083       # Choose an available port number

# Create a socket object
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# Bind the socket to a specific IP address and port
server_socket.bind((server_ip, server_port))

# Listen for incoming connections
server_socket.listen(1)
print(f"Server listening on {server_ip}:{server_port}")

while True:
    # Wait for a connection from a client
    client_socket, client_address = server_socket.accept()
    print(f"Connection from {client_address}")

    # Receive and echo messages from the client
    while True:
        data = client_socket.recv(1024)
        if not data:
            break
        message = data.decode('utf-8')
        print(f"Received from {client_address}: {message}")

    # Close the connection with the client
    print(f"Connection with {client_address} closed")
    client_socket.close()
    server_socket.listen(1)