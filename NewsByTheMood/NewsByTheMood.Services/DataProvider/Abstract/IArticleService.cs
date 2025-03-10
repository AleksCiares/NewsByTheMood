﻿using NewsByTheMood.Data.Entities;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of articles provider service
    public interface IArticleService
    {
        // Get article count with certain positivity
        public Task<int> CountAsync(short positivity);

        // Get latest articles range with certain positivity
        public Task<Article[]> GetRangeLatestAsync(short positivity, int pageNumber, int pageSize);

        // Get article count with certain positivity and topic
        public Task<int> CountByTopicAsync(short positivity, Int64 topicId);

        // Get latest articles range with certain positivity and topic
        public Task<Article[]> GetRangeByTopicAsync(short positivity, Int64 topicId, int pageNumber, int pageSize);

        // Get article by certain id
        public Task<Article?> GetByIdAsync(Int64 id);

        // Get range off articles preview with certain positivity and rating
        /*public Task<Article[]> GetRangePreviewAsync(int pageNumber, int pageSize, short positivity, int rating);*/

        // Is exist article with current url
        public Task<bool> IsExistByUrl(string articleUrl);

        public Task AddAsync(Article article);
    }
}
