using AutoLot.Dal.Repos.Base;
using AutoLot.Models.Entities;

namespace AutoLot.Dal.Repos.Interfaces;

public interface ICarRepo : IRepo<Car>
{
    IEnumerable<Car> GetAllBy(int makeId);
    string GetPetName(int id);
}