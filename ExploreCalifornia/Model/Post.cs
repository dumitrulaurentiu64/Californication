using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/* To be a valid database entity, the post class must define a property that will act as the primary key
 * Entity Framework calls this property ID, long type used to indicate that this should be a incremental numeric ID
 * 
 * A key property is added as well, this represents a unique string that can be used to find a post, as oposed to passing
 * ugly id numbers everywhere. That's all the code needed to define the DbContext that entityFramework needs.
 * 
 * More configuration is needed before the dbCOntext can be used to connect to a database. ( -> startup.cs -> ConfServices())
 * The DataContext class needs to be registered with the dependency injection framework, so the consumers don't need to
 * know how to create instances of it.  */

namespace ExploreCalifornia.Model
{
    public class Post  
    {
        public long Id { get; set; }
        private string _key;
        public string Key
        {
            get
            {
                if(_key == null)
                {
                    _key = Regex.Replace(Title.ToLower(), "[^a-z0-9]", "-");
                }
                return _key;
            }
            set { _key = value; }
        }

        [Display(Name = "Post Title")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 5, 
            ErrorMessage = "Title must be between 5 and 100 characters long")]
        public string Title { get; set; }
        public string Author { get; set; }
        [Required]
        [MinLength(100, ErrorMessage = "Blog posts must be at least 100 characters long")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
        public DateTime Posted { get; set; }
    }
}
