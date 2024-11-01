using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike?> GetUserLike(int sourUserId, int targetUserId);
        Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams);   // * pagination added
        Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentId);
        void DeleteLike(UserLike like);
        void AddLike(UserLike like);
        Task<bool> SaveChanges();
    }
}