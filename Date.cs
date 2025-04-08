using SQLite;
using System.Diagnostics.CodeAnalysis;

namespace DateSaver
{
    [Table("date")]
    public class Date
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [AllowNull]
        [Column("date_name")]
        public string DateName { get; set; }

        [Column("date_saved")]
        public DateTime DateSaved { get; set; }

        [Column("original_date_saved")]
        public DateTime OriginalDateSaved { get; set; }

        [Column("countdown")]
        public int CountDown { get; set; }

        [Column("repeat_date")]
        public Boolean RepeatDate { get; set; }

        [AllowNull]
        [Column("age")]
        public int Age { get; set; }

        [Column("track_age")]
        public Boolean TrackAge { get; set; }
    }
}
