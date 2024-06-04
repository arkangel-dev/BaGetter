using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaGetter.Core;
public class User
{
    public int Key { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }
    public bool IsAdmin { get; set; }
    public List<Package> Packages { get; set; } = new List<Package>(); 
}
