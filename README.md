# Navun

# Project Description

This repository contains two parts of the project: **mobile application** and **server**. The mobile application allows users to create groups for tracking coordinates via GPS and hotspot. Viewing the location of group members on an interactive map. The server part handles requests over the MQTT protocol, saves the history of movements during a session, and provides this information for use by rescue groups in case of loss of connection.

## Mobile Application

The mobile application allows you to:

- Create groups for tracking the location of several users.
- Track the coordinates of group members using GPS.
- View the location of group members on an interactive map.

## Server Part

The server part performs the following functions:

- Receiving and processing requests via the MQTT protocol from mobile applications.
- Saving the history of user movements during a session.
- Providing access to the movement history for use by rescue groups in case of loss of connection with mobile applications.

## Installation and Launch

- **Mobile Application:** Instructions for installing and launching the mobile application can be found in the mobile-app branch README file.
- **Server:** Instructions for installing and launching the server part can be found in the mqtt-server branch README file.

## Environment Requirements

- The mobile application requires a compatible device with GPS support and the ability to install mobile applications.
- The server part requires an environment capable of installing and running server software that supports the MQTT protocol.

## Contributions and Bug Reports

If you have found a bug or would like to suggest an improvement, please create a new issue in this repository. We welcome your contributions and are ready to consider any proposals for improving the project.

## License

This project is licensed under the [MIT License](LICENSE).
