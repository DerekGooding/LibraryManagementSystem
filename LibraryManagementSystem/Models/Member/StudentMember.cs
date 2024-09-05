using LibraryManagementSystem.Utils;

namespace LibraryManagementSystem.Models.Member
{
    internal class StudentMember : Member
    {
        public string StudentId { get; } = CustomUtils.GenerateUniqueID(0, 8);

        public StudentMember(
            string firstName,
            string lastName,
            string email) : base(firstName: firstName, lastName: lastName, email: email, type: MemberType.Student)
        {
        }

        public override string ToString() => base.ToString() + $"\n\tstudent id: '{StudentId}'";
    }
}