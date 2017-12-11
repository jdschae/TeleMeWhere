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
NOTE: If the Unity Editor asks you to upgrade MCS from 1.0 to 1.6, click "Cancel, do NOT install."
1. Open project in Unity
2. In the Project window, expland the Scenes folder and then double-click MainMenuScene
3. In the Hierarchy window, select Managers.
4. In the Inspector window, enter the IP address you saved from earlier into the field under Network Utility
5. Go back to the Project and double-click on ModelScene under Scenes
6. Repeat steps 3-5 for ModelScene and Homepage
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

* In the first menu, users can either create an account or sign in. The tapping gesture can be used to click buttons on the screen. Tapping input fields brings up the keyboard which in turn can be tapped to enter letters into the fields. 
* NOTE: The backspace button is currently buggy and will be fixed in a later release. For now, hit the go back button (or equivalent) to return to your previous menu to clear the input fields.
* In the account creation page, there is the option to choose sex and whether you are a patient or a doctor. Sex will affect the type of model for that user.
* After signing in or creating an account, the user will be brought to the homepage.
* In the homepage, there are 5 options:
  * The user can edit their account information. Blank input fields will not be changed when the submit button is clicked, but valid input will change the information for the corresponding fields in the database.
  * The user can send invites. Simply type in the username of the desired invitee and hit the button for sending it.
  * The user can check and accept invites. In this page, there will be a list of users that have invited you to view their model. Select the corresponding checkbox and hit "Accept Invite" to go to that session. There is also a "Refresh" button to load any new invites.
  * The user can sign out and return to the Create Account/Sign In page.
  * The user can go to view their own model
* When viewing your own model or another user's the options are the same.
* In a model session, there are several options according to the pressed button on the action menu:
  * Dragging the model: this is the default interaction. Using the pinching and dragging gesture, the model can be moved with the hand.
  * Rotating the model: Using the pinching and dragging gesture the model can be rotated according to how far the hand is from straight in front of you. Currently, the pivot is at the feet of the model which will change for the Omega Release.
  * Removing a marker: Using the pinching gesture when the cursor is on a marker will delete the marker from the model and the database. This removal will be seen by all users viewing the model.
  * Placing marker or viewing notes: Pressing this button will allow two options. If the user selects a point on the model using their gaze and pinching gesture, a menu will pop up that allows the user to select the shape of the marker and the notes to add to it. Pressing "Place Marker" will add the marker to the spot previously selected on the model with the corresponding shape. These markers are added to the database and seen by every user viewing the session. Selecting a marker will cause a different window to open up that displays the notes were added to that marker.
* The user also has the option to return the homepage with a "Homepage" button.

For more info on usage, view Tutorial.pdf.
