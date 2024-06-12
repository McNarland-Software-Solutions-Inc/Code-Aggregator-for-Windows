# Code Aggregator

## Description

Code Aggregator is an application that allows you to select a root folder and aggregate all text files within the selected folder and its subfolders into a single text file. This is particularly useful for consolidating code files or other text-based files.

## Features

- Select a root folder for aggregation.
- Include or exclude specific files and folders using checkboxes.
- Save the aggregated text to a specified location.
- Option to open the aggregated file after the operation completes.
- Command line support for running the aggregator without the GUI.

## How to Use

### Graphical User Interface

1. Click the 'Select Folder' button to choose the root folder for aggregation.
2. Use the checkboxes to include or exclude specific files and folders. By default, all folders and files are included.
3. Click 'Start Operation' to begin the aggregation process. You will be prompted to specify the location and name of the output file.
4. Note: It is recommended to exclude folders like .git, .vs, or other dependency folders to avoid including unnecessary files.

### Command Line Interface

- `?` - Show command line syntax and tips.
- `source_folder` - Run the aggregator on this source folder.
- `-o:"New_Output_Folders\New_Output_Filename.ext"` - Change the output file for this run only.
- `-oc:"New_Output_Folders\New_Output_Filename.ext"` - Change the output file and update the JSON settings.
- `-a:"Folder\Folder_or_File_to_Add[.ext]"` - Add this folder or file for this run only.
- `-ac:"Folder\Folder_or_File_to_Add[.ext]"` - Add this folder or file and update the JSON settings.
- `-q` - Quiet mode (no output from the program, don't show windows).

#### Error Codes

- `0` - Worked perfectly.
- `1` - Folder not found (when trying to add a new folder).
- `2` - File not found (when trying to add a new file).
- `3` - Error outputting to the output file.
- `4` - Source folder not found.

## Installation

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Building the Executable

1. Clone the repository:

    ```sh
    git clone https://github.com/yourusername/code-aggregator.git
    cd code-aggregator
    ```

2. Build the project:

    ```sh
    dotnet build --configuration Release
    ```

### Creating the Installer

1. Ensure you have [Inno Setup](https://jrsoftware.org/isinfo.php) installed.

2. Open the `Inno Install Compiler Script.iss` file in Inno Setup.

3. Compile the script. This will look for the built executable in the `bin/Release/net8.0-windows` folder and create a `setup.exe` file in the `Output` folder.

### Running the Installer

1. Navigate to the `Output` folder where the `setup.exe` file was created.

2. Run `setup.exe` to install the Code Aggregator application.

3. After installation, you can find the application in the Start Menu under "Code Aggregator."

## Version 1.0.0 - Initial Release

### Overview

This is the initial release of the Code Aggregator application, designed to help users aggregate text files from selected folders into a single text file. This is particularly useful for consolidating code files or other text-based files from a project directory.

### Features

- **Folder Selection**: 
  - Users can select a root folder to aggregate files from.
  - A tree view is provided to include or exclude specific files and folders.
  - All selected folders and files are remembered for future use.
  
- **File Aggregation**:
  - Aggregates all text files from the selected folders and subfolders into a single text file.
  - Ensures that only text files are included in the aggregation.
  
- **Settings Management**:
  - Saves the selected folders and file inclusion settings in a JSON file.
  - Detects new folders and indicates them to the user for inclusion.
  
- **User Interface**:
  - Instructions are provided on the main form to guide the user.
  - Buttons for selecting folders and starting the aggregation process.
  - Progress form to show the progress of the aggregation process.
  - Option to open the generated file after the aggregation is complete.

- **Command Line Support**:
  - Supports command line arguments to run the aggregator without the graphical user interface.
  - Options to specify output file, add folders, and run in quiet mode.

### Command Line Options

- `?` - Show command line syntax and tips.
- `source_folder` - Run the aggregator on this source folder.
- `-o:"New_Output_Folders\New_Output_Filename.ext"` - Change the output file for this run only.
- `-oc:"New_Output_Folders\New_Output_Filename.ext"` - Change the output file and update the JSON settings.
- `-a:"Folder\Folder_or_File_to_Add[.ext]"` - Add this folder or file for this run only.
- `-ac:"Folder\Folder_or_File_to_Add[.ext]"` - Add this folder or file and update the JSON settings.
- `-q` - Quiet mode (no output from the program, don't show windows).

### Error Codes

- `0` - Worked perfectly.
- `1` - Folder not found (when trying to add a new folder).
- `2` - File not found (when trying to add a new file).
- `3` - Error outputting to the output file.
- `4` - Source folder not found.

### Usage

#### Graphical User Interface

1. Open the application.
2. Click the 'Select Folder' button to choose the root folder for aggregation.
3. Use the checkboxes in the tree view to include or exclude specific files and folders.
4. Click 'Start Operation' to begin the aggregation process.
5. After completion, choose whether to open the generated file.

#### Command Line Interface

1. Run the application from the command line with the appropriate arguments as described above.

### Known Issues

- Initial scanning of large directories may take some time.
- Antivirus software may flag the executable. Consider signing the code with a recognized certificate.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any changes.

## License

This project is licensed under the MIT License.
