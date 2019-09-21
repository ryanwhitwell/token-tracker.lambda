using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Token.DataAccess.Interfaces;
using Token.Models;

namespace Token.DataAccess
{
  public class TokenUserRepository : ITokenUserRepository
  {
    private IDynamoDBContext _context;

    public TokenUserRepository(IDynamoDBContext context)
    {
      _context = context ?? throw new ArgumentNullException("context");
    }

    public async Task Save(TokenUser user)
    {
      await _context.SaveAsync<TokenUser>(user);
    }

    public async Task Delete(string id)
    {
      await _context.DeleteAsync<TokenUser>(id);
    }

    public async Task<TokenUser> Load(string id)
    {
      TokenUser user = await _context.LoadAsync<TokenUser>(id);

      return user;
    }
  }
}