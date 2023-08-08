using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoApp.Models.Entities
{
    /// <summary>
    /// This is our domain entity: ToDo 
    /// </summary>
    public class ToDo
    {
        // [Key] add Pk in her using key attribute or use it in model creation method in dbcontext
        public int Id { get; set; }
        [Required]
        public string Detail { get; set; } = string.Empty;

        /// <summary>
        /// CompletionDate is a nullable property
        /// </summary>
        [Display(Name = "Completion Date")]
        [DisplayFormat(DataFormatString = "{0:MMMM/dd/yyyy")]
        public DateTime? CompletionDate { get; set; }

        [Display(Name = "Is Completed ?")]

        /// <summary>
        /// IsCompleted is a property that defines whether or not a To Do item is completed or not.
        /// </summary>
        public bool IsCompleted { get; set; }
    }
}