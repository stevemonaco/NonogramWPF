using Microsoft.Win32;
using NonogramWPF.Model;
using Stylet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Threading;

namespace NonogramWPF.ViewModels
{
    class ShellViewModel : Screen
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private DateTime _startTime;

        private NonogramBoardViewModel _nonogramBoard;
        public NonogramBoardViewModel NonogramBoard
        {
            get => _nonogramBoard;
            set => SetAndNotify(ref _nonogramBoard, value);
        }

        private TimeSpan? _timeElapsed;
        public TimeSpan? TimeElapsed
        {
            get => _timeElapsed;
            set => SetAndNotify(ref _timeElapsed, value);
        }

        private string _puzzleName;
        public string PuzzleName
        {
            get => _puzzleName;
            set => SetAndNotify(ref _puzzleName, value);
        }

        public void OpenPuzzle()
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Title = "Open new puzzle";
            ofd.ValidateNames = true;
            ofd.CheckFileExists = true;
            ofd.DefaultExt = ".xml";
            ofd.Filter = "Puzzle Files|*.xml";

            if (ofd.ShowDialog() == true)
            {
                var board = new NonogramMatrix();
                board.LoadFromXml(ofd.FileName);

                var vm = new NonogramBoardViewModel(board);
                NonogramBoard = vm;
                PuzzleName = Path.GetFileNameWithoutExtension(ofd.FileName);


                _timer.Stop();
                _startTime = DateTime.Now;
                TimeElapsed = new TimeSpan(0);
                _timer.Interval = TimeSpan.FromSeconds(1);
                _timer.Tick += Timer_Tick;
                _timer.Start();
            }
        }

        public void ExitApplication()
        {
            Environment.Exit(0);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeElapsed = DateTime.Now - _startTime;
        }
    }
}
