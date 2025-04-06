namespace WorkChronicle.ViewModels
{
    public partial class SchedulePageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private IShift? selectedShift;

        public IRelayCommand<SelectionChangedEventArgs> ShiftSelectedCommand { get; }


        private WorkShiftRepositoryDB shiftRepo;

        private WorkScheduleRepositoryDB scheduleRepo;

        [ObservableProperty]
        private ISchedule<IShift> schedule;

        //[ObservableProperty]
       // private IList<object> selectedShiftsForRemove;

        //[ObservableProperty]
       // private ObservableCollection<IShift> shiftCollectionView;

        [ObservableProperty]
        private string textMessage = "";

        [ObservableProperty]
        private string hoursMessage = "";

        public SchedulePageViewModel(ISchedule<IShift> schedule, WorkShiftRepositoryDB shiftRepositoryDB, WorkScheduleRepositoryDB scheduleRepositoryDB)
        {
            this.ShiftSelectedCommand = new AsyncRelayCommand<SelectionChangedEventArgs>(OnShiftSelected!);

            this.schedule = schedule;
            //this.ShiftCollectionView = new ObservableCollection<IShift>();
           // this.SelectedShiftsForRemove = new List<object>();

            this.shiftRepo = shiftRepositoryDB;
            this.scheduleRepo = scheduleRepositoryDB;
        }


        private async Task OnShiftSelected(SelectionChangedEventArgs args)
        {
            // Check if the selected item is null or not
            if (this.SelectedShift == null)
                return;


            //var popup = new ShiftCompensationPopup(this.SelectedShift!);
            // await Shell.Current.CurrentPage.ShowPopupAsync(popup);

            //-TO DELTE !!
            //if (args.CurrentSelection.FirstOrDefault() is IShift selected)
            //{
            //    if (!selected.IsCurrentMonth)
            //    {
            //        // Deselect the item
            //        this.SelectedShift = null;
            //        return;
            //    }


            //    var popup = new ShiftCompensationPopup(this.SelectedShift!);
            //    await Shell.Current.CurrentPage.ShowPopupAsync(popup);

            //    // Do whatever logic you want with the valid selection
            //    this.SelectedShift = selected;
            //}
        }

        public async Task RefreshThePage()
        {

            if (Schedule == null)
            {
                Console.WriteLine("Schedule is NULL!");
                return;
            }

            if (Schedule.WorkSchedule == null)
            {
                Console.WriteLine("Schedule.WorkSchedule is NULL!");
                return;
            }

            if (Schedule.WorkSchedule.Count == 0)
            {
                TextMessage = "You have no shifts for this month.";
            }
            else
            {
                DateTime startDateNew = Schedule.WorkSchedule
                                            .Where(s=>s.IsCurrentMonth==true)
                                            .First()
                                            .GetDateShift();

                await GenerateShiftDetails(this.Schedule, startDateNew);
            }
        }

        private async Task GenerateShiftDetails(ISchedule<IShift> schedule, DateTime startDate)
        {
            int totalHours = await schedule.CalculateTotalWorkHours();

            KeyValuePair<int, string[]> monthByHoursTotal = Provider.GetMonthHoursTotal(startDate);

            string monthName = Provider.GetMonthName(monthByHoursTotal.Key);
            int totalHoursByMonth = int.Parse(monthByHoursTotal.Value[1]);

            TextMessage = $"Your total hours are: {totalHours}, for the month {monthName} the working hours are {totalHoursByMonth}";

           // this.ShiftCollectionView = new ObservableCollection<IShift>
           //                                                 (this.Schedule
           //                                                         .WorkSchedule
          //                                                          .Where(s => s.ShiftType != ShiftType.RestDay 
          //                                                            && s.IsCompensated == false));

            //int compensatedShiftsCount = await this.Schedule.TotalCompansatedShifts(); - To Delete ????

            if (totalHours > totalHoursByMonth)
            {
                HoursMessage = $"You have {totalHours - totalHoursByMonth} hours of overtime, you have to choose which shift to compensate.";
            }
            else
            {
                HoursMessage = $"You have {totalHoursByMonth - totalHours} under the total hours for the month.";
            }
        }


        [RelayCommand]
        private async Task CompensateButton()
        {
            if (this.SelectedShift == null)
                return;

            this.SelectedShift.IsCompensated = true;

            DbShift dbShift = new DbShift
            {
                ShiftType = this.SelectedShift.ShiftType,
                Year = this.SelectedShift.Year,
                Month = this.SelectedShift.Month,
                Day = this.SelectedShift.Day,
                StarTime = this.SelectedShift.StarTime,
                ShiftHour = this.SelectedShift.ShiftHour,
                IsCompensated = this.SelectedShift.IsCompensated,
                IsCurrentMonth = this.SelectedShift.IsCurrentMonth
            };

            await this.shiftRepo.UpdateShift(dbShift);

            await Shell.Current.DisplayAlert("Success", "Your shift has been compensated.", "OK");

        }


        [RelayCommand]
        private async Task EditShift()
        {
            if (this.SelectedShift == null)
                return;

            var popup = new ShiftCompensationPopup(this.SelectedShift!);
            await Shell.Current.CurrentPage.ShowPopupAsync(popup);

            DbShift dbShift = new DbShift
            {
                ShiftType = this.SelectedShift.ShiftType,
                Year = this.SelectedShift.Year,
                Month = this.SelectedShift.Month,
                Day = this.SelectedShift.Day,
                StarTime = this.SelectedShift.StarTime,
                ShiftHour = this.SelectedShift.ShiftHour,
                IsCompensated = this.SelectedShift.IsCompensated,
                IsCurrentMonth = this.SelectedShift.IsCurrentMonth
            };

            await this.shiftRepo.UpdateShift(dbShift);

            await Shell.Current.DisplayAlert("Success", "Your shift has been compensated.", "OK");

        }

        //[RelayCommand]
        //private async Task RemoveShift()
        //{
        //    if (SelectedShiftsForRemove == null || SelectedShiftsForRemove.Count == 0)
        //        return;

        //    await Task.Delay(10);

        //    foreach (IShift shift in SelectedShiftsForRemove)
        //    {
        //        foreach (var s in Schedule.WorkSchedule
        //                                   .Where(s => s.Equals(shift)))
        //        {
        //            s.IsCompensated = true;
        //        }
        //    }

        //    SelectedShiftsForRemove.Clear();

        //    await RefreshThePage();
        //}

        [RelayCommand]
        private async Task CompensateShift()
        {
            await Shell.Current.GoToAsync(nameof(CompensateShiftsPage));
        }

        [RelayCommand]
        private async Task SaveShiftSchedule()
        {
            int year = this.Schedule.WorkSchedule
                                    .Where(s=>s.IsCurrentMonth)
                                    .First()
                                    .Year;

            int month = this.Schedule.WorkSchedule
                                     .Where(s=>s.IsCurrentMonth)
                                     .First()
                                     .Month;

            var scheduleDb = new DbSchedule 
            { 
                ScheduleName = $"{Provider.GetMonthName(month)} {year}",
                Year = year,
                Month = month,
            }; 
            
            await scheduleRepo.AddSchedule(scheduleDb);

            foreach (var shifts in Schedule.WorkSchedule)
            {
                DbShift dbShift = new DbShift
                {
                    DbScheduleId = scheduleDb.Id,
                    ShiftType = shifts.ShiftType,
                    Year = shifts.Year,
                    Month = shifts.Month,
                    Day = shifts.Day,
                    StarTime = shifts.StarTime,
                    ShiftHour = shifts.ShiftHour,
                    IsCompensated = shifts.IsCompensated,
                    IsCurrentMonth = shifts.IsCurrentMonth
                };

                await shiftRepo.AddShift(dbShift);
            }
                        
            await Shell.Current.DisplayAlert("Success", "Your schedule has been saved.", "OK");
        }

        [RelayCommand]
        private async Task GoBackButton()
        {
            await Shell.Current.GoToAsync("///MainPage");
        }
    }
}
