# TeleMeWhere

## Getting Started:

### Software Requirements:
* Unity 2017.1.1f1
* Windows 10 machine
* Visual Studio 2017
* Microsoft HoloLens in Developer Mode
* Oracle VirtualBox
* Vagrant

### Server Setup:
1. In Putty, cd to API folder.
2. Start virtual machine using command"vagrant up"
3. Use "vagrant ssh" to access the machine.
4. cd to "/vagrant" and run "source venv/bin/activate"
5. Setup the database with " mysql -u root -p < sql/tbl_create.sql", password is "root"
7. Quit from mysql and run "python app.py" to boot the server.
8. Find out the local IP address of the computer and input into hololens.

### App Deployment:
1. Open project in Unity
2. In the Project window, expland the Scenes folder and then double-click MainMenuScene
3. In the Hierarchy window, select Managers.
4. In the Inspector window, enter the IP address you saved from earlier into the field under Network Utility
5. Go back to the Project and double-click on ModelScene under Scenes
6. Repeat steps 3-4 for ModelScene
7. In the upper tabs, go to Mixed Reality Toolkit > Build Window
8. Connect your Hololens vis USB
9. Set "Build Configuration" to "Release" and "Build Platform" to "x86"
10. Make sure the Unity Csharp Projects is checked
11. Enter username and password
12. Click "Build SLN, Build APPX, then Install
13. When that is finished click Open Project Solution to open Visual Studio
14. In Visual Studio, at the top set "Solution Configuration" to "Release", "Solution Platform" to "x86", and change "Local Machine" to "Remote Machine" and then enter the HoloLens IP address
15. At the top go to Debug > Start Without Debugging and enjoy!

## Usage:

* Users can either create an account or sign in. The tapping gesture can be used to click buttons on the screen. Tapping input fields bring up the keyboard which in turn can tapped to enter letters into the fields. 
* NOTE: The backspace button is currently buggy and will be fixed in a later release. For now, go back to the main menu and then return to your previous menu to clear the input fields.
* After signing in or creating an account, the user will be brought to the model scene.
* Currently, the model is a rectangular prism used to test the functionality of marker placing and manipulation. In the next release, this will be changed to a human body.
* There are several options for interacting with the model as well as a sign out button. By default, it is set to drag. All of these options are employ the use of the pinching/tapping gesture. Pinching and holding is for rotating and dragging while tapping is used for the marker interactions.
* The user must have the cursor on a marker in order for the removal to work.
* Marker placements and removals are saved in the database so when the user signs out and then signs back in, the markers will still be in the same place.

