# CineExplorer

CineExplorer is a web application that allows users to explore movies, their filming locations, and plan trips based on these locations. Users can create accounts, view movie and location details, plan trips, and write reviews for locations they've visited.

## Features

- Browse movies and their filming locations
- View detailed information about movies and locations
- User authentication and authorization
- Create and manage trips based on movie locations
- Write and view reviews for locations
- Admin functionality for managing movies and locations

## Technologies Used

- ASP.NET MVC 5
- C#
- Entity Framework
- HTML/CSS
- JavaScript
- Bootstrap
- Web API for backend services

## Controllers

The application includes the following main controllers:

1. `HomeController`: Manages the home page and general navigation
2. `MovieController`: Handles movie-related operations (CRUD)
3. `LocationController`: Manages location-related operations (CRUD)
4. `TripController`: Handles trip creation and management
5. `ReviewController`: Manages user reviews for locations

## Setup

1. Clone the repository
2. Open the solution in Visual Studio
3. Restore NuGet packages
4. Update the connection string in `Web.config` to point to your database
5. Run database migrations to create the necessary tables
6. Build and run the application

## Usage

- Register an account or log in
- Browse movies and locations
- Create trips based on movie locations
- Write reviews for locations you've visited
- Admins can manage movies and locations through the admin interface
