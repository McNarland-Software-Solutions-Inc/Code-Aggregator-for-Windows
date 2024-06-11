# Code Aggregator

## Description

Code Aggregator is a Windows Forms application that allows users to select folders and files within a directory structure and aggregate the contents of selected text files into a single output file. This application is useful for creating a consolidated file of all code or text within a solution folder.

## Features

- Select a root folder using a folder dialog.
- Display the folder structure in a tree view with checkboxes to include/exclude files and folders.
- Save and load inclusion/exclusion settings for each folder.
- Aggregate text contents from selected files into a single output file.
- Detect and exclude non-text files from aggregation.
- Display progress during the aggregation process.

## Requirements

- .NET Framework 4.7.2 or later
- Windows OS

## Installation

1. Clone the repository from GitHub:
    ```bash
    git clone https://github.com/your-username/code-aggregator.git
    ```
2. Open the solution in Visual Studio.
3. Build the solution to restore the NuGet packages and compile the project.

## Usage

1. Run the application.
2. Select a root folder when prompted.
3. Use the tree view to select or deselect folders and files.
4. Click "Start Operation" to begin aggregating the text contents of the selected files.
5. Choose the location and name for the output file in the save file dialog.

## Contributing

Contributions are welcome! Please fork the repository and create a pull request with your changes.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
