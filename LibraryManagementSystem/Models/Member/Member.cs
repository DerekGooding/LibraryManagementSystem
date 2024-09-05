﻿using LibraryManagementSystem.Models.Books;
using LibraryManagementSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem.Models.Member
{
    // uniqueness = email
    internal abstract class Member
    {
        public enum MemberType
        {
            Student,
            Teacher
        }

        private const int MEMBER_ID_LENGTH = 8;

        private readonly string _memberId = CustomUtils.GenerateUniqueID(0, MEMBER_ID_LENGTH);
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty; // unique prop
        private MemberType _type;

        protected HashSet<string> BorrowedBookIds { get; set; } = new HashSet<string>();
        public static readonly List<string> MemberTypeNames = Enum.GetNames(typeof(MemberType)).ToList();
        public MemberType Type { get => _type; protected set => _type = value; }
        public string Name { get => $"{FirstName} {LastName}"; }
        public string MemberId { get => _memberId; }

        public string FirstName
        {
            get => _firstName;
            protected set => _firstName = !string.IsNullOrWhiteSpace(value) && !string.IsNullOrEmpty(value) ? value.Trim().ToLower() : string.Empty;
        }

        public string LastName
        {
            get => _lastName;
            protected set => _lastName = !string.IsNullOrWhiteSpace(value) && !string.IsNullOrEmpty(value) ? value.Trim().ToLower() : string.Empty;
        }

        public string Email
        {
            get => _email;
            private set
            {
                if (!Validator.IsValidEmail(value))
                    throw new ArgumentException($"Can't set an invalid email: '{value}' while creating system member.");

                _email = value.Trim().ToLower();
            }
        }

        protected Member(string firstName, string lastName, string email, MemberType type)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Type = type;
        }

        // uniqueness = email
        public override bool Equals(object obj) => obj is Member member && Email.Equals(member.Email.Trim(), StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => Email.GetHashCode();

        public override string ToString()
        {
            return $"Member details:\n\tid: '{MemberId}'" +
                $"\n\tname: '{Name}'" +
                $"\n\temail: '{Email}'" +
                $"\n\ttype: '{Type}'";
        }

        public static bool SelectMemberTypeUsingMenuSelector(out MemberType result, string message = "Use the arrow keys to navigate and press Enter to select member type:")
        {
            string selectedMemberTypeInput = MenuSelector.SelectOption(MemberTypeNames, message);
            bool isValidSelectedMemberType = Enum.TryParse(selectedMemberTypeInput, false, out MemberType validMemberType);
            result = validMemberType;
            return isValidSelectedMemberType;
        }

        public bool FindIfBookBorrowedByMemberByBooKId(string bookId, out bool validationError)
        {
            validationError = false;

            if (!Book.IsValidBookId(bookId))
            {
                validationError = true;
                return false;
            }

            return BorrowedBookIds.Contains(bookId);
        }

        public bool TryReturnBook(string bookId, out bool validationError)
        {
            validationError = false;

            if (!Book.IsValidBookId(bookId))
            {
                validationError = true;
                return false;
            }

            return BorrowedBookIds.Remove(bookId);
        }

        public bool BorrowBook(string bookId, out bool validationError)
        {
            validationError = false;

            if (Validator.IsStringNullOrEmptyOrWhitespace(bookId))
            {
                validationError = true;
                return false;
            }

            if (!(bookId.Length == 8))
            {
                validationError = true;
                return false;
            }

            return BorrowedBookIds.Add(bookId);
        }
    }
}