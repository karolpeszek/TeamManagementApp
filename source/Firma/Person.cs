using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Firma
{
    public enum genders : ushort
    {
        unset,
        female,
        male,
        nonbinary,
        genderfluid
    }
    public abstract class Person : IEquatable<Person>
    {
        protected string _Name;
        protected string _Surname;
        protected DateTime _Birthday;
        protected string _PESEL;
        protected genders _Gender;

        public string Name { get { return _Name; } set { _Name = value; } }
        public string Surname { get { return _Surname; } set { _Surname = value; } }
        public DateTime Birthday { get { return _Birthday; } set { _Birthday = value; } }
        public string PESEL { get { return _PESEL; } set { _PESEL = value; } }
        public genders Gender { get { return _Gender; } set { _Gender = value; } }
        public Person()
        {
            _Name = null;
            _Surname = null;
            _Birthday = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            _PESEL = "00000000000";
            _Gender = 0;
        }
        public Person(string Name, string Surname, string Birthday, string PESEL, genders Gender)
        {
            _Name = Name;
            _Surname = Surname;
            DateTime.TryParseExact(Birthday, new[] { "yyyy-MM-dd", "yyyy/MM/dd", "MM/dd/yy", "dd-MMM-yy", "dd.MM.yyyy" }, null, DateTimeStyles.None, out _Birthday);
            _Gender = Gender;
            this._PESEL = PESEL;
        }
        public int Age()
        {
            DateTime now = DateTime.Now;
            TimeSpan _age = now - _Birthday;
            return (int)(_age.TotalDays / 365.25);
        }
        /*
        new public string ToString()
        {
            return $"{PESEL}: {name} {surname}";
        }
        */
        public override string ToString()
        {
            return $"{_Name} {_Surname} ({Age()})";
        }
        public bool Equals(Person other)
        {
            return _PESEL == other.PESEL;
        }
    }

    public class TeamMember : Person, ICloneable, IComparable<TeamMember>
    {
        DateTime _JoinDate;
        protected string _MemberFunction;
        public string Function { get { return _MemberFunction; } set { _MemberFunction = value; } }
        public DateTime JoinDate { get { return _JoinDate; } set { _JoinDate = value; } }
        public TeamMember()
        {
            base._Name = null;
            base._Surname = null;
            base._Birthday = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            _JoinDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            base._PESEL = "00000000000";
            base._Gender = 0;
            _MemberFunction = "bezrobotny";
        }
        public TeamMember(string Name, string Surname, string Birthday, string _PESEL, genders Gender, string MemberFunction, string JoinDate)
        {
            base._Name = Name;
            base._Surname = Surname;
            base._Gender = Gender;
            base._PESEL = _PESEL;
            _MemberFunction = MemberFunction;
            DateTime.TryParseExact(Birthday, new[] { "yyyy-MM-dd", "yyyy/MM/dd", "MM/dd/yy", "dd-MMM-yy", "dd.MM.yyyy" }, null, DateTimeStyles.None, out _Birthday);
            DateTime.TryParseExact(JoinDate, new[] { "yyyy-MM-dd", "yyyy/MM/dd", "MM/dd/yy", "dd-MMM-yy", "dd.MM.yyyy" }, null, DateTimeStyles.None, out _JoinDate);
        }
        public override string ToString()
        {
            return $"{base._Name} {base._Surname} ({base.Age()}) - {_MemberFunction}, joined {_JoinDate}";
        }

        public object Clone()
        {

            var clone = new TeamMember();
            clone._Name = _Name;
            clone._Surname = _Surname;
            clone._Gender = _Gender;
            clone._PESEL = _PESEL;
            clone._MemberFunction = _MemberFunction;
            clone._Birthday = _Birthday;
            return clone;
        }

        public int CompareTo(TeamMember other)
        {
            return this.Surname == other.Surname ? String.Compare(this.Name, other.Name) : String.Compare(this.Surname, other.Surname);
        }
    }

    public class TeamManager : Person, ICloneable
    {
        int _Experience;
        public int Experience { get { return _Experience; } set { _Experience = value; } }
        public TeamManager()
        {
            base._Name = null;
            base._Surname = null;
            base._Birthday = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            base._PESEL = "00000000000";
            base._Gender = 0;
            _Experience = -1;
        }
        public TeamManager(string Name, string Surname, string Birthday, string _PESEL, genders Gender, int Experience)
        {
            base._Name = Name;
            base._Surname = Surname;
            base._Gender = Gender;
            base._PESEL = _PESEL;
            _Experience = Experience;
            DateTime.TryParseExact(Birthday, new[] { "yyyy-MM-dd", "yyyy/MM/dd", "MM/dd/yy", "dd-MMM-yy", "dd.MM.yyyy" }, null, DateTimeStyles.None, out _Birthday);
        }
        public override string ToString()
        {
            return $"{base._Name} {base._Surname} ({base.Age()}) - Team Manager (Experience: {_Experience})";
        }

        public object Clone()
        {
            var clone = new TeamManager();
            clone.Name = this._Name;
            clone.Surname = this._Surname;
            clone.Gender = this._Gender;
            clone.PESEL = this._PESEL;
            clone.Gender = this._Gender;
            clone.Experience = this._Experience;
            return clone;
        }

    }
}
