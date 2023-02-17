import socket
import time

UDP_IP=socket.gethostbyname( socket.gethostname())
UDP_PORT =8585

sock = socket.socket(socket.AF_INET, # Internet
                     socket.SOCK_DGRAM) # UDP
print("binding to " + str(UDP_IP) + ":" +str(UDP_PORT) )
sock.bind((UDP_IP, UDP_PORT))

Data = []
def Loop():
    global Data
    while True:
        data, addr = sock.recvfrom(1024) # buffer size is 1024 bytes
        input = data.decode().split()


        exist = False
        for x in Data:
            if(x[0] == input[0]):
                exist = True  
                x[1] = input[1]
                x[2] = input[2]
                x[3] = input[3]
                x[4] = input[4]
                x[5] = input[5]
                x[6] = input[6]
                
        if(exist == False):
            Data.append([input[0], 0,0,0, 0,0,0])
            
             