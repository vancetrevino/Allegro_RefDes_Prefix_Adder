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
string currentID = " ";
string prefixID = "";
string[] csaFileArray;


// WARNING LABEL
Console.WriteLine(" \n\n +++ WARNING!! ");
Console.WriteLine(" +++ Before running this program, please backup your current schematic directory.");
Console.WriteLine(" +++ More specifically, backup you 'sch_1' folder to a different directory.");
Console.WriteLine(" +++ DO NOT put your 'sch_1' folder in the same directory as your current project.");
Console.WriteLine(" +++ In the event that this program provides incorrect results, ");
Console.WriteLine(" +++ you can revert back to your current design. \n\n");

GetSchematicDirectory(ref schematicDirectory);
FindAllCsaFiles(schematicDirectory, out csaFileArray);

currentID = AskUserInput("\nWhat is the current prefix ID for your REF DES? If none, press 'ENTER'.");
CheckForBlankUserInput(ref currentID);

while (String.IsNullOrEmpty(prefixID))
{
    prefixID = AskUserInput("\nWhat prefix ID would you like to add to your REF DES?");
    if (String.IsNullOrEmpty(prefixID))
    {
        Console.WriteLine("Your chosen prefix ID is empty/blank. Please input a valid ID.");
    }
}

currentID = currentID.ToUpper();
prefixID  = prefixID.ToUpper();

ParseCSAFiles(currentID, prefixID, csaFileArray);
DeleteNonCsaFiles(schematicDirectory);

Console.WriteLine("\nAll files have been overwritten with updated Ref Des.");
Console.WriteLine("And all non .csa files in the 'sch_1' directory have been deleted.");
Console.WriteLine("Please open your schematic project and run 'Save Hierarchy'");

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

void CheckForBlankUserInput(ref string userInput)
{
    if (String.IsNullOrEmpty(userInput))
    {
        userInput = " ";
    }
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
void ParseCSAFiles(string oldID, string newID, string[] fileArray)
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
                tempLine = SearchForRefDes(line, oldID, newID);
                outputFile.WriteLine(tempLine);
            }

            outputFile.Close();
            File.Replace(outputFileName, file, backupFileName);
        }

        Console.WriteLine($" - Finished overwritting the file: {outputFileName}");
    }
}

string SearchForRefDes(string csaFileLine, string oldID, string newID)
{
    string outputResult = csaFileLine;
    string matchResultValue = "";
    int matchResultIndex = 0;
    // match.Value   match.Index

    string resistorPattern   = oldID + @"R[0-9]+$";
    string capacitorPattern  = oldID + @"C[0-9]+$";
    string inductorPattern   = oldID + @"L[0-9]+$";
    string transistorPattern = oldID + @"Q[0-9]+$";
    string diodePattern      = oldID + @"D[0-9]+$";
    string connectorPattern  = oldID + @"J[0-9]+$";
    string microPartPattern  = oldID + @"U[0-9]+$";
    string crystalPattern    = oldID + @"Y[0-9]+$";
    string testPointPattern  = oldID + @"TP[0-9]+$";

    Match resistorMatch   = Regex.Match(csaFileLine, resistorPattern);
    Match capacitorMatch  = Regex.Match(csaFileLine, capacitorPattern);
    Match inductorMatch   = Regex.Match(csaFileLine, inductorPattern);
    Match transistorMatch = Regex.Match(csaFileLine, transistorPattern);
    Match diodeMatch      = Regex.Match(csaFileLine, diodePattern);
    Match connectorMatch  = Regex.Match(csaFileLine, connectorPattern);
    Match microPartMatch  = Regex.Match(csaFileLine, microPartPattern);
    Match crystalMatch    = Regex.Match(csaFileLine, crystalPattern);
    Match testPointMatch  = Regex.Match(csaFileLine, testPointPattern);

    if(resistorMatch.Success)
    {
        matchResultIndex = resistorMatch.Index;
        matchResultValue = resistorMatch.Value;
    }
    else if(capacitorMatch.Success)
    {
        matchResultIndex = capacitorMatch.Index;
        matchResultValue = capacitorMatch.Value;
    }
    else if(inductorMatch.Success)
    {
        matchResultIndex = inductorMatch.Index;
        matchResultValue = inductorMatch.Value;
    }
    else if (transistorMatch.Success)
    {
        matchResultIndex = transistorMatch.Index;
        matchResultValue = transistorMatch.Value;
    }
    else if (diodeMatch.Success)
    {
        matchResultIndex = diodeMatch.Index;
        matchResultValue = diodeMatch.Value;
    }
    else if(connectorMatch.Success)
    {
        matchResultIndex = connectorMatch.Index;
        matchResultValue = connectorMatch.Value;
    }
    else if(microPartMatch.Success)
    {
        matchResultIndex = microPartMatch.Index;
        matchResultValue = microPartMatch.Value;
    }
    else if(crystalMatch.Success)
    {
        matchResultIndex = crystalMatch.Index;
        matchResultValue = crystalMatch.Value;
    }
    else if(testPointMatch.Success)
    {
        matchResultIndex = testPointMatch.Index;
        matchResultValue = testPointMatch.Value;
    }

    if(matchResultIndex != 0)
    {
        string newRefDesFromResult = matchResultValue.Replace(oldID, newID);
        outputResult = csaFileLine.Replace(matchResultValue, " " + newRefDesFromResult);
    }
    
    return outputResult; 
}

void DeleteNonCsaFiles(string directory)
{
    string[] allFiles = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

    foreach(var file in allFiles)
    {
        if (file.Contains(".csv") || file.Contains(".csb") ||
            file.Contains(".cpc") || file.Contains(".dcf"))
        {
            File.Delete(file);
        }
    }
}