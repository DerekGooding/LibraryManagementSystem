using LibraryManagementSystem.Services;
using System;

namespace LibraryManagementSystem
{
    internal class Program
    {
        private static void Main()
        {
            MenuService.Start();

            Console.ReadLine();
        }
    }
}