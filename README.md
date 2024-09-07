# Workspace Booking

## Description

In modern organizations, managing workspaces is becoming an increasingly relevant task, especially in the context of hybrid and remote work formats. Effective workspace booking allows for optimizing the use of office space, increasing employee productivity, and reducing rental costs. Existing booking systems have a common drawback — high implementation costs. Many organizations still use manual accounting methods, such as maintaining logs or Excel spreadsheets, which leads to errors and inefficiencies.

The goal of this project is to design and develop software intended for tracking workspace bookings, ensuring real-time availability and usage management. This is particularly relevant for organizations using coworking spaces or flexible office environments, where the number of available spots may vary.

### Key Features:
- Booking of workspaces and parking spaces.
- Management of booking statuses.
- Support for various user roles (administrators, employees).
- User-friendly interface.

## Technical Details

- **Programming Language**: C#
- **Development Environment**: Microsoft Visual Studio 2022
- **Application Type**: Desktop (WPF)
- **Programming Paradigm**: Object-Oriented Programming (OOP)
- **Data Organization Method**: MSSQL Database
- **Data Access Technology**: Entity Framework
- **Architectural Pattern**: MVVM

## Installation

To install and set up the project, follow these steps:

1. Clone the repository:
   ```bash
   git clone https://github.com/AleksandrFyaksel/BookingSystem.git
   ```
2. Navigate to the project directory:
   ```bash
   cd BookingSystem
   ```
3. Install the necessary dependencies:
   ```bash
   npm install
   ```
   (or use another package manager if necessary).

4. Set up the database by following the instructions in the `DATABASE_SETUP.md` file.

## Usage

To run the project, execute:
bash
npm start
After that, open your browser and go to `http://localhost:3000` (or another address specified in the settings).
## Examples
Example code for booking a workspace:
javascript
const booking = new Booking();
booking.reserveWorkspace(userId, workspaceId, startTime, endTime);
Screenshot of the interface:
![Screenshot](link_to_screenshot.png)
## Contribution
If you would like to contribute to the project, please create a pull request or report issues by opening an issue in the repository.
## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.
## Contacts
If you have any questions or suggestions, you can contact me via email: famix75@mail.ru

Summary of Changes
•  The text has been translated into English.
•  Minor grammatical and formatting adjustments were made for clarity.
If you have any further questions or need additional assistance, feel free to ask!
