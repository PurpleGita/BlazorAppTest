using BlazorAppTest.Components.Account.Pages;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlazorAppTest.Components.Model
{
    public class Cpr
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CprNumber { get; set; }

        public string Email { get; set; }

        public List<Todolist> Todolist { get; set; }

    }


}