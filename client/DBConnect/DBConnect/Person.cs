using System;
using System.Text.RegularExpressions;

namespace DBConnect
{
    /// <summary>
    /// Sample class to be used in PersonRepository.
    /// </summary>
    public class Person : EntityBase
    {
        private string firstName;
        private string lastName;
        private DateTime dateOfBirth;

        public Person(int id) : base(id)
        {
        }

        public Person(int id, string firstName, string lastName, DateTime dateOfBirth) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (Regex.IsMatch(value, @"\p{L}+"))
                {
                    firstName = value;
                }
                else
                {
                    throw new ArgumentException("Unicode letters only.");
                }
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                if (Regex.IsMatch(value, @"\p{L}+"))
                {
                    lastName = value;
                }
                else
                {
                    throw new ArgumentException("Unicode letters only.");
                }
            }
        }

        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                if (value < DateTime.Now)
                {
                    dateOfBirth = value;
                }
                else
                {
                    throw new ArgumentException("You cannot be born in the future.");
                }
            }
        }

        public override string ToString()
        {
            return Id + ", " + FirstName + ", " + LastName + ", " + DateOfBirth;
        }
    }
}