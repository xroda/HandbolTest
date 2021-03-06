﻿using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Media;

namespace Handbol
{
    [Activity(Label = "Handbol", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        Match CurrentMatch;
        long timeWhenStopped = 0;
        bool userWantsSound = false;

        enum SoundType {
            Fail = 0, Success = 1, Error = 2
        }

        string ResultPart1Property { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            CurrentMatch = new Match(4);
            SetCurrentPartMoreVisible();

            #region Parts Buttons

            Button btnPart1 = FindViewById<Button>(Resource.Id.btnPart1);
            btnPart1.Click += BtnPart1_clicked;

            Button btnPart2 = FindViewById<Button>(Resource.Id.btnPart2);
            btnPart2.Click += BtnPart2_clicked;

            Button btnPart3 = FindViewById<Button>(Resource.Id.btnPart3);
            btnPart3.Click += BtnPart3_clicked;

            Button btnPart4 = FindViewById<Button>(Resource.Id.btnPart4);
            btnPart4.Click += BtnPart4_clicked;


            #endregion

            #region Goals Buttons

            Button btnGoalAddToLocal = FindViewById<Button>(Resource.Id.btnGoalAddToLocal);
            btnGoalAddToLocal.Click += BtnGoalAddToLocal_clicked;

            Button btnGoalAddToVisitor = FindViewById<Button>(Resource.Id.btnGoalAddToVisitor);
            btnGoalAddToVisitor.Click += BtnGoalAddToVisitor_clicked;

            Button btnGoalDecreaseToLocal = FindViewById<Button>(Resource.Id.btnGoalDecreaseToLocal);
            btnGoalDecreaseToLocal.Click += BtnGoalDecreaseToLocal_clicked;

            Button btnGoalDecreaseToVisitor = FindViewById<Button>(Resource.Id.btnGoalDecreaseToVisitor);
            btnGoalDecreaseToVisitor.Click += BtnGoalDecreaseToVisitor_clicked;

            #endregion

            #region Chronometer

            Button btnTimerPlay = FindViewById<Button>(Resource.Id.btnTimerPlay);
            btnTimerPlay.Click += BtnTimerPlay_clicked;

            Button btnTimerPause = FindViewById<Button>(Resource.Id.btnTimerPause);
            btnTimerPause.Click += BtnTimerPause_clicked;

            Button btnTimerReset = FindViewById<Button>(Resource.Id.btnTimerReset);
            btnTimerReset.Click += BtnTimerReset_clicked;

            #endregion

            Button btnResetAll = FindViewById<Button>(Resource.Id.btnResetAll);
            btnResetAll.Click += BtnResetAll_clicked;

            ToggleButton toggleSound = FindViewById<ToggleButton>(Resource.Id.toggleSound);
            toggleSound.Click += ToggleSound_Click;

        }

        private void PlaySoud(SoundType whatSound) {
            MediaPlayer _player;

            switch (whatSound)
            {
                case SoundType.Fail:
                    _player = MediaPlayer.Create(this, Resource.Raw.bad_trombon);
                    _player.Start();
                    break;
                case SoundType.Success:
                    _player = MediaPlayer.Create(this, Resource.Raw.good_applause);
                    _player.Start();
                    break;
                case SoundType.Error:
                    _player = MediaPlayer.Create(this, Resource.Raw.error_windows_error);
                    _player.Start();
                    break;
            }

        }

        private void ToggleSound_Click(object sender, EventArgs e)
        {
            ToggleButton toggleSound = FindViewById<ToggleButton>(Resource.Id.toggleSound);
            userWantsSound = toggleSound.Checked;
        }

        #region Parts Buttons eventhandlers
        private void BtnPart1_clicked(object sender, EventArgs e)
        {
            CurrentMatch.CurrentPart = 1;
            SetCurrentPartMoreVisible();
            TimerReset();
        }

        private void BtnPart2_clicked(object sender, EventArgs e)
        {
            CurrentMatch.CurrentPart = 2;
            SetCurrentPartMoreVisible();
            TimerReset();
        }

        private void BtnPart3_clicked(object sender, EventArgs e)
        {
            CurrentMatch.CurrentPart = 3;
            SetCurrentPartMoreVisible();
            TimerReset();
        }

        private void BtnPart4_clicked(object sender, EventArgs e)
        {
            CurrentMatch.CurrentPart = 4;
            SetCurrentPartMoreVisible();
            TimerReset();
        }

        #endregion

        #region Buttons eventhandlers for goals control
        private void BtnGoalAddToLocal_clicked(object sender, EventArgs e)
        {
            CurrentMatch.AddGoalToLocal();

            UpdateMarkers();

            if (userWantsSound) PlaySoud(SoundType.Success);

        }

        private void BtnGoalAddToVisitor_clicked(object sender, EventArgs e)
        {
            CurrentMatch.AddGoalToVisitor();

            UpdateMarkers();

            if (userWantsSound) PlaySoud(SoundType.Fail);

        }

        private void BtnGoalDecreaseToLocal_clicked(object sender, EventArgs e)
        {
            CurrentMatch.DecreaseGoalToLocal();

            UpdateMarkers();

            if (userWantsSound) PlaySoud(SoundType.Error);

        }

        private void BtnGoalDecreaseToVisitor_clicked(object sender, EventArgs e)
        {
            CurrentMatch.DecreaseGoalToVisitor();

            UpdateMarkers();

            if (userWantsSound) PlaySoud(SoundType.Error);

        }
        #endregion

        #region Eventhandlers for chrono
        private void BtnTimerPlay_clicked(object sender, EventArgs e)
        {
            Chronometer chrono = FindViewById<Chronometer>(Resource.Id.chronoHelper);
            chrono.Base = SystemClock.ElapsedRealtime() + timeWhenStopped;
            chrono.Start();
        }

        private void BtnTimerPause_clicked(object sender, EventArgs e)
        {
            Chronometer chrono = FindViewById<Chronometer>(Resource.Id.chronoHelper);
            timeWhenStopped = chrono.Base - SystemClock.ElapsedRealtime();
            chrono.Stop();
        }

        private void BtnTimerReset_clicked(object sender, EventArgs e)
        {
            TimerReset();
        }

        private void TimerReset()
        {
            Chronometer chrono = FindViewById<Chronometer>(Resource.Id.chronoHelper);
            chrono.Stop();
            chrono.Base = SystemClock.ElapsedRealtime();
            timeWhenStopped = 0;
        }
        #endregion


        private void BtnResetAll_clicked(object sender, EventArgs e)
        {
            AlertDialog.Builder confirmDialog = new AlertDialog.Builder(this);
            confirmDialog.SetPositiveButton("Yes", (senderDialog, args) => {
                CurrentMatch.CurrentPart = 1;
                CurrentMatch.ResetAllValues();
                SetCurrentPartMoreVisible();
                UpdateMarkers();
                TimerReset();
            });

            confirmDialog.SetNegativeButton("Nope!", (senderDialog, args) => {});

            confirmDialog.SetTitle("Confirmation");
            confirmDialog.SetMessage("Are you sure you want to reset all values?");

            RunOnUiThread(() =>
            {
                confirmDialog.Show();
            });

        }

        private void SetCurrentPartMoreVisible() {

            Button btnPart1 = FindViewById<Button>(Resource.Id.btnPart1);
            Button btnPart2 = FindViewById<Button>(Resource.Id.btnPart2);
            Button btnPart3 = FindViewById<Button>(Resource.Id.btnPart3);
            Button btnPart4 = FindViewById<Button>(Resource.Id.btnPart4);

            btnPart1.SetBackgroundColor(Android.Graphics.Color.Gray);
            btnPart2.SetBackgroundColor(Android.Graphics.Color.Gray);
            btnPart3.SetBackgroundColor(Android.Graphics.Color.Gray);
            btnPart4.SetBackgroundColor(Android.Graphics.Color.Gray);

            TextView txtMarkerPart1 = FindViewById<TextView>(Resource.Id.txtResultPart1);
            TextView txtMarkerPart2 = FindViewById<TextView>(Resource.Id.txtResultPart2);
            TextView txtMarkerPart3 = FindViewById<TextView>(Resource.Id.txtResultPart3);
            TextView txtMarkerPart4 = FindViewById<TextView>(Resource.Id.txtResultPart4);

            txtMarkerPart1.SetTextSize(Android.Util.ComplexUnitType.Sp, 15);
            txtMarkerPart2.SetTextSize(Android.Util.ComplexUnitType.Sp, 15);
            txtMarkerPart3.SetTextSize(Android.Util.ComplexUnitType.Sp, 15);
            txtMarkerPart4.SetTextSize(Android.Util.ComplexUnitType.Sp, 15);

            switch (CurrentMatch.CurrentPart)
            {
                case 1:
                    btnPart1.SetBackgroundColor(Android.Graphics.Color.Green);
                    txtMarkerPart1.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
                    break;
                case 2:
                    btnPart2.SetBackgroundColor(Android.Graphics.Color.Green);
                    txtMarkerPart2.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
                    break;
                case 3:
                    btnPart3.SetBackgroundColor(Android.Graphics.Color.Green);
                    txtMarkerPart3.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
                    break;
                case 4:
                    btnPart4.SetBackgroundColor(Android.Graphics.Color.Green);
                    txtMarkerPart4.SetTextSize(Android.Util.ComplexUnitType.Sp, 20);
                    break;
            }

        }

        private void UpdateMarkers() {

            TextView txtMarkerPart1 = FindViewById<TextView>(Resource.Id.txtResultPart1);
            TextView txtMarkerPart2 = FindViewById<TextView>(Resource.Id.txtResultPart2);
            TextView txtMarkerPart3 = FindViewById<TextView>(Resource.Id.txtResultPart3);
            TextView txtMarkerPart4 = FindViewById<TextView>(Resource.Id.txtResultPart4);
            TextView txtResultTotal = FindViewById<TextView>(Resource.Id.txtResultTotal);

            txtMarkerPart1.Text = CurrentMatch.GetGoalsMarker(1);
            txtMarkerPart2.Text = CurrentMatch.GetGoalsMarker(2);
            txtMarkerPart3.Text = CurrentMatch.GetGoalsMarker(3);
            txtMarkerPart4.Text = CurrentMatch.GetGoalsMarker(4);
            txtResultTotal.Text = CurrentMatch.GetFinalPointsMarker();

        }



    }
}

