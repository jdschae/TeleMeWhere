CREATE DATABASE telemewhere;
USE telemewhere;

CREATE TABLE User(username VARCHAR(20), firstname VARCHAR(20), lastname VARCHAR(20), password VARCHAR(256), email VARCHAR(256), creationdate TIMESTAMP DEFAULT CURRENT_TIMESTAMP, PRIMARY KEY(username));
CREATE TABLE Model(modelid INT NOT NULL AUTO_INCREMENT, username VARCHAR(20), creationdate TIMESTAMP DEFAULT CURRENT_TIMESTAMP, PRIMARY KEY(modelid));
CREATE TABLE Marker(markerid INT NOT NULL AUTO_INCREMENT, modelid INT NOT NULL, x FLOAT(10,5), y FLOAT(10,5), z FLOAT(10,5), creationdate TIMESTAMP DEFAULT CURRENT_TIMESTAMP, message VARCHAR(500), PRIMARY KEY(markerid));

INSERT INTO User (username, firstname, lastname, password, email) VALUES ("jahan", "Frank", "Yu", "eecs498", "hello@gmail.com");
INSERT INTO User (username, firstname, lastname, password, email) VALUES ("daddy", "John", "Schaefer", "eecs498","123@gmail.com");
INSERT INTO User (username, firstname, lastname, password) VALUES ("mitch", "Mitchell", "Trost", "eecs498", "222@gmail.com");

