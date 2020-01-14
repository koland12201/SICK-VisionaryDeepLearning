# SICK-VisionaryDeepLearning
Requires TCP/IP connection with MATLAB or other platform of your choice. Tested on Visionary model S

Interface to MATLAB port: 12201
default Visionary IP: 192.168.1.10
default Visionary port: 2114

upon receiving "REQUEST", the current frame will be send over as 1310720x1 *512 x 640 x 4* uint8 array

to re-arrange array back to 512x640x4, reshape 1310720x1 in RGBA order (in this case, A is replaced by depth map or can be ignored)

# Display bounding boxes
It expects a reply from backend (x,y,w,h) for each boxes in ASCII

Example: "100 200 50 50 300 250 80 60" 

Not tested on other backends, but it should work as long as communication format agrees.
