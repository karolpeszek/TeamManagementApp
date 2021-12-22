using System;
using System.Collections.Generic;
using System.Text;

namespace FirmaGUI
{
    internal class Team : ICloneable
    {
        protected int _TeamMemberCount { get; set; }
        protected string _TeamName { get; set; }
        TeamManager _Manager { get; set; }
        public int TeamMemberCount { get { return _TeamMemberCount; } set { _TeamMemberCount = value; } }
        public string TeamName { get { return _TeamName;} set { _TeamName = value; } }
        public TeamManager Manager { get { return _Manager; } set { _Manager = value; } }
        List<TeamMember> _Members;
        public List<TeamMember> Members { get { return _Members; } set { _Members = value; } }
        public Team() : base()
        {
            _TeamMemberCount = 0;
            _TeamName = null;
            _Manager = null;
            _Members=new List<TeamMember>();

        }
        public Team(string Name, TeamManager Manager):base()
        {
            _TeamName = Name;
            _Members = new List<TeamMember>();
            _TeamMemberCount = 0;
            _Manager = Manager;
        }

        public void AddMember(TeamMember Member)
        {
            _Members.Add(Member);
            _TeamMemberCount++;
        }
        public override string ToString()
        {
            string result = $"Name : {_TeamName}\n" +
                $"Manager : {_Manager.ToString()}\n" +
                $"Members ({_TeamMemberCount}):\n";
            foreach (var Member in _Members)
                result += $"{Member.ToString()}\n";
            return result;
        }
        public bool IsTeamMember(string _PESEL)
        {
            foreach (var M in _Members) if (M.PESEL == _PESEL) return true;
            return false;
        }
        public void FireTeamMember(string _PESEL)
        {
            bool wasFired = false;
            for (int i = 0; i < _TeamMemberCount; i++) if (_Members[i].PESEL == _PESEL)
                {
                    wasFired = true;
                    _TeamMemberCount--;
                    _Members.RemoveAt(i);
                    return;
                }
            if (wasFired) return;
            throw new Exception("MemberNotFound");
        }
        public void FireTeamMember(string _Name, string _Surname)
        {
            bool wasFired = false;
            for (int i = 0; i < _TeamMemberCount; i++) if (_Members[i].Name == _Name && _Members[i].Surname == _Surname)
                {
                    wasFired = true;
                    _TeamMemberCount--;
                    _Members.RemoveAt(i);
                    return;
                }
            if(wasFired) return;
            throw new Exception("MemberNotFound");
        }
        public void FireTeamMember(int Index)
        {
            _Members.RemoveAt(Index);
            _TeamMemberCount--;
        }
        public void FireEveryone()
        {
            _TeamMemberCount = 0;
            _Members = new List<TeamMember>();
        }

        public List<TeamMember>FindMemberByFunction(string _Function)
        {
            return _Members.FindAll(
                delegate (TeamMember M)
                {
                    return M.Function == _Function;
                }
                );
        }
        public List<TeamMember>FindMemberByJoinMonth(int _Month)
        {
            return _Members.FindAll(
                delegate (TeamMember M)
                {
                    return M.JoinDate.Month == _Month;
                }
                );
        }
        public object Clone()
        {
            var clone = new Team();
            clone.TeamName = _TeamName;
            clone.Manager = _Manager;
            foreach(var TeamMember in _Members)
                clone.AddMember(TeamMember);
            return clone;
        }
        public void Sort()
        {
            _Members.Sort();
        }
        internal class ComparePesel : IComparer<TeamMember>
        {
            public int Compare(TeamMember x, TeamMember y)
            {
                return String.Compare(x.PESEL, y.PESEL);
            }
        }
        public void SortByPesel()
        {
            _Members.Sort(new ComparePesel());
        }
    }
}
