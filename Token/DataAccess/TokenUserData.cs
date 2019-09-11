using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Token.Core;
using Token.DataAccess.Interfaces;
using Token.Models;

namespace Token.DataAccess
{
    public class TokenUserData: ITokenUserData
    {
        private static readonly DateTime EPOCH_DATE = new DateTime(1970, 1, 1);
        private static readonly int TTL_MINUTES = Int32.Parse(Configuration.File.GetSection("Application")["DataTimeToLiveMinutes"]);
        private ILogger<TokenUserData> logger;
        private ITokenUserRepository tokenUserRepository;
        public TokenUserData(ILogger<TokenUserData> logger, ITokenUserRepository tokenUserRepository)
        {
            if (logger is null)
            {
                throw new ArgumentNullException("logger");
            }

            if (tokenUserRepository is null)
            {
                throw new ArgumentNullException("tokenUserRepository");
            }

            this.logger = logger;
            this.tokenUserRepository = tokenUserRepository;
        }

        public async Task Delete(string id)
        {
            await this.tokenUserRepository.Delete(id);
        }

        public async Task<bool> Exists(string id)
        {
            TokenUser tokenUser = await this.tokenUserRepository.Load(id);

            return tokenUser != null;
        }

        public async Task<TokenUser> Get(string id)
        {
            TokenUser tokenUser = await this.tokenUserRepository.Load(id);

            return tokenUser;
        }

        public async Task Save(TokenUser tokenUser)
        {
            DateTime utcNow = DateTime.UtcNow;

            tokenUser.CreateDate = tokenUser.CreateDate ?? utcNow;
            tokenUser.UpdateDate = utcNow;
            
            tokenUser.TTL = tokenUser.HasPointsPersistence ? (long?)null : TokenUserData.GetTTL(tokenUser.CreateDate);
            
            await this.tokenUserRepository.Save(tokenUser);
        }

        private static long GetTTL(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return (long)(dateTime.Value.AddMinutes(TTL_MINUTES) - EPOCH_DATE).TotalSeconds;;
            }

            throw new ArgumentNullException("dateTime");
        }
    }
}