using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lab6.Models.Prediction
{
    public enum Questions
    {
        Earth, Computer
    }
    public class Prediction
    {

        [Required]
        [DisplayName("Prediction Id")]
        public int PredictionId { get; set; }

        [Required]
        [DisplayName("File Name")]
        public string FileName { get; set; }

        [Required]
        [Url]
        [DisplayName("URL")]
        public string Url { get; set; }

        [Required]
        public Questions Questions { get; set; }

    }
}
