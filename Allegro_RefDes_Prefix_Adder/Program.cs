/*
 * This program will take the schematic pages from an
 * Allegro HDL project and parse each .csa page for 
 * each REF DES and add a user inputted PREFIX ID
 * 
 * It will delete all other unnecessary schematic files, 
 * keeping just the .csa files.
 * 
 * -- Written by Vance Trevino.
 */

Console.WriteLine(@" -- Allegro REF DES Prefix Adder --
 * This program will take the schematic pages from an
 * Allegro HDL project and parse each .csa page for 
 * each REF DES and add a user inputted PREFIX ID
 * 
 * It will delete all other unnecessary schematic files, 
 * keeping just the .csa files.
 * 
 * -- Written by Vance Trevino. \n\n");

// List of all global variables
string schDirectory = "";



void GetSchematicDirectory()
{
    while (String.IsNullOrEmpty(schDirectory))
    {
        string tempDirectory = AskUserInput("Please input the directory of your schematic project.");

        if (Directory.GetFiles(tempDirectory, "*.cpm").Length == 0 ||
            Directory.GetFiles(tempDirectory, "*.csa").Length == 0)
        {
            Console.WriteLine("This is not a valid directory. Cannot find any .csa or .cpm files here.");
        }
        else
        {
            schDirectory = tempDirectory;
        }
    }
}

string AskUserInput(string displayString)
{
    Console.WriteLine(displayString);
    string userInput = Console.ReadLine();

    return userInput;
}

void ParseSchematicDirectory()
{

}

void ParseCSAFiles()
{

}