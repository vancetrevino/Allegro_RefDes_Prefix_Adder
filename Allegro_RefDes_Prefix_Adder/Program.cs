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
string[] csaFileList;


GetSchematicDirectory(ref schematicDirectory);
FindAllCsaFiles(schematicDirectory, out csaFileList);

prefixID = AskUserInput("What prefix ID would you like to add to your REF DES?");






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

void FindAllCsaFiles(string directory, out string[] fileList)
{
    fileList = Directory.GetFiles(directory, "*.csa", SearchOption.AllDirectories);
}

void ParseCSAFiles()
{

}