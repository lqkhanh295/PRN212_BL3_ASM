# PRN212_BL3_ASM - Flashcard Application

Welcome to the **PRN212_BL3_ASM** project! This is a Windows Presentation Foundation (Wpf) application built with .NET 9.0 and C#, designed as a basic Flashcard and Quiz application to help users learn and memorize vocabulary or technical terms efficiently.

## Features

- **Flashcard Management**: View, add, edit, and delete vocabulary sets (decks) and flashcards.
- **Bookmarks**: Bookmark specific flashcards for review.
- **JSON Data Storage**: All flashcards and users are persisted locally using JSON files (`data.json`, `users.json`), ensuring your data is saved across sessions.
- **Modern UI**: The user interface is built using `MaterialDesignThemes` and `MaterialDesignColors` for a sleek, modern, and user-friendly experience.
- **N-Tier Architecture**: The application is structured into clearly separated layers for better maintainability and scalability:
  - **ASM.Entities**: Contains domain models and entities.
  - **ASM.Data**: Handles data access and JSON serialization/deserialization.
  - **ASM.Bussiness**: Contains business logic and services.
  - **ASM_PRN212_BL3**: The main presentation layer (WPF Application).

## Technologies Used

- C# 13.0
- .NET 9.0 (Windows)
- WPF (Windows Presentation Foundation)
- MaterialDesignThemes (v1.0.1) / MaterialDesignColors (v5.3.0)
- JSON for local data persistence

## Getting Started

### Prerequisites
- Visual Studio 2022 (or newer) with the **.NET desktop development** workload installed.
- .NET 9.0 SDK.

### How to Run
1. Clone or download this repository.
2. Open the solution file `ASM_PRN212_BL3.slnx` using Visual Studio.
3. Build the solution to restore NuGet packages (like MaterialDesignThemes).
4. Set `ASM_PRN212_BL3` as the Startup Project.
5. Press `F5` or click **Start** to run the application.

## Project Structure

- `ASM_PRN212_BL3/` - The main WPF project containing views and UI logic.
- `ASM.Bussiness/` - Business logic layer processing user actions.
- `ASM.Data/` - Data access layer, interacts with `data.json` and `users.json`.
- `ASM.Entities/` - Shared class library defining objects like `Flashcard`, `User`, etc.

## Data Persistence
The app uses local JSON files for storage. Upon building and running, `data.json` and `users.json` are automatically copied to the output directory to maintain state. 
