using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Handbol
{
    class Match
    {
        private List<MatchPart> MatchParts = new List<MatchPart>();

        private int _CurrentPart;

        public int CurrentPart { get => _CurrentPart; set => _CurrentPart = value; }

        public Match(int howManyParts)
        {
            for (int i = 1; i <= howManyParts; i++)
            {
                MatchParts.Add(new MatchPart(i));
            }

            _CurrentPart = 1;

        }

        public void AddGoalToLocal() {

            MatchParts[_CurrentPart - 1].GoalsChangeLocal(1);

        }

        public void AddGoalToVisitor()
        {

            MatchParts[_CurrentPart - 1].GoalsChangeVisitor(1);

        }

        public void DecreaseGoalToLocal()
        {

            MatchParts[_CurrentPart - 1].GoalsChangeLocal(-1);

        }

        public void DecreaseGoalToVisitor()
        {

            MatchParts[_CurrentPart - 1].GoalsChangeVisitor(-1);

        }


        public string GetFinalPointsMarker() {

            string marker;

            int localPoints = 0;
            int visitorPoints = 0;

            foreach (var oneMatchPart in MatchParts)
            {
                localPoints += oneMatchPart.LocalPoints;
                visitorPoints += oneMatchPart.VisitorPoints;

            }

            marker = string.Format("Final: {0} - {1}", localPoints, visitorPoints);

            return marker;

        }

        public string GetGoalsMarker(int part)
        {

            string marker;

            marker = string.Format("Q{0}: {1} - {2} --> Score: {3} - {4}", part, MatchParts[part - 1].LocalGoals, MatchParts[part - 1].VisitorGoals
                                                                        , MatchParts[part - 1].LocalPoints, MatchParts[part - 1].VisitorPoints);

            return marker;

        }

        public void ResetAllValues() {

            foreach (var oneMatchPart in MatchParts)
            {
                oneMatchPart.LocalGoals = 0;
                oneMatchPart.VisitorGoals = 0;

            }


        }


    }
}