using AutoLot.Dal.Repos.Base;
using AutoLot.Models.Entities;
using AutoLot.Models.ViewModels;

namespace AutoLot.Dal.Repos.Interfaces;

public interface IOrderRepo : IRepo<Order>
{
    IQueryable<CustomerOrderViewModel> GetOrdersViewModel();
}