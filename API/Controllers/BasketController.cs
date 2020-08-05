using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseAPIController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _mapper = mapper;
            _basketRepository = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket());
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateBasketAsync(CustomerBasketDto basket)
        {
            var result = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            return Ok(await _basketRepository.UpdateBasketAsync(result));
        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }
    }
}