# AdminKeyAPI

AdminKeyAPI is an ASP.NET Core Web API project that provides various endpoints for managing data. This project uses Dapper for data access and JWT for authentication.

## Features

- API versioning
- JWT authentication
- CORS support for development and production environments
- Swagger documentation
- Output caching

## Getting Started

### Prerequisites

- .NET 6.0 SDK or later
- SQL Server
- Visual Studio or JetBrains Rider

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/yourusername/FirstAPI.git
    cd FirstAPI
    ```

2. Set up the environment variables:
    - Create a `.env` file in the root directory.
    - Add the following environment variables:
        ```env
        CONNECTION_STRING=your_connection_string
        TOKEN_KEY=your_token_key
        PASSWORD_KEY=your_password_key
        ```
- Replace `your_connection_string`, `your_token_key`, and `your_password_key` with actual values specific to your environment.


### Running the Application

To start the API server locally, follow these steps:

1. **Build and Run the Application**:
    ```bash
    dotnet run
    ```

2. The API will be accessible at:
    - **HTTPS**: `https://localhost:7218`
    - **HTTP**: `http://localhost:5137`


### API Documentation

- **Swagger UI** is available at `https://localhost:7218/swagger` (or `http://localhost:5137/swagger` depending on your configuration).
    - Swagger UI provides an interactive interface to explore and test all API endpoints.
      
## API Usage

### Endpoints Overview

#### **UserJobInfo Endpoints**
The following endpoints manage user job information:

- **GET** `/api/v{version}/UserJobInfo/GetUserJobInfo`
    - **Description**: Retrieves job information for users.
    - **Response**: A list of job information objects.
    - **Example**: Returns job titles, companies, and related details for all users.

- **POST** `/api/v{version}/UserJobInfo/AddUserJobInfo`
    - **Description**: Adds new job information for a user.
    - **Request Body**: A JSON object containing job details (e.g., job title, company, start date).
    - **Response**: A confirmation of the job information added.

- **PUT** `/api/v{version}/UserJobInfo/UpdateUserJobInfo`
    - **Description**: Updates the existing job information for a user.
    - **Request Body**: A JSON object containing updated job details.
    - **Response**: A confirmation of the updated job information.

- **DELETE** `/api/v{version}/UserJobInfo/DeleteUserJobInfo/{UserId}`
    - **Description**: Deletes job information for a specific user.
    - **Parameters**: `UserId` (path parameter) - The unique identifier of the user.
    - **Response**: A confirmation of the deletion.

#### **Authentication Endpoints**
These endpoints manage user authentication:

- **POST** `/api/v1/Auth/Register`
    - **Description**: Registers a new user in the system.
    - **Request Body**: A JSON object containing user registration details (e.g., username, password, email).
    - **Response**: A success message or error if registration fails.

- **POST** `/api/v1/Auth/Login`
    - **Description**: Authenticates a user and returns a JWT token for further requests.
    - **Request Body**: A JSON object containing the user’s login credentials (username and password).
    - **Response**: A JWT token that should be used for authorization in subsequent requests.

- **PUT** `/api/v1/Auth/ResetPassword`
    - **Description**: Resets the user's password.
    - **Request Body**: A JSON object containing the new password and user identification details.
    - **Response**: A success message confirming the password reset.

- **GET** `/api/v1/Auth/RefreshToken`
    - **Description**: Refreshes the user's JWT token using a valid refresh token.
    - **Response**: A new JWT token for continued authentication.

#### **User Endpoints**
These endpoints manage user data:

- **GET** `/api/v{version}/User/GetUsers`
    - **Description**: Retrieves a list of all users.
    - **Response**: A list of user objects containing user information.

- **PUT** `/api/v{version}/User/UpsertUser`
    - **Description**: Updates an existing user or inserts a new one if the user doesn't already exist.
    - **Request Body**: A JSON object containing user details.
    - **Response**: A confirmation of the user being updated or added.

- **DELETE** `/api/v{version}/User/DeleteUser/{userId}`
    - **Description**: Deletes a user by their unique ID.
    - **Parameters**: `userId` (path parameter) - The unique identifier of the user to delete.
    - **Response**: A confirmation of the deletion.

#### **UserSalary Endpoints**
These endpoints manage user salary information:

- **POST** `/api/v{version}/UserSalary/AddUserSalaryInfo`
    - **Description**: Adds a new salary record for a user.
    - **Request Body**: A JSON object containing salary information (e.g., amount, date).
    - **Response**: A confirmation of the salary record added.

- **GET** `/api/v{version}/UserSalary/GetUserSalaryInfo`
    - **Description**: Retrieves salary information for users.
    - **Response**: A list of salary records, which may be filtered by `userId`.

- **PUT** `/api/v{version}/UserSalary/UpdateUserSalaryInfo`
    - **Description**: Updates an existing salary record for a user.
    - **Request Body**: A JSON object containing updated salary details.
    - **Response**: A confirmation of the updated salary information.

- **DELETE** `/api/v{version}/UserSalary/DeleteUserSalaryInfo/{UserId}`
    - **Description**: Deletes salary information for a user.
    - **Parameters**: `UserId` (path parameter) - The unique identifier of the user.
    - **Response**: A confirmation of the salary record deletion.

### Contributing

Contributions are welcome! To contribute, simply open a pull request with your changes.

