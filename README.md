# PRN212_BL3_ASM - Flashcard Application

Welcome to the **PRN212_BL3_ASM** project! This is a Flashcard management and quiz application developed using C# and WPF (Windows Presentation Foundation) following the **MVVM (Model-View-ViewModel)** architectural pattern. The app supports two main user roles: **Admin** (managing flashcard decks) and **Student** (studying and taking tests).

## 🚀 Key Features

### For Admin
- **Deck Management**: Create new, view, and delete study decks.
- **Flashcard Management**: Add, edit, and delete terms and definitions within each deck.
- **Excel Import**: Quickly import flashcard data from Excel files (`.xlsx`).
- **Bookmarks**: Bookmark important cards for later review.

### For Student
- **Quiz Mode (Learn)**: Flip cards to view content, self-evaluate as "Learned" or "Not learned," and view learning results upon completion.
- **Matching Mode**: An engaging card-matching game to test memory and reaction speed.
- **Shuffle**: Randomize the order of cards to improve review efficiency.

## 🏗 Project Architecture
The project follows an N-tier architecture to ensure easy maintenance and scalability:
- **ASM.Entities**: Contains domain models such as `Deck`, `Flashcard`, `User`, and `QuizResult`.
- **ASM.Data (DAL)**: Data access layer responsible for reading/writing data to local JSON files (`data.json`, `users.json`).
- **ASM.Bussiness (BLL)**: Business logic layer acting as an intermediary between the UI and Data layers.
- **ASM_PRN212_BL3 (WPF UI)**: The presentation layer, utilizing the MVVM pattern with ViewModels and RelayCommands.

## 🛠 Technologies Used
- **Language**: C# 13.0 (.NET 9.0)
- **UI Framework**: WPF, MaterialDesignThemes
- **Data Handling**: System.Text.Json (JSON storage), ExcelDataReader (Excel Import)

## 📋 Getting Started

### Prerequisites
- Visual Studio 2022 (or newer) with the **.NET desktop development** workload.
- .NET 9.0 SDK.

### How to Run
1. Open the solution file `ASM_PRN212_BL3.slnx` (or `.sln`) using Visual Studio.
2. Build the solution to restore NuGet packages (like MaterialDesignThemes and ExcelDataReader).
3. Set `ASM_PRN212_BL3` as the Startup Project.
4. Press `F5` or click **Start** to run the application.

### Usage Instructions
- **Login**: Upon launching, choose between:
  - **Admin**: To access deck and flashcard management features.
  - **Student**: To start studying and taking quizzes.
- **Data Persistence**: Data is automatically stored locally in `data.json` and `users.json` within the output directory after building and running the app.
