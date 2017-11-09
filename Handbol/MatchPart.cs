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
    class MatchPart
    {
        enum MatchPossiblePoints
        {
            PointsForWin = 3,
            PointsForDraw = 2,
            PointsForLoose = 1
        }

        int _PartOrdinal;
        int _LocalGoals;
        int _VisitorGoals;
        int _LocalPoints;
        int _VisitorPoints;


        public MatchPart(int partOrdinal)
        {
            _PartOrdinal = partOrdinal;
            _LocalGoals = 0;
            _VisitorGoals = 0;
        }

        public int PartOrdinal { get => _PartOrdinal; }
        public int LocalGoals { get => _LocalGoals; set => _LocalGoals = value; }
        public int VisitorGoals { get => _VisitorGoals; set => _VisitorGoals = value; }
        public int LocalPoints {
            get {
                CalculatePoints();
                return _LocalPoints;
            }
        }
        public int VisitorPoints
        {
            get
            {
                CalculatePoints();
                return _VisitorPoints;
            }
        }

        public void GoalsChangeLocal(int change) {

            _LocalGoals += change;

        }

        public void GoalsChangeVisitor(int change)
        {

            _VisitorGoals += change;

        }


        private void CalculatePoints() {

            switch (_LocalGoals.CompareTo(_VisitorGoals))
            {
                case -1:
                    _LocalPoints = (int)MatchPossiblePoints.PointsForLoose;
                    _VisitorPoints = (int)MatchPossiblePoints.PointsForWin;
                    break;
                case 1:
                    _LocalPoints = (int)MatchPossiblePoints.PointsForWin;
                    _VisitorPoints = (int)MatchPossiblePoints.PointsForLoose;
                    break;
                default:
                    _LocalPoints = (int)MatchPossiblePoints.PointsForDraw;
                    _VisitorPoints = (int)MatchPossiblePoints.PointsForDraw;
                    break;
            }

        }




    }
}