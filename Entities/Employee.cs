using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormConversion.Entities
{
    public class Employee
    {
        /// <summary>Used to ensure unique identifiers for each instance</summary>
        private static int _nextId = 1;

        /// <summary>Unique identifier</summary>
        public int Id { get; }

        /// <summary>First Name</summary>
        public string FirstName { get; set; }

        /// <summary>Last Name</summary>
        public string LastName { get; set; }

        /// <summary>Username (should be unique)</summary>
        public string Username { get; set; }

        /// <summary>
        /// Creates a new instance of "Employee" and sets a unique Id value
        /// </summary>
        public Employee()
        {
            Id = _nextId++;
        }
    }
}
