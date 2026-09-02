using AutoMapper;
using StoreApi.DTOs;
using StoreApi.Models;

namespace StoreApi.Mapping
{
    public class PaymentProfile:Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentDto>();
        }
    }
}
