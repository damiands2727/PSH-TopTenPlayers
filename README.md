# Player Rankings Application

## Overview

### Here's a quick video demo: https://drive.google.com/file/d/1gSWFkv4IVz98mUl5dXSIonDRnN0L3tXa/view?usp=drive_link

This project provides a full-stack application that displays the top 10 player rankings and includes various functionalities like adding a new player, batch insertion of multiple players, clearing the database, and exporting the top 10 players as a CSV file. The backend is built using **ASP.NET Core** with a **SQL Server database**, while the frontend is developed using **React.js**.

## Features

1. **Display Top 10 Players**\
   Fetches and displays the top 10 players sorted by their score in descending order.

2. **Add Winner**\
   Allows the addition of a new player to the database with details like nickname, stat type, profile image, and score.

3. **Batch Insert Players**\
   Inserts multiple players into the database in a single transaction using a custom **table-valued parameter (TVP)**.

4. **Clear Database**\
   Deletes all player records from the database.

5. **Export CSV**\
   Exports the current top 10 players as a downloadable CSV file.

## Backend Setup

### Endpoints

1. **GET /api/players/top10**\
   Fetches the top 10 players sorted by score.

2. **POST /api/players**\
   Adds a new player to the database.

3. **POST /api/players/batch**\
   Batch inserts multiple players into the database using the `Players_InsertMany` stored procedure.

4. **DELETE /api/players**\
   Deletes all player records from the database using the `Players_Delete` stored procedure.

5. **GET /api/players/downloadCsv**\
   Generates and returns a CSV file containing the top 10 players.

---

### SQL Procedures

Here are the SQL stored procedures used in this project:

#### **[dbo].[Players\_InsertMany]**

Batch inserts multiple players using a table-valued parameter (TVP).

```sql
ALTER PROCEDURE [dbo].[Players_InsertMany]
    @Players [dbo].[PlayerTableType] READONLY
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[Players] ([StatId], [NickName], [ProfileImage], [Score])
    SELECT [StatId], [NickName], [ProfileImage], [Score]
    FROM @Players;

    SELECT *
    FROM [dbo].[Players]
    WHERE Id IN (SELECT Id FROM [dbo].[Players]);
END
```

- **Note:** This procedure uses a custom `PlayerTableType` table type:

```sql
CREATE TYPE [dbo].[PlayerTableType] AS TABLE
(
    StatId INT,
    NickName NVARCHAR(100),
    ProfileImage NVARCHAR(500),
    Score INT
);
```

---

## Frontend Setup

### Key Functionalities

1. **PlayerList Component**

   - Displays the top 10 players using `PlayerCard` components.
   - Includes buttons for developer cheats, exporting CSV, adding a winner, and clearing the database.

2. **Service Methods**
   The `playersService` file handles API calls:

   ```javascript
   import axios from "axios";

   const playersService = {
     endpoint: `https://localhost:50001/api/players`,
   };

   playersService.getTopTen = () => {
     return axios.get(`${playersService.endpoint}/top10`);
   };

   playersService.add = (payload) => {
     return axios.post(playersService.endpoint, payload);
   };

   playersService.addBatch = (players) => {
     return axios.post(`${playersService.endpoint}/batch`, players);
   };

   playersService.deleteAll = () => {
     return axios.delete(playersService.endpoint);
   };

   playersService.downloadCsv = () => {
     return axios.get(`${playersService.endpoint}/downloadCsv`, {
       responseType: "blob",
     });
   };

   export default playersService;
   ```

3. **CSV Export**
   Triggered by a button in the `PlayerList` component, which calls `playersService.downloadCsv` to download the file containing the top 10 players.

---

## How to Run the Project

1. **Clone the Repository**

   ```bash
   git clone <repository-url>
   cd <repository-folder>
   ```

2. **Set Up Backend**

   - Open the solution in Visual Studio.
   - Update the connection string in `appsettings.json` to point to your SQL Server instance.
   - Run the database migrations or execute the SQL scripts manually.

3. **Set Up Frontend**

   - Navigate to the `ClientApp` folder.
   - Install dependencies:
     ```bash
     yarn install
     ```
   - Start the React development server:
     ```bash
     yarn start
     ```

4. **Run the Application**

   - Start the backend server from Visual Studio.
   - Open `http://localhost:3000` to access the React frontend.

---

## Screenshots

Add screenshots of the UI here for better visualization of the application.

---

## Future Enhancements

- Add user authentication for accessing the application.
- Implement a leaderboard for different stat types.
- Add pagination for player rankings.

---

## Credits

This project was created by **Damian Guido Stella** as part of a full-stack development exercise.
