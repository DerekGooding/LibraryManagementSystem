using LibraryManagementSystem.Utils;
using System;

namespace LibraryManagementSystem.Services;

internal static class MenuService
{
    private const string ADD_BOOK_ACTION_TEXT = "[ACTION]: Add Book";
    private const string BORROW_BOOK_ACTION_TEXT = "[ACTION]: Borrow book";
    private const string RETURN_BOOK_ACTION_TEXT = "[ACTION]: Return book";
    private const string REGISTER_MEMBER_TEXT = "[ACTION]: Register Member";

    // update min and max of IntInValidRange() upon updating Actions
    private enum Actions
    {
        ClearConsole = -2,
        Exit,
        DisplayMenu,
        AddBook,
        BorrowBook,
        ReturnBook,
        GetTotalPhysicalBooks,
        GetTotalEBooks,
        GetTotalBorrowedPhysicalBooks,
        GetTotalBorrowedEBooks,
        GetAllBookTitles,
        GetTotalBooksCount,
        RegisterMember,
        GetTotalMembersCount,
        GetTotalTeacherMembersCount,
        GetTotalStudentMembersCount
    }

    private static void DisplayMenu()
    {
        // update Actions enum upon updating Menu
        WriteLine("[MENU]:");
        WriteLine("Enter -2 to clear console");
        WriteLine("Enter -1 to exit");
        WriteLine("Enter 0 to see menu");
        WriteLine("Enter 1 to add book");
        WriteLine("Enter 2 to borrow book");
        WriteLine("Enter 3 to return book");
        WriteLine("Enter 4 to get total physical books");
        WriteLine("Enter 5 to get total e-books");
        WriteLine("Enter 6 to get total borrowed physical books");
        WriteLine("Enter 7 to get total borrowed e-books");
        WriteLine("Enter 8 to get all book titles");
        WriteLine("Enter 9 to get system's total books count");
        WriteLine("Enter 10 to register a member");
        WriteLine("Enter 11 to get total members count");
        WriteLine("Enter 12 to get total teacher members count");
        WriteLine("Enter 13 to get total student members count");
        Write("\n");
    }

    private static int AskUserValidActionNumber()
    {
        bool askUserInput = true;
        int userInput = 0;

        while (askUserInput)
        {
            Write("Enter an action number: ");
            bool isValidUserInput = int.TryParse(ReadLine(), out int userInputInt);

            // checking if user input is a valid integer
            if (!isValidUserInput)
            {
                WriteLine("[ALERT]: Enter valid action number from Menu\n");
                continue;
            }

            // TODO: get total actions
            //int totalActions = Enum.GetValues(typeof(Actions)).Length;
            //Console.WriteLine($"total actions: {totalActions}");

            // checking if user input integer is in valid range of "Actions" numbers
            bool userInputInValidRange = CustomUtils.IntInValidRange(check: userInputInt, max: (int)Actions.GetTotalStudentMembersCount, min: (int)Actions.ClearConsole);
            if (!userInputInValidRange)
            {
                WriteLine("[ALERT]: Enter valid action number from Menu\n");
                continue;
            }

            userInput = userInputInt;
            askUserInput = false;
        }

        return userInput;
    }

    public static void Start()
    {
        LibManagementSystem libraryManagementSystem = new();

        WriteLine("============= WELCOME TO LIBRARY MANAGEMENT SYSTEM ==============\n");
        DisplayMenu();

        bool AskActionNumberUserInput = true;
        while (AskActionNumberUserInput)
        {
            switch (AskUserValidActionNumber())
            {
                case (int)Actions.ClearConsole:
                    Clear();
                    DisplayMenu();
                    break;

                case (int)Actions.Exit:
                    Environment.Exit(0);
                    //AskActionNumberUserInput = false;
                    break;

                case (int)Actions.DisplayMenu:
                    WriteLine();
                    DisplayMenu();
                    continue;
                case (int)Actions.AddBook:
                    WriteLine($"\n{ADD_BOOK_ACTION_TEXT}");
                    libraryManagementSystem.AddBook();
                    break;

                case (int)Actions.ReturnBook:
                    WriteLine($"\n{RETURN_BOOK_ACTION_TEXT}");
                    libraryManagementSystem.ReturnBook();
                    break;

                case (int)Actions.BorrowBook:
                    WriteLine($"\n{BORROW_BOOK_ACTION_TEXT}");
                    libraryManagementSystem.BorrowBook();
                    break;

                case (int)Actions.GetTotalPhysicalBooks:
                    WriteLine($"Total physical books: {libraryManagementSystem.PhysicalBooks.Count}");
                    break;

                case (int)Actions.GetTotalEBooks:
                    WriteLine($"Total e-books: {libraryManagementSystem.EBooks.Count}");
                    break;

                case (int)Actions.GetTotalBorrowedPhysicalBooks:
                    WriteLine($"Total borrowed physicalBooks: {libraryManagementSystem.TotalBorrowedPhysicalBooks}");
                    break;

                case (int)Actions.GetTotalBorrowedEBooks:
                    WriteLine($"Total borrowed e-books: {libraryManagementSystem.TotalBorrowedEBooks}");
                    break;

                case (int)Actions.GetAllBookTitles:
                    libraryManagementSystem.ConsoleAllBookTitles();
                    break;

                case (int)Actions.GetTotalBooksCount:
                    WriteLine($"System's total books count: {libraryManagementSystem.TotalBooksCount}");
                    break;

                case (int)Actions.RegisterMember:
                    WriteLine($"\n{REGISTER_MEMBER_TEXT}");
                    libraryManagementSystem.RegisterMember();
                    break;

                case (int)Actions.GetTotalMembersCount:
                    WriteLine($"Total members count: {libraryManagementSystem.TotalMembersCount}");
                    break;

                case (int)Actions.GetTotalTeacherMembersCount:
                    WriteLine($"Total members count: {libraryManagementSystem.TotalTeacherMembersCount}");
                    break;

                case (int)Actions.GetTotalStudentMembersCount:
                    WriteLine($"Total members count: {libraryManagementSystem.TotalStudentMembersCount}");
                    break;

                default:
                    WriteLine("[ERROR]: Something went wrong while choosing menu");
                    break;
            }
            WriteLine();
        }
    }
}