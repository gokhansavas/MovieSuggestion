using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSuggestion.Models.Entities.View
{
    public class PaginationParameters
    {
        private const int _maxItemsPerPage = 20;
        private int _itemsPerPage = 10;

        public int Page { get; set; } = 1;

        public int ItemsPerPage { 
            get => _itemsPerPage; 
            set => _itemsPerPage = value > _maxItemsPerPage ? _maxItemsPerPage : value; 
        }
    }

    public class PaginationMetadata
    {
        public PaginationMetadata(int _totalCount, int _currentPage, int _itemsPerPage)
        {
            TotalCount = _totalCount;
            CurrentPage = _currentPage;
            TotalPages = (int)Math.Ceiling(_totalCount / (double)_itemsPerPage);
        }

        public int CurrentPage { get; private set; }

        public int TotalCount { get; private set; }

        public int TotalPages { get; private set; }

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;
    }
}
