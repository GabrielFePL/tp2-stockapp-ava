﻿using StockApp.Domain.Entities;
using StockApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Application.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IOrderRepository _orderRepository;

        public RecommendationService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
    
        public async Task<IEnumerable<Product>> GetRecommendationsAsync(string userId)
        {
            var userOrders = await _orderRepository.GetByUserIdAsync(userId);
            var recommendations = userOrders.SelectMany(order => order.Products)
                                            .GroupBy(product => product.Id)
                                            .OrderByDescending(group => group.Count())
                                            .Select(group => group.First())
                                            .Take(5);
            return recommendations;
        }
    }
}
