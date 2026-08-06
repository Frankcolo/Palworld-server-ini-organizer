using System;
using System.IO;
using System.Linq;
namespace palserver_easy_reader
{
    class Program
    {
        static void Main(string[] args)
        {
            //variables
            bool isint = false;
            bool found = false;
            char yn;
            int selection = 0;
            int count = 0;
            string filename = "";
            string[] settings = new string[0];
            string raw;
            string command;
            string edit;
            Console.WriteLine("Enter .ini location");
            while (found == false)
            {
                filename = Console.ReadLine();
                if (File.Exists(filename))
                {
                    //takes the ini that wasn't line broken and splits it along the comma as long as the ini was found
                    found = true;
                    raw = File.ReadAllText(filename);
                    settings = raw.Split(',');
                    //Prints each line of the now separated list
                    foreach (string test in settings)
                    {
                        Console.Write("[{0}]: ", count);
                        Console.WriteLine(test);
                        //count needed to list the array numbers
                        count++;
                    }
                }
                else
                {
                    Console.WriteLine("File not found, Try again:\n");
                }
            }
            while (true)
            {
                Console.WriteLine("Which setting do you wish to change?");
                while (isint == false)
                {
                    //Makes sure user selection is within numbers that make sense (and are actual numbers)
                    try
                    {
                        selection = Convert.ToInt32(Console.ReadLine());
                        if (selection <= settings.Length && selection >= 0)
                        {
                            isint = true;
                        }
                        else
                        {
                            Console.WriteLine("invalid selection");
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid selection");
                    }
                }
                Console.WriteLine(settings[selection]);
                //takes the selection and saves everything left of the equal sign
                command = settings[selection].Split('=').First();
                //resets the found bool
                found = false;
                while (found == false)
                {
                    //takes a new entry from the user and merges it with the previously split front half
                    Console.WriteLine("Enter new setting:");
                    edit = Console.ReadLine();
                    Console.WriteLine("{0}={1}", command, edit);
                    Console.WriteLine("Is this correct? (Y/N)");
                    //verifies if the user enters y or n or something incorrect
                    yn = Verify();
                    if (char.ToUpper(yn) == 'Y')
                    {
                        //if Y was entered combines everything together and readd the = 
                        found = true;
                        settings[selection] = Convert.ToString(command + "=" + edit);
                        //Console.WriteLine(settings[selection]);
                    }
                }
                Console.WriteLine("Continue changing entries?\n File will only save when N is selected here. (Y/N)");
                yn = Verify();
                //after another verification either resets bools to run again or writes to the file to finalize
                if (char.ToUpper(yn) == 'N')
                {
                    Done(settings,filename);
                    return;
                }
                else
                {
                    isint = false;
                    found = false;
                }
            }
        }
        public static char Verify()
        {
            char verify = 'a';
            while (true)
            {
                try
                {
                    //checks if the entered value is Y or N and if not sends error and user must try again
                    verify = Convert.ToChar(Console.ReadLine());
                    while (char.ToUpper(verify) != 'Y' && char.ToUpper(verify) != 'N')
                    {
                        Console.WriteLine("Invalid entry please try again:");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid entry please try again:");
                }
                if (char.ToUpper(verify) == 'Y' || char.ToUpper(verify) == 'N')
                {
                    return verify;
                }
            }
        }
        public static void Done(string[] settings,string filename)
        {
            //puts everything back the way we found it with the changes now made
            string raw;
            raw = string.Join(",", settings);
            Console.WriteLine(raw);
            File.WriteAllText(filename, raw);
        }
    }
}
