using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taskSquare.Models;

namespace taskSquare.ViewModels
{
    public class TeamViewModel
    {
        public List<TeamMember> teamMembers { get; set; }
        public TitleArea titleArea { get; set; }
    }
}
