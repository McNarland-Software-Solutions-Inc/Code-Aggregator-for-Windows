# Code Aggregator

## Description

Code Aggregator is an application that allows you to select a root folder and aggregate all text files within the selected folder and its subfolders into a single text file. This is particularly useful for consolidating code files or other text-based files.

## Features

- Select a root folder for aggregation.
- Include or exclude specific files and folders using checkboxes.
- Save the aggregated text to a specified location.
- Option to open the aggregated file after the operation completes.

## How to Use

1. Click the 'Select Folder' button to choose the root folder for aggregation.
2. Use the checkboxes to include or exclude specific files and folders. By default, all folders and files are included.
3. Click 'Start Operation' to begin the aggregation process. You will be prompted to specify the location and name of the output file.
4. Note: It is recommended to exclude folders like .git, .vs, or other dependency folders to avoid including unnecessary files.

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

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any changes.

## License

This project is licensed under the MIT License.
