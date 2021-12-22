using System;
using System.Globalization;

namespace Firma
{  class Program
    {
        static void Main(string[] args)
        {
            TeamManager manager = new TeamManager("Adam", "Kowalski", "01.07.1990", "90070142412", genders.male, 5);

            TeamMember m1 = new TeamMember("Witold", "Adamski", "22.10.1992", "92102266738", genders.male, "sekretarz", "01.01.2020");
            TeamMember m2 = new TeamMember("Jan", "Janowski", "15.03.1992", "92031532652", genders.male, "programista", "01.01.2020");
            TeamMember m3 = new TeamMember("Jan", "But", "16.05.1992", "92051613915", genders.male, "programista", "01.06.2019");
            TeamMember m4 = new TeamMember("Beata", "Nowak", "22.11.1993", "93112225023", genders.female, "projektant", "01.01.2020");
            TeamMember m5 = new TeamMember("Anna", "Mysza", "22.07.1991", "91072235964", genders.female, "projektant", "31.07.2019");

            Team t = new Team("Grupa IT", manager);

            t.AddMember(m1);
            t.AddMember(m2);
            t.AddMember(m3);
            t.AddMember(m4);
            t.AddMember(m5);
            

            t.FireTeamMember("Jan", "But");                 


            Console.WriteLine(t.ToString());

            Console.WriteLine("Members joined in January:");
            var JoinedInJanuary = t.FindMemberByJoinMonth(1);
            foreach (var m in JoinedInJanuary) Console.WriteLine(m.ToString());
            Console.WriteLine();

            Console.WriteLine("Team Programmers:");
            var Programmers = t.FindMemberByFunction("programista");
            foreach (var p in Programmers) Console.WriteLine(p.ToString());
            Console.WriteLine();
        }
    }
}