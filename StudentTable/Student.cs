using System;
using Microsoft.WindowsAzure.Storage;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace StudentTable
{
    public class StudentEntity : TableEntity
    { 
        string _name, _surname;
        

        public StudentEntity(string lastName, string firstName, string email, string homeAddress, string phoneNumber)
        {
            this.PartitionKey = lastName;
            this.RowKey = firstName;
            _name = firstName;
            _surname = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public StudentEntity() { }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName
        {
            get
            {
                return _surname;
            }
            set
            {
                _surname = value;
                this.PartitionKey = value;
            }
        }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                this.RowKey = value;
            }
        }

        [Required]
        [Display(Name = "Email is Required!")]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number is Required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                  ErrorMessage = "Entered Contact number format is not valid.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Status")]
        public bool? IsActive { get; set; }
        public string URI { get; set; }

        public string BlobName { get; set;}
    }
}
//Student No
//• First Name
//• Last Name
//• Email
//• Home Address
//• Mobile
//• Upload photo of Student
//• isActive – a flag indicating if the student is a valid