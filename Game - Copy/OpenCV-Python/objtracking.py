import sys
import cv2 as cv
import numpy as np
import imutils
import socket


UDP_IP = "127.0.0.1" #Declaring loopback address which will be used to send data over to the game
UDP_PORT = 5065    #Declaring a port to go with the IPv4 address

 #declaring a socket 
 # argument 1 : states its IPv4 address; argument 2: states its a UDP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

#Declaring a VideoCapture object; argument 0 means it is your device's camera
cap = cv.VideoCapture(0)
while(1):

    # Take each frame
    _, frame = cap.read()
    #Resizing frame
    frame = imutils.resize(frame, width=640)
    
    cv.imshow('bgr',frame)

    # Convert BGR to HSV
    hsv = cv.cvtColor(frame, cv.COLOR_BGR2HSV)

    #cv.imshow('hsv',hsv)

    # define range of blue color in HSV
    lower_blue = np.array([100 ,100,100])
    upper_blue = np.array([120,255,255])
    # Threshold the HSV image to get only blue colors
    mask = cv.inRange(hsv, lower_blue, upper_blue)
    
    #cv.imshow('mask',mask)

    
    #Erode and dilate to remove any white noise
    mask = cv.morphologyEx(mask, cv.MORPH_OPEN, None)
   

    #cv.imshow('mask after dilation',mask)

    center = None
    #finding contours in the mask
    image, cnts, hierarchy  = cv.findContours(mask.copy(), cv.RETR_TREE, cv.CHAIN_APPROX_SIMPLE)

    #draw all contours; -1 signifies draw out all contours
    cv.drawContours (frame, cnts, -1, (0,0,255), 2)
    #cv.imshow ( 'all of the contours', frame)
    #print("Number of contours: ", len(cnts))

    #if one contour is found
    if len(cnts) > 0:

        # find the largest contour from the array of contours
        largest_contour = max(cnts, key=cv.contourArea)
        #draw all contours; -1 signifies draw out all contours
        cv.drawContours (frame, largest_contour, -1, (0,255,0), 2)
        #cv.imshow ( 'largest contour', frame)
        #compute an enclosing circle that is around the largest contour
        ((x, y), radius) = cv.minEnclosingCircle(largest_contour)
        #finding the image moment of the contour
        M = cv.moments(largest_contour)
        #computing the center from the image moment found above
        if (M["m00"]!=0):
            center = (int(M["m10"] / M["m00"]), int(M["m01"] / M["m00"]))
        #filtering out the contour based on radius; radius found from the enclosing circle computed above
            if (radius > 35) & (radius <65):
                cv.circle(frame, (int(x), int(y)), int(radius),(0, 255, 255), 2)
            
                #print ("X-coordinate of center: ", center[0])
            #Changing the x-coordinates from the openCV system to the Unity system
            #TargetCoordinate = TranslateFactor + ScalingFactor*SourceCoordinate

            #T1 = TranslateFactor + ScalingFactor*S1
            #T2 = TranslateFactor + ScalingFactor*S2
            #given two points in TargetCoordinate (T1, T2) that corresponds to two points in SourceCoordinate(S1, S2)
            #TranslateFactor = (T2*S1 - T1*S2) / (S1 - S2)
            #ScalingFactor   = (T2 - T1) / (S2 - S1)
            
                realworldX = 11.5 -0.03*(center[0])
                #print ("X-coordinate sent to unity: ", realworldX )

           #if radius between above ranges, send the x-coordinate of the centroid to the game through the socket
           #arguments: the x-coordinate coverted to a UTF-8 encoded string in bytes, the address & port to forward it to
                sock.sendto(bytes(str(realworldX).encode('UTF-8')) , (UDP_IP, UDP_PORT))


    
    cv.imshow('frame',frame)
    
    k = cv.waitKey(5) & 0xFF
    if k == 27:
        break
cv.destroyAllWindows()