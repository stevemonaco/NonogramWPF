using Microsoft.Win32;
using Nonogram.WPF.EventModels;
using Nonogram.Domain;
using Stylet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Threading;

namespace Nonogram.WPF.ViewModels
{
    class ShellViewModel : Screen, IHandle<GameWinEventModel>
    {
        private readonly IEventAggregator _events;
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

        public ShellViewModel(IEventAggregator events)
        {
            _events = events;
            _events.Subscribe(this);
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

                var vm = new NonogramBoardViewModel(board, _events);
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

        public void Handle(GameWinEventModel message)
        {
            TimeElapsed = DateTime.Now - _startTime;
            _timer.Stop();
        }
    }
}
