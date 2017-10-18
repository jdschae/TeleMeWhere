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
2. In the Assets tree, double 
