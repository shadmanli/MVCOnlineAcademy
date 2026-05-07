namespace Academy.Models
{

        public class Course:BaseEntity
        {

            public string Title { get; set; }
            public string Description { get; set; }

            public decimal Price { get; set; }

            public string ImageUrl { get; set; }

            public double Rating { get; set; }

        
            public int LanguageId { get; set; }
            public Language Language { get; set; }

      
            public int CategoryId { get; set; }
            public Category Category { get; set; }

            public int InstructorId { get; set; }
            public Instructor Instructor { get; set; }

      
            public List<Lesson> Lessons { get; set; }
            public List<Video> Videos { get; set; }
            public List<Review> Reviews { get; set; }
            public List<Enrollment> Enrollments { get; set; }
          
            public int Duration { get; set; } 
            public int StudentCount { get; set; }
        public List<CourseRequirement> Requirements { get; set; }
        public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }

        public List<CourseFeature> Features { get; set; }
        public DateTime CreatedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }
        }
    
}
