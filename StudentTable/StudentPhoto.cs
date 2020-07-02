using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StudentTable
{
    class StudentPhoto
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Please select file.")]
        public HttpPostedFileBase FileUpload { get; set; }
    }
}
