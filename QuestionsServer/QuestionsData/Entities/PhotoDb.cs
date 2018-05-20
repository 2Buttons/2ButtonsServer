using System;
using System.ComponentModel.DataAnnotations;
using CommonLibraries;

namespace QuestionsData.Entities
{
    public class PhotoDb
    {
        [Key]
        public int UserId { get; set; }
        public string Login { get; set; }
        public SexType Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public string SmallAvatarLink { get; set; }
        public string City { get; set; }
    }
}
