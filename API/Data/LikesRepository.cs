using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace API.Data
{
  public class LikesRepository(DataContext context, IMapper mapper) : ILikesRepository
  {
    public void AddLike(UserLike like)
    {
      context.Likes.Add(like);
    }

    public void DeleteLike(UserLike like)
    {
      context.Likes.Remove(like);
    }

    // * get currently logged in user liked users' id
    public async Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentId)
    {
      return await context.Likes
          .Where(x => x.SourceUserId == currentId)
          .Select(x => x.TargetUserId)
          .ToListAsync();
    }

    // * get userlike entry
    public async Task<UserLike?> GetUserLike(int sourUserId, int targetUserId)
    {
      return await context.Likes.FindAsync(sourUserId, targetUserId);
    }

    // * get list of userlike entry based on predicate asked then returned as Memberdto
    public async Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams)
    {
      var likes = context.Likes.AsQueryable();

      IQueryable<MemberDto> query;

      switch (likesParams.Predicate)
      {
        case "liked":
          query = likes
            .Where(x => x.SourceUserId == likesParams.UserId)
            .Select(x => x.TargetUser)
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider);
          break;
        case "likedBy":
          query = likes
            .Where(x => x.TargetUserId == likesParams.UserId)
            .Select(x => x.SourceUser)
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider);
          break;
        default:
          // * mutual likes
          var likeIds = await GetCurrentUserLikeIds(likesParams.UserId);
          query = likes
            .Where(x => x.TargetUserId == likesParams.UserId && likeIds.Contains(x.SourceUserId))
            .Select(x => x.SourceUser)
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider);
          break;
      }
      return await PagedList<MemberDto>.CreateAsync(query, likesParams.PageNumber, likesParams.PageSize);
    }

    // * save
    public async Task<bool> SaveChanges()
    {
      return await context.SaveChangesAsync() > 0;
    }
  }
}