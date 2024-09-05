using LibraryManagementSystem.Interfaces.Infrastructure.Services;
using LibraryManagementSystem.Models.Books;
using LibraryManagementSystem.Models.Member;
using LibraryManagementSystem.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LibraryManagementSystem.Services;

internal class LibManagementSystem : ILibraryService
{
    private HashSet<Member> Members { get; } = [];
    private HashSet<Book> Books { get; } = [];

    // derived class props
    public ReadOnlyCollection<StudentMember> StudentMembers => Members.OfType<StudentMember>().ToList().AsReadOnly();

    public ReadOnlyCollection<TeacherMember> TeacherMembers => Members.OfType<TeacherMember>().ToList().AsReadOnly();
    public ReadOnlyCollection<PhysicalBook> PhysicalBooks => Books.OfType<PhysicalBook>().ToList().AsReadOnly();
    public ReadOnlyCollection<EBook> EBooks => Books.OfType<EBook>().ToList().AsReadOnly();
    public long TotalStudentMembersCount => StudentMembers.Count;
    public long TotalTeacherMembersCount => TeacherMembers.Count;
    public long TotalMembersCount => TotalStudentMembersCount + TotalTeacherMembersCount;
    public long TotalBorrowedPhysicalBooks => Books.OfType<PhysicalBook>().ToList().FindAll(book => book.IsBorrowed).Count;
    public long TotalBorrowedEBooks => Books.OfType<EBook>().ToList().FindAll(book => book.IsBorrowed).Count;
    public ReadOnlyCollection<string> PhysicalBookTitlesList => Books.OfType<PhysicalBook>().ToList().ConvertAll(book => book.Title).AsReadOnly();
    public ReadOnlyCollection<string> EBookTitlesList => Books.OfType<EBook>().ToList().ConvertAll(book => book.Title).AsReadOnly();

    public ReadOnlyCollection<string> AllBookTitlesList
    {
        get
        {
            List<string> _allBookTitlesList = [.. PhysicalBookTitlesList, .. EBookTitlesList];
            return new ReadOnlyCollection<string>(_allBookTitlesList);
        }
    }

    public long TotalBooksCount => PhysicalBooks.Count + EBooks.Count;

    //methods
    // done
    public void RegisterMember()
    {
        // TODO: Validate member input details

        // member first name input
        Write("Enter first name: ");
        string firstName = ReadLine().Trim().ToLower();

        // validation: first name input
        if (string.IsNullOrWhiteSpace(firstName))
        {
            WriteLine($"[Invalid Input]: member's firstName can't be empty, or only contains whitespace, entered value = '{firstName}'");
            return;
        }

        // member last name input
        Write("Enter last name: ");
        string lastName = ReadLine().Trim().ToLower();

        // validation: last name input
        if (string.IsNullOrWhiteSpace(lastName))
        {
            WriteLine($"[Invalid Input]: member's lastName can't be empty, or only contains whitespace, entered value = '{lastName}'");
            return;
        }

        // member email input
        Write("Enter email: ");
        string email = ReadLine().Trim();

        // validation: email input
        if (!Validator.IsValidEmail(email))
        {
            WriteLine($"[INVAID INPUT]: Received invalid email, entered value = '{email}'");
            return;
        }

        // taking valid member type input and validating it
        bool memberTypeSelection = Member.SelectMemberTypeUsingMenuSelector(out Member.MemberType memberType);
        if (!memberTypeSelection)
        {
            WriteLine("[ERROR]: Error while selecting member type");
            return;
        }

        // registering members according to selected member type
        if (memberType == Member.MemberType.Student)
        {
            StudentMember newMember = new(firstName, lastName, email);
            bool memberRegistered = Members.Add(newMember);
            if (!memberRegistered)
            {
                WriteLine($"[ALERT]: member with email = '{email.Trim().ToLower()}' has already been registered in the system!!");
                return;
            }

            WriteLine("[SUCCESS]: Student member successfully registered with following details: !!");
            WriteLine(newMember);
        }
        else if (memberType == Member.MemberType.Teacher)
        {
            TeacherMember newMember = new(firstName, lastName, email);
            bool memberRegistered = Members.Add(newMember);
            if (!memberRegistered)
            {
                WriteLine($"[ALERT]: member with email = '{email.Trim().ToLower()}' has already been registered in the system!!");
                return;
            }

            WriteLine("[SUCCESS]: Teacher member successfully registered with following details: !!");
            WriteLine(newMember);
            return;
        }
        else
        {
            WriteLine("[ERROR]: Received invalid member type while registering member in the system.");
        }
    }

    // bool to check if some kind of error while finding member or if member exists or not
    private bool FindMemberByEmail(string email, out Member result)
    {
        bool operationSuccess = false;
        result = null;

        if (!Validator.IsValidEmail(email))
            return operationSuccess;

        email = email.Trim();

        // checking if member exists
        Member member = Members.FirstOrDefault(m => m.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (member != null)
        {
            operationSuccess = true;
            result = member;
        }

        return operationSuccess;
    }

    // done
    public void AddBook()
    {
        // book title input
        Write("Enter book title: ");
        string bookTitle = ReadLine().Trim().ToLower();

        // validating book title
        if (string.IsNullOrWhiteSpace(bookTitle))
        {
            WriteLine($"[Invalid Input]: book title can't be empty, or only contains whitespace, entered value = '{bookTitle}'");
            return;
        }

        // book author input
        Write("Enter book author: ");
        string BookAuthor = ReadLine().Trim().ToLower();

        // validating book author
        if (string.IsNullOrWhiteSpace(BookAuthor))
        {
            WriteLine($"[Invalid Input]: book author can't be empty, or only contains whitespace, entered value = '{BookAuthor}'");
            return;
        }

        // book type input
        bool bookTypeSelectionSuccess = Book.SelectBookTypeUsingMenuSelector(out Book.BookType selectedBookType);

        if (!bookTypeSelectionSuccess)
        {
            WriteLine("[ERROR]: Error while book type selection.");
            return;
        }

        // taking actions according to selected book type
        if (selectedBookType.Equals(Book.BookType.Physical))  // for physical book creation
        {
            // book shelfLocation input
            Write("Enter book shelfLocation: ");
            string bookShelfLocation = ReadLine().Trim();

            // validating book shelfLocation
            if (string.IsNullOrWhiteSpace(bookShelfLocation))
            {
                WriteLine($"[Invalid Input]: book shelfLocation can't be empty, or only contains whitespace, entered value = '{bookShelfLocation}'");
                return;
            }

            Book newBook = new PhysicalBook(title: bookTitle, author: BookAuthor, shelfLocation: bookShelfLocation);
            bool bookAdded = Books.Add(newBook);

            // checking if book already exists in the "books" hash set, if not book added then it already exists
            if (!bookAdded)
            {
                WriteLine("[ALERT]: Physical book with the following details already exists in the system!!");
                WriteLine(newBook);
                return;
            }

            WriteLine("[SUCCESS]: Physical book has been successfully added to the system!!");
            WriteLine(newBook);
        }
        else if (selectedBookType.Equals(Book.BookType.EBook))  // for e-book creation
        {
            // book download link input
            Write("Enter book download link: ");
            string downloadLinkInput = ReadLine().Trim();

            // validating book  download link, can't be empty or contains whitespace only
            if (string.IsNullOrWhiteSpace(downloadLinkInput))
            {
                WriteLine($"[Invalid Input]: book download link can't be empty, or only contains whitespace, entered value = '{downloadLinkInput}'");
                return;
            }

            // validating book download link, URL validation
            bool isValidDownloadLink = Validator.IsValidURL(downloadLinkInput, out string downloadLink);
            if (!isValidDownloadLink)
            {
                WriteLine($"[Invalid Input]: book download link URL is invalid, entered value = '{downloadLinkInput}'");
                return;
            }

            Book newBook = new EBook(title: bookTitle, author: BookAuthor, downloadLink: downloadLink);
            bool bookAdded = Books.Add(newBook);

            // checking if book already exists in the "books" hash set, if not book added then it already exists
            if (!bookAdded)
            {
                WriteLine("[ALERT]: E-book with the following details already exists in the system!!");
                WriteLine(newBook);
                return;
            }

            WriteLine("[SUCCESS]: E-book has been successfully added to the system!!");
            WriteLine(newBook);
        }
        else
        {
            WriteLine("[ERROR]: Received invalid member type while registering member in the system.");
            return;
        }
    }

    /*
         *  Flow:
         *  - input member email with email validation, if member exists then book can be borrowed and if not, then not
         *  - if member exists, ask book details
         *  -   input title, author and type with their validation
         *      - check if book exists
         *          -   if exists,
         *              -   check if book has already been borrowed or not
         *                  - if yes
         *                      - alert user
         *                  - if not
         *                      - borrow book method on book
         *                      - add book id to the borrowed books list of member
         *          -   if not, then alert user
    */

    public void BorrowBook()
    {
        // member email input
        Write("Enter member email: ");
        string memberEmail = ReadLine().Trim();

        // validation: member email input
        if (!Validator.IsValidEmail(memberEmail))
        {
            WriteLine($"[INVALID INPUT]: Received invalid email, entered value = '{memberEmail}'");
            return;
        }

        // checking if member exists
        bool memberExists = FindMemberByEmail(memberEmail, out Member member);
        if (!memberExists)
        {
            WriteLine($"[NOT FOUND ERROR]: Operation failed because member with email = '{memberEmail}' doesn't exists in the system");
            WriteLine("[SYSTEM SUGGESTION]: Signup by creating a new member in the system");
            return;
        }

        // asking book details
        // book title input
        Write("Enter book title: ");
        string bookTitle = ReadLine().Trim().ToLower();

        // validating book title
        if (string.IsNullOrWhiteSpace(bookTitle))
        {
            WriteLine($"[Invalid Input]: book title can't be empty, or only contains whitespace, entered value = '{bookTitle}'");
            return;
        }

        // book author input
        Write("Enter book author: ");
        string bookAuthor = ReadLine().Trim().ToLower();

        // validating book author
        if (string.IsNullOrWhiteSpace(bookAuthor))
        {
            WriteLine($"[Invalid Input]: book author can't be empty, or only contains whitespace, entered value = '{bookAuthor}'");
            return;
        }

        // book type input
        bool bookTypeSelectionSuccess = Book.SelectBookTypeUsingMenuSelector(out Book.BookType selectedBookType);

        // system error
        if (!bookTypeSelectionSuccess)
        {
            WriteLine("[ERROR]: Error while book type selection.");
            return;
        }

        // checking if book exists with given book details
        Book book = Books.FirstOrDefault(_book => _book.Title.Equals(bookTitle) && _book.Author.Equals(bookAuthor) && _book.Type.Equals(selectedBookType));
        if (book == null)
        {
            WriteLine("[NOT FOUND]: book with entered details doesn't exist in the system!!");
            return;
        }

        // check if book has already been borrowed
        if (book.IsBorrowed)
        {
            WriteLine("[ALERT]: book with entered details has already been borrowed!!");
            return;
        }

        // Add book to the 'borrowed books list' of member
        bool memberBorrowBookOperationSuccess = member.BorrowBook(bookId: book.BookId, out bool validationError);
        if (!memberBorrowBookOperationSuccess)
        {
            if (validationError)
            {
                WriteLine("[SYSTEM ERROR]: Error while borrowing book to member");
                return;
            }
            WriteLine("[ALERT]: Book with given details has already been borrowed by the member");
            return;
        }

        // set book isBorrowed to true
        book.BorrowBook();

        WriteLine($"[SUCCESS]: Book with title: '{bookTitle}' has been successfully borrowed by member with name: '{member.Name}' and email: '{member.Email}'");
    }

    /*
      *  -  Take member email input with email validation
      *  -  Check if member exists
      *     -   if member exists
      *         -   ask book details: title, author and type with validation and check if book exists
      *             -   if book exists
      *                 -   check if book was previously borrowed or not
      *                     -   if book was borrowed, check if borrowed by member
      *                         -   if book borrowed by member
      *                             -   call returnBook method on both book and member
      *                         -   if book not borrowed by member, alert user
      *                 -   if book not borrowed, alert user
      *             -   if book doesn't exist then alert user
      *     -   if member doesn't exists, alert user to register
      */

    public void ReturnBook()
    {
        // member email input
        Write("Enter member email: ");
        string memberEmail = ReadLine().Trim();

        // validation: member email input
        if (!Validator.IsValidEmail(memberEmail))
        {
            WriteLine($"[INVALID INPUT]: Received invalid email, entered value = '{memberEmail}'");
            return;
        }

        // checking if member exists
        bool memberExists = FindMemberByEmail(memberEmail, out Member member);
        if (!memberExists)
        {
            WriteLine($"[NOT FOUND ERROR]: Operation failed because member with email = '{memberEmail}' doesn't exists in the system");
            WriteLine("[SYSTEM SUGGESTION]: Signup by creating a new member in the system");
            return;
        }

        // asking book details
        // book title input
        Write("Enter book title: ");
        string bookTitle = ReadLine().Trim().ToLower();

        // validating book title
        if (string.IsNullOrWhiteSpace(bookTitle))
        {
            WriteLine($"[Invalid Input]: book title can't be empty, or only contains whitespace, entered value = '{bookTitle}'");
            return;
        }

        // book author input
        Write("Enter book author: ");
        string bookAuthor = ReadLine().Trim().ToLower();

        // validating book author
        if (string.IsNullOrWhiteSpace(bookAuthor))
        {
            WriteLine($"[Invalid Input]: book author can't be empty, or only contains whitespace, entered value = '{bookAuthor}'");
            return;
        }

        // book type input
        bool bookTypeSelectionSuccess = Book.SelectBookTypeUsingMenuSelector(out Book.BookType selectedBookType);

        // system error
        if (!bookTypeSelectionSuccess)
        {
            WriteLine("[ERROR]: Error while book type selection.");
            return;
        }

        // checking if book exists with given book details
        Book book = Books.FirstOrDefault(_book => _book.Title.Equals(bookTitle) && _book.Author.Equals(bookAuthor) && _book.Type.Equals(selectedBookType));
        if (book == null)
        {
            WriteLine("[NOT FOUND]: book with entered details doesn't exist in the system!!");
            return;
        }

        // check if book was previously borrowed or not
        if (!book.IsBorrowed)
        {
            WriteLine("[ERROR]: book with entered details was never borrowed!!");
            return;
        }

        //  check if book borrowed by member by trying to return book
        string bookId = book.BookId;
        if (!member.TryReturnBook(bookId, out bool validationError))
        {
            if (validationError)
            {
                WriteLine("[SYSTEM ERROR]: Error while doing operation");
                return;
            }

            WriteLine("[ERROR]: Book wasn't borrowed by you!!");
            return;
        }

        book.ReturnBook();

        WriteLine($"Book with given details and borrowed by '{member.Name}' has been successfully returned!");
    }

    // done
    public void ConsoleAllBookTitles()
    {
        if (AllBookTitlesList.Count == 0)
        {
            WriteLine("[ALERT]: No book titles found!!");
            return;
        }

        WriteLine("Book titles:");
        foreach (string title in AllBookTitlesList)
            Write($"'{title}',\n");
    }
}