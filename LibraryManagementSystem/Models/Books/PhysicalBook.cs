namespace LibraryManagementSystem.Models.Books;

/*
 * TODO: Add book copies
*/

internal class PhysicalBook(string title, string author, string shelfLocation) : Book(title, author, BookType.Physical)
{
    public string ShelfLocation { get; set; } = shelfLocation;

    //public int CalculatePhysicalBookWeight() => 12;

    public override string ToString() => base.ToString() + $"\n\tShelf-location: '{ShelfLocation}'";
}