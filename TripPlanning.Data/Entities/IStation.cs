using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripPlanning.Data.Entities
{
    public interface IStation
    {
        string StationName { get; set; }
    }
}