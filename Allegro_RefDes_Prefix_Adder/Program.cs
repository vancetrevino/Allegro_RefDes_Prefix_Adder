/*
 * This program will take the schematic pages from an
 * Allegro HDL project and parse each .csa page for 
 * each REF DES and add a user specified PREFIX ID
 * 
 * It will delete all other unnecessary schematic files, 
 * keeping just the .csa files.
 * 
 * -- Written by Vance Trevino.
 */

// Additional namespace 
using System.Text.RegularExpressions;
 
Console.WriteLine(@" -- Allegro REF DES Prefix Adder --
 * This program will take the schematic pages from an
 * Allegro HDL project and parse each .csa page for 
 * each REF DES and add a user specified PREFIX ID
 * 
 * It will delete all other unnecessary schematic files, 
 * keeping just the .csa files.
 * 
 * -- Written by Vance Trevino. \n\n");

// List of all global variables
string schematicDirectory = "";
string prefixID = "";
string[] csaFileArray;


// WARNING LABEL
Console.WriteLine(" \n\n +++ WARNING!! ");
Console.WriteLine(" +++ Before running this program, please backup your current schematic directory ");
Console.WriteLine(" +++ More specifically, backup you 'sch_1' folder");
Console.WriteLine(" +++ In the event that this program provides incorrect results, ");
Console.WriteLine(" +++ you can revert back to your current design. \n\n");

GetSchematicDirectory(ref schematicDirectory);
FindAllCsaFiles(schematicDirectory, out csaFileArray);
prefixID = AskUserInput("What prefix ID would you like to add to your REF DES?");
ParseCSAFiles(prefixID, csaFileArray);
DeleteNonCsaFiles(schematicDirectory);
Console.WriteLine("");

void GetSchematicDirectory(ref string directory)
{
    while (String.IsNullOrEmpty(directory))
    {
        string tempDirectory = AskUserInput("Please input the directory of your schematic project.");

        if (Directory.GetFiles(tempDirectory, "*.cpm").Length == 0 &&
            Directory.GetFiles(tempDirectory, "*.csa").Length == 0)
        {
            Console.WriteLine("This is not a valid directory. Cannot find any .csa or .cpm files here.");
        }
        else
        {
            directory = tempDirectory;
        }
    }
}

string AskUserInput(string displayString)
{
    Console.WriteLine(displayString);
    string userInput = Console.ReadLine();

    return userInput;
}

void FindAllCsaFiles(string directory, out string[] fileArray)
{
    fileArray = Directory.GetFiles(directory, "*.csa", SearchOption.AllDirectories);
}


/*
 * Method Purpose:
 * Read in every .csa file in the directory, line by line.
 * Create a temporary output file .csa file to save each .csa file to
 * While reading each line, find the lines where a REF DES is found
 * Prepend that REF DES with the prefix ID 
 * Save the temp file by overwritting the existing .csa file
 * 
 */
void ParseCSAFiles(string prefixID, string[] fileArray)
{
    foreach(var file in fileArray)
    {
        string outputFileName = Path.GetFileName(file);
        string backupFileName = outputFileName + ".bac";
        string tempLine = "";

        using(var outputFile = new StreamWriter(outputFileName))
        {
            foreach(var line in File.ReadLines(file))
            {
                tempLine = SearchForRefDes(line, prefixID);
                outputFile.WriteLine(tempLine);
            }

            outputFile.Close();
            File.Replace(outputFileName, file, backupFileName);
        }

        Console.WriteLine($" - Finished overwritting the file: {outputFileName}");
    }
}

string SearchForRefDes(string csaFileLine, string prefixID)
{
    string outputResult = csaFileLine;
    int matchResultIndex = 0;
    // match.Value   match.Index

    string resistorPattern  = @"R[0-9]+$";
    string capacitorPattern = @"C[0-9]+$";
    string inductorPattern  = @"L[0-9]+$";
    string connectorPattern = @"J[0-9]+$";
    string microPartPattern = @"U[0-9]+$";
    string crystalPattern   = @"Y[0-9]+$";
    string testPointPattern = @"TP[0-9]+$";

    Match resistorMatch  = Regex.Match(csaFileLine, resistorPattern);
    Match capacitorMatch = Regex.Match(csaFileLine, capacitorPattern);
    Match inductorMatch  = Regex.Match(csaFileLine, inductorPattern);
    Match connectorMatch = Regex.Match(csaFileLine, connectorPattern);
    Match microPartMatch = Regex.Match(csaFileLine, microPartPattern);
    Match crystalMatch  = Regex.Match(csaFileLine, crystalPattern);
    Match testPointMatch  = Regex.Match(csaFileLine, testPointPattern);

    if(resistorMatch.Success)
    {
        matchResultIndex = resistorMatch.Index;
    }
    else if(capacitorMatch.Success)
    {
        matchResultIndex = capacitorMatch.Index;
    }
    else if(inductorMatch.Success)
    {
        matchResultIndex = inductorMatch.Index;
    }
    else if(connectorMatch.Success)
    {
        matchResultIndex = connectorMatch.Index;
    }
    else if(microPartMatch.Success)
    {
        matchResultIndex = microPartMatch.Index;
    }
    else if(crystalMatch.Success)
    {
        matchResultIndex = crystalMatch.Index;
    }
    else if(testPointMatch.Success)
    {
        matchResultIndex = testPointMatch.Index;
    }

    if(matchResultIndex != 0)
    {
        outputResult = csaFileLine.Insert(matchResultIndex, prefixID);
    }
    
    return outputResult; 
}

void DeleteNonCsaFiles(string directory){

}