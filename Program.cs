// Filename: Program.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            HandleCommandLineArgs(args);
        }
        else
        {
            RunInGuiMode();
        }
    }

    private static void RunInGuiMode()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());
    }

    private static void HandleCommandLineArgs(string[] args)
    {
        string sourceFolder = args[0];
        string? outputFile = null;
        bool changeOutputFile = false;
        bool quietMode = false;
        var additionalFolders = new List<FolderStatus>();

        for (int i = 1; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "?":
                    ShowHelp();
                    return;
                case var o when o.StartsWith("-o:"):
                    outputFile = o.Substring(3);
                    break;
                case var oc when oc.StartsWith("-oc:"):
                    outputFile = oc.Substring(4);
                    changeOutputFile = true;
                    break;
                case var a when a.StartsWith("-a:"):
                    additionalFolders.Add(new FolderStatus { FolderPath = a.Substring(3), Included = true });
                    break;
                case var ac when ac.StartsWith("-ac:"):
                    additionalFolders.Add(new FolderStatus { FolderPath = ac.Substring(4), Included = true });
                    break;
                case "-q":
                    quietMode = true;
                    break;
            }
        }

        if (quietMode)
        {
            // Execute without showing any windows
            RunAggregation(sourceFolder, outputFile, changeOutputFile, additionalFolders);
        }
        else
        {
            var selection = SelectionManager.GetFolderSelection(sourceFolder);

            if (outputFile != null)
            {
                selection.OutputFile = outputFile;
                if (changeOutputFile)
                {
                    SelectionManager.UpdateFolderSelection(selection);
                }
            }

            if (additionalFolders.Any())
            {
                foreach (var folder in additionalFolders)
                {
                    if (!Directory.Exists(Path.Combine(sourceFolder, folder.FolderPath)) && !File.Exists(Path.Combine(sourceFolder, folder.FolderPath)))
                    {
                        Console.WriteLine(folder.FolderPath.EndsWith(".ext") ? "2" : "1");
                        return;
                    }

                    selection.SelectedFolders.Add(folder);
                }

                if (additionalFolders.Any(f => f.FolderPath.StartsWith("-ac:")))
                {
                    SelectionManager.UpdateFolderSelection(selection);
                }
            }

            try
            {
                FileAggregator.AggregateFiles(sourceFolder, selection.SelectedFolders.Where(f => f.Included).Select(f => Path.Combine(sourceFolder, f.FolderPath)), selection.OutputFile);
                Console.WriteLine("0");
            }
            catch
            {
                Console.WriteLine("3");
            }
        }
    }

    private static void RunAggregation(string sourceFolder, string? outputFile, bool changeOutputFile, List<FolderStatus> additionalFolders)
    {
        var selection = SelectionManager.GetFolderSelection(sourceFolder);

        if (outputFile != null)
        {
            selection.OutputFile = outputFile;
            if (changeOutputFile)
            {
                SelectionManager.UpdateFolderSelection(selection);
            }
        }

        if (additionalFolders.Any())
        {
            foreach (var folder in additionalFolders)
            {
                if (!Directory.Exists(Path.Combine(sourceFolder, folder.FolderPath)) && !File.Exists(Path.Combine(sourceFolder, folder.FolderPath)))
                {
                    Console.WriteLine(folder.FolderPath.EndsWith(".ext") ? "2" : "1");
                    return;
                }

                selection.SelectedFolders.Add(folder);
            }

            if (additionalFolders.Any(f => f.FolderPath.StartsWith("-ac:")))
            {
                SelectionManager.UpdateFolderSelection(selection);
            }
        }

        try
        {
            FileAggregator.AggregateFiles(sourceFolder, selection.SelectedFolders.Where(f => f.Included).Select(f => Path.Combine(sourceFolder, f.FolderPath)), selection.OutputFile);
            Console.WriteLine("0");
        }
        catch
        {
            Console.WriteLine("3");
        }
    }

    private static void ShowHelp()
    {
        Console.WriteLine("Usage: CodeAggregator [source_folder] [options]");
        Console.WriteLine("Options:");
        Console.WriteLine("  -o:<output_file>        Specify the output file path.");
        Console.WriteLine("  -oc:<output_file>       Specify and change the output file path in the JSON.");
        Console.WriteLine("  -a:<folder_or_file>     Add a folder or file for this aggregation.");
        Console.WriteLine("  -ac:<folder_or_file>    Add a folder or file for this aggregation and save to JSON.");
        Console.WriteLine("  -q                      Run in quiet mode (no output).");
        Console.WriteLine("  ?                       Show this help message.");
        Console.WriteLine("Error Codes:");
        Console.WriteLine("  0 - Success.");
        Console.WriteLine("  1 - Folder not found.");
        Console.WriteLine("  2 - File not found.");
        Console.WriteLine("  3 - Error outputting to output file.");
        Console.WriteLine("  4 - Source folder not found.");
    }
}
