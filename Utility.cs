using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateSaver
{
    public class Utility
    {
        private readonly LocalDbService _dbService;
        private DateTime currentDate = DateTime.Now;

        public Utility(LocalDbService dbService)
        {
            _dbService = dbService;
        }


        public async void SetupDateListViewTest()
        {
            List<Date> resultsFromSQL = await _dbService.GetDates();

            foreach (Date date in resultsFromSQL)
            {
                //Calculate date countdown
                date.CountDown = (date.DateSaved.Date - currentDate.Date).Days;

                //Check if date needs to be repeated
                if (date.CountDown < 0 && date.RepeatDate == true)
                {
                    await _dbService.Update(new Date
                    {
                        Id = date.Id,
                        DateName = date.DateName,
                        //Description = date.Description,
                        DateSaved = date.DateSaved.AddYears(1),
                        RepeatDate = date.RepeatDate
                    });

                    //Recalculate date countdown
                    date.CountDown = (date.DateSaved.Date - currentDate.Date).Days;
                }
            }

            //listView.ItemsSource = resultsFromSQL;
        }
    }
}
