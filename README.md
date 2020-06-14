# Anoroc-Server

## Table of Contents

- [Project Overview](#projectoverview)
- [Documentation](#documentation)
- [Technologies](#technologies)
- [Installation](#installation)
- [Testing Instructions](#testinginstructions)
- [Team](#team)
- [Contributions](#Contributions)

## Project Overview
Considering the recent Global Emergency, it has been more apparent than ever before that the ability to track and trace a contingent is of the utmost importance. Cotangent tracking allows users to avoid potential infections and for authorities to be able to flatten the infection rate curve by the use of restrictions or medical services at the hot spots of the tracing.
The application will allow the tracking of multiple contingents that are deemed as pandemics by the World Health Organisation, that are currently affecting the country that the user is in. Often this will only be one contingent at a time; however, the system will have the capability to track multiple.

The application proposed is a tool that tracks the user’s location data on a mobile app, that is sent to the server and stored in a manner that both protects the user’s identity and location, as well as provides a way to generate spatial data that can be analyzed and displayed in the manner of a heat map on the user’s mobile device or the web app. User’s should be able to view and download their location data as well as sensitive data they have opted into entering.

The application will allow the user to log in or register using either an Anoroc account or by the use of their social media accounts (Google, Facebook). Once logged in, the user will be able to voluntarily give their contagent status and select the contagent they will be focusing on. The application allows for anonymous users, these user will simply be able to see the hotspots and calcuate their risk.

## Documentation

- SRS Document: https://bit.ly/2XZDwEA
- Demo 1 Video: https://bit.ly/2UDQRAk
- Team email: code.sum.moar@gmail.com
- Team Communications: [Microsoft Teams](https://www.microsoft.com/en-za/microsoft-365/microsoft-teams/group-chat-software)
- Organisation: [Clubhouse](https://app.clubhouse.io/codesummoar)

## Technologies

- Mobile Application: [Xamarin Forms | .NET](https://dotnet.microsoft.com/apps/xamarin/xamarin-forms)
- API: [ASP.NET Core API](https://dotnet.microsoft.com/apps/aspnet/apis)
- Server: Azure Services

## Installation

### Required Software

- [Visual Studios](https://visualstudio.microsoft.com/) With these options installed:
![VisualStudioInstallOptions](https://user-images.githubusercontent.com/61750301/84587406-413da680-ae1f-11ea-88f3-bad89050ea1a.png)
- Clone the Anoroc-Mobile or Anoroc-Server repository into your local directory.
- Open Visual Studios by launching the AnorocMobileApp.sln or Anoroc.sln file respectively.


## Testing Instructions

## Team
| Name   | Surname    |        Email         |       Github.io        |
|--------|------------|----------------------|------------------------|
| Tebogo | Selahle     | u15210822@tuks.co.za | [tebogo.codes](https://tebogo.codes/)  |
| Andrew | Wilson     | u15191223@tuks.co.za | [andrew96-eng.github.io](https://andrew96-eng.github.io) |
| Anika  | van Rensburg | u17246840@tuks.co.za | [anikavanrensburg.github.io](https://anikavanrensburg.github.io) |
| Anrich | Hildebrand | u15178782@tuks.co.za | [anrich96.github.io](https://anrich96.github.io) |
| Kevin  | Huang | u15026681@tuks.co.za | [kevin-d-h.github.io](https://kevin-d-h.github.io/myCV/) |
| Ronald | Looi | u15226532@tuks.co.za | [rsl-student.github.io](https://rsl-student.github.io) |


## Contributions

### Tebogo Selahle

- Worked on API Server
- Created unit tests

### Andrew Wilson

- Xamarin Forms Map
- Login with Facebook

### Anika van Rensburg

- Worked on API Server
- Enable GEO Location tracking on Mobile App

### Anrich Hildebrand

- Login with Google to return User's Google profile data
- Created mock database to test GEO Location points on map

### Kevin Huang

- Worked on API Server
- Notification Module of the Mobile App

### Ronald Looi

- Integrate Team modules
- Navigation in the Mobile App
